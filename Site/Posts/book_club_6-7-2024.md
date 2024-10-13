;;;
{
	"title": "Book Club 6&7/2024: Postgres",
	"description": "I have become further radicalized to the opinion that we should all just be using PostgreSQL",
	"date": "29 July 2024",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Postgres", "Databases"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 5/2024: SOLID", "description": "Is SOLID still relevant?", "fileName": "book_club_5-2024" },
		{ "title": "Book Club 4/2024: I Don't Like ORMs", "description": "Object-relational mappers are more trouble than they're worth.", "fileName": "book_club_4-2024" },
		{ "title": "Book Club 3/2024: Simplicity", "description": "Everything is too complicated.", "fileName": "book_club_3-2024" }
	]
}
;;;

I hope you all in the northern hemisphere are enjoying summer; I've been trying to take some extra time here and there to enjoy things before Minnesota inevitably plunges back into the depths of winter, so I've been writing a bit less (and combining two months' book clubs) but I think that's okay, all things considered!

A few months ago I wrote a short piece recommending that we [just use PostgreSQL](https://ian.wold.guru/Posts/just_use_postgresql.html), and I've spent several months becoming more entrenched in this position. This might be me jumping on the bandwagon - Postgres has become increasingly popular in recent years - but its genuine, proven capabilities leave me wondering what else could be the best de facto choice? You might argue that a default database option isn't needed, though I see a lot of value in it; at least as a standards bearer, like Git is anymore for version control, it provides a baseline for the whole industry.

I've become very serious about insisting that any new projects _must_ use Postgres unless there's a clear argument in favor of another database, and that's a difficult hill to climb out of. The vast majority of professional applications are not just adequately served by a Postgres database, rather Postgres is typically one of the better options. The kicker though is that Postgres usually allows you to do a lot more than just data storage, keeping the solution architecture very minimal for a whole host of apps.

I didn't touch on the ancillary capabilities that Postgres has in my original article, but from the articles below we can collate a heck of a lot. It can be your cache, message queue, job queue, or cron daemon. It can act as a document database, graph database, geospatial database, or search database. It supports any data model and easily handles complicated data patterns like event sourcing. Logging, metrics, and analytics can all live within Postgres; [Timescale](https://www.timescale.com/) allows you to make Postgres your data warehouse.

Most importantly, Postgres is _used_, and it's used _a lot_. All of these solutions are first-class and have been in use in major production systems for quite a while. There's great documentation all over for Postgres and all of these capabilities.

The other month I used Redis for a project, as SignalR doesn't have the ability to use PostgreSQL as a backplane, but I'm going to see about writing that. I've been exploring using Postgres for most of the uses above, and I'm feeling quite settled that Postgres is the standard bearer.

Next time you get the inkling to use something other than Postgres on a professional project, ask _why not Postgres?_ Insist on a proper, comprehensive answer that accounts for the flexibility, security, simplicity, documentation, and resilience that you get with Postgres. Yes, it's a tall hill to climb; yes, our professional work requires it.

* [Why PostgreSQL Is the Bedrock for the Future of Data - Ajay Kulkarni](https://www.timescale.com/blog/postgres-for-everything/)
* [Just Use Postgres for Everything - Stephan Schmidt](https://www.amazingcto.com/postgres-for-everything/)
* [Why Not Just Use Postgres? - Keith Gregory](https://medium.com/data-engineering-chariot/why-not-just-use-postgres-cc13a506a9b5)
* [Postgres: a Better Message Queue than Kafka? - Pete Hunt](https://dagster.io/blog/skip-kafka-use-postgres-message-queue)
* [How to use PostgreSQL like a message queue - Calvin Furano](https://worlds-slowest.dev/posts/postgresql-message-queue/)
* [pg_cron : Probably the best way to schedule jobs within PostgreSQL database - FatDBA](https://fatdba.com/2021/07/30/pg_cron-probably-the-best-way-to-schedule-jobs-within-postgresql-database/)
* [You Don't Need a Dedicated Cache Service - PostgreSQL as a Cache - Martin Heinz](https://martinheinz.dev/blog/105)
* [PostgreSQL Full Text Search: The Definitive Guide - Antonello Zanini](https://www.dbvis.com/thetable/postgresql-full-text-search-the-definitive-guide/)
* [Postgres: The Graph Database You Didn't Know You Had - Dylan Paulus](https://www.dylanpaulus.com/posts/postgres-is-a-graph-database)
* [Architecting petabyte-scale analytics by scaling out Postgres on Azure with the Citus extension - Claire Giordano](https://techcommunity.microsoft.com/t5/azure-database-for-postgresql/architecting-petabyte-scale-analytics-by-scaling-out-postgres-on/ba-p/969685)

And some videos:

* [The Only Database Abstraction You Need - The Primagen](https://www.youtube.com/watch?v=nWchov5Do-o)
* [Using PostgreSQL to Handle Calendar Data Like a Freak - Rob Conery](https://www.youtube.com/watch?v=pfL-9ntZqnI)
* [Just in time compilation in PostgreSQL - Andres Freund](https://www.youtube.com/watch?v=k5PQq9a4YqA)

And now for something completely different:

* [DHH discusses SQLite (and Stoicism) - Aaron Francis with DHH](https://www.youtube.com/watch?v=0rlATWBNvMw)