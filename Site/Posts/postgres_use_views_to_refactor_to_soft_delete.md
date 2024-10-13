;;;
{
	"title": "Postgres: Use Views to Refactor to Soft Delete",
	"description": "Refactors are tough, database refactors are scary. Being a bit clever can save us a lot of pain!",
	"date": "5 October 2024",
	"contents": true,
	"hero": "photo-1560506839-a23b986135a0",
    "topics": ["Databases", "Postgres", "How-To", "Architecture"],
    "related": [
        { "title": "Write Your Own RDBMS Versioned Migration Boilerplate", "description": "Versioned migrations are an essential tool for systems using an RDBMS, and it's no work at all to start your applications the right way with this pattern.", "fileName": "write_your_own_rdbms_versioned_migration_boilerplate" },
        { "title": "Just Use PostgreSQL", "description": "With a vast and growing ecosystem of database systems, data models, patterns, and paradigms, choosing the right one can be a long and complicated process. I prefer a simpler approach: Just use PostgreSQL.", "fileName": "just_use_postgresql" },
        { "title": "Learn the Old Languages", "description": "New languages are hip, old languages are erudite. Don't neglect these languages as you round out your skills.", "fileName": "learn_the_old_languages" }
    ]
}
;;;

_Author's note: I have updated this article with some extra bits of information as well as a repository demonstrating this solution. I hope this is helpful!_

In the world of persistence, there's two main patterns (maybe "groups of patterns") to handle deletion: _hard_ and _soft_ deletion. Hard deletion is the default for most database systems - when you "delete" a record it is wiped from the database; as soon as the delete is committed the data is gone forever. More common in most business scenarios - particularly server-side - is to retain the deleted data, just using a flag to hide the "deleted" data from the customer. This is soft deletion: it allows us to support more comprehensive internal reporting, maintain more complicated referential integrity, and we can support an "undo" button for our users.

If you're designing a system from the ground up and you know you need to accommodate soft deletion, there's a whole host of implementations you can tailor to your needs. If you're updating an existing system from hard to soft delete though, you're more constrained. Yes, the data that has already been hard deleted is irrecoverable after your migration, but that's not your main concern. You might have a lot of tables and there's probably already a lot of code written to query against these tables. This refactor might seem like a lot of work at first glance.

