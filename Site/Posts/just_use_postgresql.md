;;;
{
	"title": "Just Use PostgreSQL",
	"description": "With a vast and growing ecosystem of database systems, data models, patterns, and paradigms, choosing the right one can be a long and complicated process. I prefer a simpler approach: Just use PostgreSQL.",
	"date": "13 January 2024",
	"contents": false,
	"hero": "photo-1599921778557-082147629542",
    "topics": ["Databases", "Postgres", "Standards"],
    "related": [
		{ "title": "Write Your Own RDBMS Versioned Migration Boilerplate", "description": "Versioned migrations are an essential tool for systems using an RDBMS, and it's no work at all to start your applications the right way with this pattern.", "fileName": "write_your_own_rdbms_versioned_migration_boilerplate" },
        { "title": "Quick & Dirty Sequential IDs in MongoDB", "description": "Mongo doesn't natively support generating sequential IDs. Here's a quick & dirty solution to get you up and going if you need sequential IDs.", "fileName": "quick_and_dirty_sequential_ids_in_mongo" },
        { "title": "Adding a Database to our Railway App", "description": "Last time I looked at Railway, I got it up and running with a Blazor WASM app. Now, I'll look at adding a PostgreSQL database to it.", "fileName": "adding_a_database_to_our_railway_app" }
    ]
}
;;;

There are a lot - and I mean _a lot_ - of options when you're considering a database to use. Every application has different data, making new, specialized databases interesting to us as potential options; it's often desirable to work with tools that support our specific use cases. More often than not though (heck, the vast majority of the time) storing data is just that - _storing data_. Specialization is good but if it's not really needed it's easy to become too constrained or too complex. Not to mention that as requirements change our systems often evolve beyond the constraints we start with, making it a bit tougher to choose a very specialized database.

I think that for the vast majority of systems, PostgreSQL is the best choice. It is open-source, performant, secure, and supports any data model or pattern you need. It's well-documented for all of its use cases, the tooling ecosystem around it is excellently mature; heck, darn near everything about it is excellently mature. I think it's the easiest database to get started with for just about any use case, and it's able to extend with your requirements more than any other database.

This is entering opinion territory, but I think that unless you have truly one-of-a-kind requirements, chosing PostgreSQL is the best way to set your new project up for success.

# PostgreSQL Supports Your Data Model

As requirements change, your data model can change for part or all of your application. I don't like being constrained by my database to a single data model or a set of models, and I don't like being shut out from implementing any patterns I need. Sometimes different data needs to be represented differently, or I might need to use different patterns for different domain contexts in my data, while still needing to maintain references between them. Only a system as widely capable and documented as PostgreSQL can allow me to do that.

Obviously PostgreSQL is a relational database, but it can support any data model you need. Built-in support for JSON data gives you everything you need for a document store. You can code your own solutions or use widely popular extensions for graph or column-family models. You can get quite far with native PostgreSQL, and well-maintained, well-documented, and well-used extensions can get you the rest of the way if you have more complicated requirements.

Any patterns you can think of are supported as well. Event sourcing, which can particularly complicated, has a lot of documentation. A lot of patterns like versioned schemas or CQRS aren't related to the specific database you choose, but PostgreSQL and its excellent community documentation makes using these patterns effortless.

# Your PostgreSQL Database can Perform and Scale

Especially with reent versions, PostgreSQL offers excellent support for performance tuning, and tools like Citus can make scaling to any size quite easy. That's relatively speaking of course; optimizing any database is necessarily complicated, and distribution is never an easy problem. But PostgreSQL has the tooling, documentation, and maturity to ensure you can get there with your use case.

Being largely open-source, there's a good chance you can set your PostgreSQL environment up without spending anything on the database system itself - ideally all of your costs can be on your infrastructure providers. Being as popular and long-lived as it is, PostgreSQL can run on just about any infrastructure you might be interested in provisioning for it.

# Don't Use a Document Database

Document databases are excellent for non-relational data. And being fair, there's a huge amount of non-relational data out there, particularly across the maybe millions of microservices we've created. If you ever end up needing to persist relational data though - and this isn't an unlikely change in your future requirements - you'll be in a bit of a pickle. Yes, most document databases can support relational data to a greater or lesser extent, but if you've got relational data you want a _relational database_.

I think for the vast majority of use cases, PostgreSQL is as capable a document database as any other popular choice. There's even libraries for most languages that allow you to use PostgreSQL with an interface more akin to popular document database drivers. But then when you need to start storing relational data, you can start using PostgreSQL in that capacity without needing to migrate your data to a different system, and without using a database that doesn't naturally support it.

# When to Not Use PostgreSQL

The other databases exist for reasons though, and I don't want to be taken as advocating an exclusively PostgreSQL approach. There are plenty of edge scenarios where you'll want a different database, but I caution that these scenarios are _very much the exception_.

If you need to serve extremely high volumes of high-performant real-time analytics, BigQuery probably makes sense over PostgreSQL. If your application absolutely requires the highest write throughput that humanity's capabilities can muster, then a document database or KV database optimized for writes will outperform PostgreSQL. Similarly, if you're handling truly planet-scale amounts of data (many petabytes) then you should probably be reading other blogs altogether, actually.

The case you're more likely to encounter (note I did not say you're likely to encounter, just that it's more likely) is when your application - say, a microservice - is uniquely, wholy (not partly), and inextricably reliant upon the specific data structures, patterns, or capabilities offered by an alternate, specialized database. I caution you to re-read the qualifiers I put on that statement: uniquely, wholy, and inextricably. If your data is mixed model or paradigm, it probably makes sense to have it all in a PostgreSQL database. If you need referential integrity between the data in the different models, then it definitely makes sense to keep it in PostgreSQL. But Elasticsearch is spectacular if you've got a microservice exclusively for complex searches, and EventStoreDb is great if you've got a state machine microservice (do those exist?).

# But Really, Just Use PostgreSQL

Those edge cases truly are at the edge of what we need to support. The overwhelming set of persitence needs for our systems is well-handled by PostgreSQL. The majority of scenarios that are supported are extremely well supported, and PostgreSQL will be very competent for everything else.
