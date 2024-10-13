;;;
{
	"title": "Write Your Own RDBMS Versioned Migration Boilerplate",
	"description": "Versioned migrations are an essential tool for systems using an RDBMS, and it's no work at all to start your applications the right way with this pattern.",
	"date": "25 November 2023",
	"contents": false,
	"hero": "photo-1596563910641-86f6aebaab9a",
    "topics": ["Databases", "How-To"],
    "related": [
		{ "title": "Adding a Database to our Railway App", "description": "Last time I looked at Railway, I got it up and running with a Blazor WASM app. Now, I'll look at adding a PostgreSQL database to it.", "fileName": "adding_a_database_to_our_railway_app" },
		{ "title": "Quick & Dirty Sequential IDs in MongoDB", "description": "Mongo doesn't natively support generating sequential IDs. Here's a quick & dirty solution to get you up and going if you need sequential IDs.", "fileName": "quick_and_dirty_sequential_ids_in_mongo" }
    ]
}
;;;

If you're using a relational database, even perhaps for a small personal project, you've almost certainly had to have a serious think about migrations. Some databases require very heavily-engineered migration systems to be able to handle large, complex, and/or frequently-changing data. Other databases are very small and might do just fine with a single migrations file, or maybe even manual migrations computed off a shared schema script, if updates are few and far between.

There's many different migration strategies, and there's no one-size-fits-all approach - different databases and applications can require vastly different strategies. One of the best and most ubiquitous strategies is the versioned migration, where individual updates are stored in SQL scripts that have an incremental version. I like a setup where, on startup, my application consults a migration history table in my database to see what the latest migration is, and to run any new migration scripts in sequential order.

In my experience this setup works very well, and it can scale to a large size of project, database, or team. I start almost all of my projects - big or small, personal or professional - with this strategy, and I think you should consider making this strategy (or some variation of it) your default as well.

One holdup though - isn't that a lot of overhead for a small, personal project? Should I really be investing the time to set up [Flyway](https://flywaydb.org/) for every little API I want to set up? I contend no and no. I keep a snippet of boilerplate code to handle these migrations that I copy for every new project. It's a small amount of code, and I can modify it as needed per-project if it requires anything special. Best of all, my entire database from the start is migration-versioned, making it easy in future to switch to another system or onto Flyway if needed.

# Versioned Migrations

As I described, this strategy involves breaking your migrations down into individual scripts for each discrete migration, and assigning them a version. What belongs in an individual file is up to you. You can restrict each file to only containing a single update on a table or column, or you could say that each feature card should have a single migration script. I prefer an in-between where each script contains a logically coupled, discrete set of changes, such that the database is valid at any migration version.

However you split these up, you might have several migration scripts:

```plaintext
/ Migrations
  |- 01_CreateUsersTable.sql
  |- 02_CreateItemsTable.sql
  |- 03_CreateListsTable.sql
  |- 04_AddListIdToItemsTable.sql
  |- 05_AddUserIdToListsTable.sql
```

You will need some way to assign a version to each script. I prefer adding the version to each file name (as Flyway does, if I recall correctly) so that it's easy to see, and is aligned by version in my file system. If you prefer otherwise, you could maintain a separate map in your code or a config file from a file name to a version number, or no doubt any number of other strategies.

The other aspect of this migrations strategy is that we will need to maintain a table containing the migration history of the database. I prefer a simple table myself:

```sql
CREATE TABLE migration_history(
    "version" bigint primary key,
    "migrated" timestamp default NOW()
)
```

In this way, I can keep track of where each database is at and apply migrations accordingly. When my app starts up, I'll look at this table to see what the latest version is. If the app sees that there are migration files exceeding this version, then I can run those migration scripts in the order they're intended.

# Run the Migrations

Our migration-running code needs to do the following:

1. Check the latest migration version, or create the `migration_history` table if it's a new database,
2. Find all the migration scripts after the latest version
3. Execute these scripts in order, updating the `migration_history` as it goes
4. Commit the changes (important)

I'll be demonstrating this with C# and PostgreSQL (via Npgsql), but this approach will work in any language with any RDBMS. The code should be straightforward enough for you to translate to whatever your case is.

```csharp
public static class DatabaseMigrator
{
    static int GetLatestVersion(NpgsqlConnection connection, NpgsqlTransaction transaction) // TODO
    static IEnumerable<(int, string)> GetNewMigrationFiles(int latestVersion) // TODO
    static void RunMigrationFile((int version, string name) file, NpgsqlConnection connection, NpgsqlTransaction transaction) // TODO

    public static void Migrate(string connectionString)
    {
        using var connection = new NpgsqlConnection(connectionString);
        using var transaction = connection.BeginTransaction();

        var latestVersion = GetLatestVersion(connection, transaction);
        var newMigrationFiles = GetNewMigrationFiles(latestVersion);

        foreach (var file in newMigrationFiles)
        {
            RunMigrationFile(file, connection, transaction);
        }

        transaction.Commit();
        connection.Close();
    }
}
```