Luckily because [you're definitely using Postgres](https://ian.wold.guru/Posts/just_use_postgresql.html), this really isn't a major concern. There's a simple way using views and rules we can add soft deletion _without changing any of our queries_! In fact, there are two options which you might want to chose in different situations. I've implemented these strategies on a few production systems with zero downtime, and I'm sure you'll be able to make just as quick work of it as I can.

The first strategy keeps all records - those that are deleted and those that aren't - in a single table and uses a view to filter out deleted items. The second strategy adds a second table specifically for deleted items and uses a view to join the deleted and non-deleted records when needed. I have found the first option simpler to implement and maintain, while the second option is sometimes better in scenarios where I need to run a lot of different queries on the deleted vs. non-deleted data. These strategies could feasibly be combined in a single database, with some tables using one strategy and others the other, though consistency is probably better here.

I've set up [a repository on GitHub](https://github.com/IanWold/PostgresRefactorSoftDelete) which runs both of these solutions on a database. If you're seriously considering implementing a migration like this, I would encourage you to fork and play with my repo, I hope it can be an effective way to tinker with some of these ideas before moving into an actual codebase. Throughout the article I'll reference where each bit of code is in this repo.

# Setting Up

I'm going to start by defining the existing system we're going to refactor. Let's consider a subset of an ecommerce system with tables `items`, `carts`, and of course `cart_items`:

```sql
CREATE TABLE items (
    item_id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    created TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE carts (
    cart_id SERIAL PRIMARY KEY,
    user_id INT NOT NULL, -- I won't define this table
    created TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE cart_items (
    cart_item_id SERIAL PRIMARY KEY,
    cart_id INT NOT NULL,
    item_id INT NOT NULL,
    quantity INT NOT NULL DEFAULT 1,
    created TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    CONSTRAINT fk_cart FOREIGN KEY (cart_id) REFERENCES carts (cart_id) ON DELETE CASCADE,
    CONSTRAINT fk_item FOREIGN KEY (item_id) REFERENCES items (item_id) ON DELETE CASCADE
);

-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/Program.cs#L18 [tl! autolink]
```

And we're going to need to support all the various queries currently written against these tables. In addition to all the queries `SELECT`ing from these tables, we're inserting, updating, and indeed deleting from each of these tables - those queries all need to remain the same.

There's a couple interesting queries I want to consider. Take the scenario where a cart is deleted:

```sql
DELETE FROM carts WHERE cart_id = @cartId
-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/Program.cs#L139 [tl! autolink]
```

In this case, it's pretty straightforward to assume that its items should delete at the same time, so we'll need to preserve the cascade functionality but with soft delete in mind. How about the following though:

```sql
DELETE FROM items WHERE item_id = @itemId;
-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/Program.cs#L138 [tl! autolink]
```

In our current production system, when we delete that item it will delete from the cart automatically. This is a good demonstration of why we care about soft delete - by preserving the data in our database we'll be able to tell the user that their dinner plate set is no longer available. Indeed, properly supporting that functionality is going to require some extra work in our code, but remember that _that_ is a feature add: we can do our soft delete refactor and deploy it, preserving the current functionality with minimal effort. Later iterations can build on this work to add the new capabilities in.

This is the sort of planning work which needs to be done before beginning the refactor: The relationship between tables needs to be mapped out and understood. Typically you'll already have cascades set, so ideally this won't be difficult work to do.

# Single Table

This is the first strategy I'll discuss. To implement this, we'll go through a few discrete steps. First, we'll add a `deleted` flag to all of our tables, then we'll create a view for each table that excludes deleted records, and finally we'll update the definition of `DELETE` to perform the soft delete. Those views will effectively replace our tables for all queries and commands. After the refactor, deleted items can be queried directly from the original tables, which for all other purposes will have been "hidden" by the new views.

## Adding the Deleted Flag

The first step in our refactor will be to update every table we want to soft delete to add a new column. This will be the flag that determines whether the record is deleted or not. You can use just a boolean here, however this is an opportunity for us to capture more data and potentially make debugging a bit easier. I prefer using a nullable timestamp for my flag - `null` signifies it is _not_ deleted, whereas the presence of a date indicates not only _that_ it was deleted, but also _when_. Because soft deletion is commonly used to support internal reporting scenarios, this can sometimes be quite useful information.

Adding the column is quite straightforward of course, it can be added with no effect on the overall system:

```sql
ALTER TABLE items ADD COLUMN deleted TIMESTAMP DEFAULT NULL;

ALTER TABLE carts ADD COLUMN deleted TIMESTAMP DEFAULT NULL;

ALTER TABLE cart_items ADD COLUMN deleted TIMESTAMP DEFAULT NULL;

-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/SingleTableMigration.cs#L6 [tl! autolink]
```

This can be deployed at any time and will not affect the rest of the system unless you're doing something squirrely with your tables. Just note though that if there's a long gap between your migration steps, new tables should have this column included. Because this column isn't used, it will be null for all records, and until we proceed to the next steps records will still be hard deleted.

## Hiding Deleted Data

This second step can also be deployed at any time after the first step with no change in the functionality of the system: we're going to hide any data where `deleted` is not null. In order to accomplish this without needing to rewrite any of our queries, constraints, or the like, we'll use views to achieve this.

For the unfamiliar, views act just like tables, but are essentially projections of other underlying tables and views. You define them with a `SELECT` statement just as you would query any other data. This allows you to marry commonly-referenced data together, but in our case we're going to use them a bit differently.

For _each_ table which has a `deleted` column, we're going to define a new view which excludes the deleted values of the underlying column. Now, because we have how many references to the tables named `items`, `carts`, and `cart_items`, and it's _these_ references from which we want to exclude the deleted items, we're going to give our views the names of the current tables and rename the current tables.

```sql
ALTER TABLE items RENAME TO items_all;
CREATE VIEW items AS SELECT * FROM items_all WHERE deleted IS NULL;

ALTER TABLE carts RENAME TO carts_all;
CREATE VIEW carts AS SELECT * FROM carts_all WHERE deleted IS NULL;

ALTER TABLE cart_items RENAME TO cart_items_all;
CREATE VIEW cart_items AS SELECT * FROM cart_items_all WHERE deleted IS NULL;

-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/SingleTableMigration.cs#L12 [tl! autolink]
```

Here I use the convention that the table name is suffixed "_all", signifying of course that the tables contain "all" of the records. Your requirements might be different, so pick a naming convention that makes sense in the context. I recommend [being extremely consistent](https://ian.wold.guru/Posts/its_better_to_be_consistently_incorrect_than_consistently_correct.html) here, and I also recommend picking a convention that is obvious in distinguishing the tables.

After deploying this update, you'll find that your system is still working exactly as it has in the past. Postgres passes any commands on these views down to the underlying table, so when you `INSERT` into one of these views, the data will be inserted into its respective table. Neat!

## Make Deletes Not Delete

This is the crucial step now: we need to redefine what a "delete" means. While the last two steps alter neither the behavior of the system nor the representation, this step will alter the representation of the data, and so long as we're dilligent it will not alter the behavior for your users. Because you're altering the data representation now though, be careful!

There's two steps we need to take here at once. First, we need to tell Postgres that when it gets a `DELETE` command for one of these tables that it actually needs to _update_ the `deleted` column instead. Second, we need to define the new cascade behaviors to propogate the soft deletion in the intended way.

To redefine `DELETE`, Potgres allows us to create a _rule_ to perform an alternate action _instead_. Typical for Postgres, this syntax is quite straightforward:

```sql
CREATE RULE rule_soft_delete AS ON DELETE TO items DO INSTEAD (UPDATE items_all SET deleted = CURRENT_TIMESTAMP WHERE item_id = OLD.item_id);
CREATE RULE rule_soft_delete AS ON DELETE TO carts DO INSTEAD (UPDATE carts_all SET deleted = CURRENT_TIMESTAMP WHERE cart_id = OLD.cart_id);
CREATE RULE rule_soft_delete AS ON DELETE TO cart_items DO INSTEAD (UPDATE cart_items_all SET deleted = CURRENT_TIMESTAMP WHERE cart_item_id = OLD.cart_item_id);

-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/SingleTableMigration.cs#L23 [tl! autolink]
```

And then we need to handle the cascades. Let's start with the simple case where we know that deleting a cart should cascade to each of its items. The rule we define will act on updates to the `carts_all` table and propogate new, non-null values for `deleted` to the cart items:

```sql
CREATE RULE rule_cascade_deleted_cart_items AS ON UPDATE TO carts_all
    WHERE OLD.deleted IS DISTINCT FROM NEW.deleted
    DO ALSO UPDATE cart_items_all SET deleted = NEW.deleted WHERE cart_id = OLD.cart_id;

-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/SingleTableMigration.cs#L27 [tl! autolink]
```

Recall that the other cascade case, where we delete an item, is more complicated. The current behavior (which we are just trying to preserve for now) is that items are deleted from a cart when the items themselves are deleted. The state we want to achieve (some time after this migration) is that items will still show in the cart but as "no longer available". That will take some extra coding to implement, however depending on the current state of our system we might be able to achieve some cost savings.

If you just want to keep the refactor simple or your system wouldn't currently support an alternate scheme, then you can implement the exact same rule but for items, then remove it later once you're ready. However, consider whether your current system would be able to do without the cascade right off the bat here. Suppose you have some logic on your cart page which attempts to get item data for all items in the cart and will not display items for which it cannot find data. In this case, you do not need to add the cascade, and the users will still observe the same behavior they currently have!

If you do decide not to carry over the cascade, there is the matter of the foreign key reference - if we soft delete a record from the `items_all` table, the foreign key on `cart_items.item_id` is still referencing the new `items` view and will break! In this case, we'll need to update the foreign key now:

```SQL
ALTER TABLE cart_items_all DROP CONSTRAINT fk_item;
ALTER TABLE cart_items_all ADD CONSTRAINT fk_items_all FOREIGN KEY (item_id) REFERENCES items_all (item_id) ON DELETE CASCADE;
```

Again, this step does _not_ need to be done now if you are adding the manual cascade for `items`.

## Accessing Deleted Records

At this point, your migration is finished. Congratulations! In order to get records that have been deleted, you just need to query the `_all` table for that record:

```sql
SELECT * FROM items_all WHERE ...
-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/SingleTableMigration.cs#L37 [tl! autolink]
```

In order to hard delete any records, you can `DELETE` from the `_all` table:

```sql
DELETE FROM items_all WHERE ...
```

# Separate Deleted Table

This is the second strategy, where deleted items are kept in separate tables. We'll also be able to work through some discrete steps to implement this. First, for each table we'll create a corresponding deleted table, and then we'll also create a corresponding view to unite the two. Finally, we'll alter the definition of `DELETE` to perform the soft delete. After the refactor, we'll be able to consult the separate deleted tables to work with our deleted records.

## Adding the Deleted Tables

The first step in our refactor will be to add a corresponding table for each existing table that will include the deleted fields. Instead of adding a `deleted` timestamp on the main table itself, it's sufficient for only this copy table to contain the field. We'll use `LIKE` to copy the schema of the base tables.

```sql
CREATE TABLE items_deleted (
    deleted TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    LIKE items INCLUDING ALL
);

CREATE TABLE carts_deleted (
    deleted TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    LIKE carts INCLUDING ALL
);

CREATE TABLE cart_items_deleted (
    deleted TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    LIKE cart_items INCLUDING ALL
);

-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/SeparateTableMigration.cs#L6 [tl! autolink]
```

My same note from earlier about naming conventions applies, though I think `_deleted` is probably the suffix 99% of us will ever use for these.

There is one issue with this snippet though - can you spot it? The problem is in `cart_items_deleted`: `LIKE ... INCLUDING ALL` will copy over the _entire_ table schema, foreign key references included. In order to get around this, we'll need to not use `INCLUDING ALL` and manually copy over the foreign key constraints:

```sql
CREATE TABLE cart_items_deleted (
    deleted TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    LIKE cart_items,
    
    CONSTRAINT fk_cart FOREIGN KEY (cart_id) REFERENCES carts_deleted (cart_id) ON DELETE CASCADE,
    CONSTRAINT fk_item FOREIGN KEY (item_id) REFERENCES items_deleted (item_id) ON DELETE CASCADE
);
```

But wait, there's more! This works all well and good so long as the carts and items you're referencing from the `cart_items_deleted` table actually exist in `items_deleted` and `carts_deleted`. This is sure to not be the case, as it makes perfect sense that I might delete a cart item without that item having been deleted! Supposing that we _did_ have a schema where we would expect the referant to be deleted also though, are we sure that _that_ record is being deleted first?

This can all get quite tricky. If you don't care about the referential integrity of your soft-delted data (but trust me, you probably _should_), then you can just forego the foreign key constraints. If you do care about the references though, then you'll need to spend some time thinking about the best approach here. A whole article can be written on potential approaches so I won't distract us here, but beware of this trap!

This demonstrates the extra complexity of this second approach. It allows for a greater separation between deleted and non-deleted items, though it's important to consider which functionality and level of complexity your system requires.

## Adding the Combined Views

There are inevitably operations we're going to want to perform on the whole dataset for each table - deleted _and_ non-deleted items all as one. To facilitate this, we'll create a view for each table/deleted table pair which unions the two:

```sql
CREATE VIEW items_combined AS SELECT null AS deleted, * FROM items UNION ALL SELECT * FROM items_deleted;
CREATE VIEW carts_combined AS SELECT null AS deleted, * FROM carts UNION ALL SELECT * FROM carts_deleted;
CREATE VIEW cart_items_combined AS SELECT null AS deleted, * FROM cart_items UNION ALL SELECT * FROM cart_items_deleted;

-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/SeparateTableMigration.cs#L23 [tl! autolink]
```

At least that's easy!

## Make Delete Not Delete

In order to make `DELETE`s perform a soft delete in this setup, we need to do a couple things when deleting: first, we need to copy the record into the respective `_deleted` table, then we need to allow the delete to actually happen.

Here we have a couple options. We could use a `RULE`...`DO ALSO` to perform the insert after the delete:

```sql
CREATE RULE soft_delete AS ON DELETE TO items DO ALSO (
    INSERT INTO items_deleted (item_id, name, price, created)
    VALUES (OLD.item_id, OLD.name, OLD.price, OLD.created)
);
CREATE RULE soft_delete AS ON DELETE TO carts DO ALSO (
    INSERT INTO carts_deleted (cart_id, user_id, created)
    VALUES (OLD.cart_id, OLD.user_id, OLD.created)
);
CREATE RULE soft_delete AS ON DELETE TO cart_items DO ALSO (
    INSERT INTO cart_items_deleted (cart_item_id, cart_id, item_id, quantity, created)
    VALUES (OLD.cart_item_id, OLD.cart_id, OLD.item_id, OLD.quantity, OLD.created)
);
```

However, because we're combining operations I would tend towards preferring a trigger in this specific scenario:

```sql
CREATE OR REPLACE FUNCTION items_soft_delete() RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO items_deleted (item_id, name, price, created)
    VALUES (OLD.item_id, OLD.name, OLD.price, OLD.created);
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_items_delete BEFORE DELETE ON items FOR EACH ROW EXECUTE FUNCTION items_soft_delete();

CREATE OR REPLACE FUNCTION carts_soft_delete() RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO carts_deleted (cart_id, user_id, created)
    VALUES (OLD.cart_id, OLD.user_id, OLD.created);
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_carts_delete BEFORE DELETE ON carts FOR EACH ROW EXECUTE FUNCTION carts_soft_delete();

CREATE OR REPLACE FUNCTION cart_items_soft_delete() RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO cart_items_deleted (cart_item_id, cart_id, item_id, quantity, created)
    VALUES (OLD.cart_item_id, OLD.cart_id, OLD.item_id, OLD.quantity, OLD.created);
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_cart_items_delete BEFORE DELETE ON items FOR EACH ROW EXECUTE FUNCTION cart_items_soft_delete();

-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/SeparateTableMigration.cs#L29 [tl! autolink]
```

You then have the same considerations regarding cascades and foreign key references as you had in the last refactor option. I'll leave that as an exercise for you to extend the trigger functions to include that behavior.

## Accessing Deleted Records

This is either the benefit or the drawback of this setup compared to the first scheme. In order to access deleted records, you query the `_deleted` table:

```sql
SELECT * FROM items_deleted WHERE ...
-- https://github.com/IanWold/PostgresRefactorSoftDelete/blob/main/SeparateTableMigration.cs#L58 [tl! autolink]
```

And of course all records together from the separate view:

```sql
SELECT * FROM items_combined WHERE ...
```

If your application will need to regard these three sets differently in distinct queries, then this setup is advantageous: there's less chance of writing the queries incorrectly and causing bugs. However, if you do need to consult the blended data or perform mostly the same queries, the first option might be better.

# Conclusion

Implementing soft delete on an existing system can seem like an exceptionally daunting task if you consider it from the perspective of needing to rewrite whole swaths of existing queries to suppot it. Not to mention, the incidence rate of bugs from such a hands-on refactor will be quite high!

The less we can touch our system during a refactor, the better. As with any refactor, this one requires some careful planning up front to understand the relationships between tables as data is deleted. Once that's understood, the refactor can proceed really quite fast _just_ by changing the database schema, preserving the existing functionality and, crucially, the existing queries _and_ the underlying semantics of those queries.

This doesn't absolve you of the careful work in ensuring your refactor works correctly (doubly so as you're tinkering around in the persistence). However, it does put more guardrails in place. Being able to move quickly _and_ deliberately, touching as little as possible, leaves you in the best position for success in your refactor.