Then call `DatabaseMigrator.Migrate(connectionString);` from your startup logic, and it's all wired up! We can focus then on implementing each of the TODOs here.

`GetLatestVersion` is probably the most complicated of these, because we'll want to check whether `migration_history` exists before we try to consult it, and create it if not. Before we get started implementing that method though, we'll want to write a little boilerplate to excute some queries on the database.

```csharp
static NpgsqlCommand GetCommand(string query, NpgsqlConnection cnonnection, NpgsqlTransaction? transaction)
{
    var command = connection.CreateCommand();

    command.Connection = connection;
    command.CommandText = query;

    if (transaction is not null)
    {
        command.Transaction = transaction;
    }

    return command;
}

static void Command(NpgsqlConnection connection, NpgsqlTransaction transaction, string query)
{
    var command = GetCommand(query, connection, transaction);
    command.ExecuteNonQuery();
}

static T Query<T>(NpgsqlConnection connection, string query)
{
    var command = GetCommand(query, connection);
    return (T)command.ExecuteScalar();
}
```

If you wanted to implement all the logic here to check that the result actually is a `T` and handle that in a special flow, you can do. However, this is good enough for me - this code is running in a way where it's unlikely for me to encounter an exceptional scenario but in an _exceptional_ scenario, and if this code fails I want to let it throw anyway so that my app crashes and its health endpoint responds with a failure. Your scenario may well differ though.

With that, I can outline `GetLatestVersion`:

```csharp
static int GetLatestVersion(NpgsqlConnection connection, NpgsqlTransaction transaction)
{
    var migrationHistoryExists = Query<bool>(
        connection,
        "SELECT EXISTS(SELECT 1 FROM pg_tables WHERE tablename = 'migration_history')"
    );

    if (migrationHistoryExists)
    {
        return Query<int>(
            connection,
            "SELECT MAX(version) FROM migration_history"
        );
    }
    else
    {
        Command(connection, transaction,
            """
            DROP SCHEMA public CASCADE;
            CREATE SCHEMA public;
            GRANT ALL ON SCHEMA public TO postgres;
            GRANT ALL ON SCHEMA public TO public;
            COMMENT ON SCHEMA public IS 'standard public schema';

            CREATE TABLE migration_history(
                "version" bigint primary key,
                "migrated" timestamp default NOW()
            )
            """
        );

        return -1;
    }
}
```

Given that, it's quite easy to implement `GetMigrationFiles`. The only peculiarity of that method is that it will return a tuple containing the version and file name for each file, so that it's easy for the other code to reference. Here I'm assuming all the migrations are in the "/Migrations" directory.

```csharp
static IEnumerable<(int, string)> GetNewMigrationFiles(int latestVersion) =>
    new DirectoryInfo("/Migrations").GetFiles()
    .Where(f => f.Extension == ".sql")
    .Select(f => (version: Convert.ToInt32(f.Name.Split('_')[0]), file: f.FullName))
    .Where(f => f.version > latestVersion)
    .OrderBy(f => f.version);
```

This can probably be more concise using query syntax and defining the version with `let` but I'll leave that as an exercise for the reader.

The only thing left then is to run these scripts and update the migration history:

```csharp
static void RunMigrationFile((int version, string name) file, NpgsqlConnection connection, NpgsqlTransaction transaction)
{
    var query = "";
    using (var reader = new StreamReader(file.name))
    {
        query = reader.ReadToEnd();
    }

    conn.Command(connection, transaction, query);
    conn.Command(connection, transaction, $"INSERT INTO migration_history (version) VALUES ({file.version})");
}
```

That's it! With just a 100-line C# file we've got fully-versioned migrations!

# Conclusion

You can find all the code together on [this GitHub Gist](https://gist.github.com/IanWold/d466f0e7e983da7b09e5ecc6bf719341). I copy this for every project I start, and I start each database out with versioned migrations.

You don't need to start your project off with a dependency on a third party migration library, you don't need to jump through any hoops - technical or conceptual - in order to get versioned migrations, and starting out with this puts you on the most solid path from the start. In future as your project evolves, if you end up in the rare situation of needing more features in your migrations, the code is right here for you to add it! If you end up needing so many migration features that a library like Flyway makes more sense, your story for switching to Flyway will be very easy. indeed.

Happy migrating!
