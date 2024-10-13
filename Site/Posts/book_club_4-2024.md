;;;
{
	"title": "Book Club 4/2024: I Don't Like ORMs",
	"description": "Object-relational mappers are more trouble than they're worth.",
	"date": "24 April 2024",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Architecture", "Patterns", "Databases"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 3/2024: Simplicity", "description": "Everything is too complicated.", "fileName": "book_club_3-2024" },
		{ "title": "Book Club 2/2024: Recovering from TDD and Unit Tests", "description": "TDD and unit tests are overused and often misprescribed. What do we really hope to gain from our tests, and what testing practices support our goals?", "fileName": "book_club_2-2024" },
		{ "title": "Book Club 1/2024: What is a Software Architect?", "description": "A few meandering and maybe unhelpful thoughts on the title \"Software Architect\"", "fileName": "book_club_1-2024" }
	]
}
;;;

Indeed, [Object-Relational Mappers](https://en.wikipedia.org/wiki/Object%E2%80%93relational_mapping) (ORMs) are a negative thing to me. For the uninitiated, these are tools which _automagically_ provide ways of mapping data between the object (class) representations in our application software and the corresponding relational representations in databases (specifically, of course, relational databases). They present an alternative to writing SQL and deserializing the data manually. Popular ORMs include [Entity Framework for .NET](https://learn.microsoft.com/en-us/aspnet/entity-framework) or [Prisma for Node/TS](https://www.prisma.io/orm).

ORMs typically have two main components: some way to map between properties in a class and table/columns in the database, and some way to not have to write SQL in your application. In the case of Entity Framework and similar libraries for .NET, this takes the form of attributes on class properties for the former, and using LINQ to write the queries which then "intelligently" get translated to SQL at runtime and sent down the wire. It provides type safety and absolves the engineer from having to write SQL in their applications. Many take this to be a good thing.

The problem which I have with ORMs is twofold, but it mainly boils down to the second. First, I perceive ORMs as polluting my objects - attributes and configuration code are annoying, sure, but I have concerns with vendor lock-in and more greatly that the query abstractions tend to hide what they're doing. Taking Entity Framework specifically, you need to be quite good at knowing what it's doing in the LINQ there to avoid any number of gotchas, which increases the incidence rate of footguns. This leads into my second and important gripe: ORMs offer less performance. Even if you're really good at them you're never going to get an overall performance benefit.

The performance problem does frequently come from a misunderstanding of how the SQL abstraction of a particular ORM maps to SQL, yes, but it's also _just_ less performant to insert an abstraction over something. A lot will push back on me at this point that performance isn't everything - readability and maintainability are important, and my straw man here will feel that the type-safe abstraction of the ORM is more of both. I think this is misguided. Unless you know SQL quite well _and_ you know how your ORM generates SQL from its abstraction you're going to be setting up some potential very volatile code, so you need to read the code you wrote in this abstraction through this lens. Which seems easier then - writing and reading this abstracted code while considering the SQL translation, or just writing and reading SQL?

One can claim "skill issue" here all they want, but the point is that I cannot see how using the ORM reduces any burden on me as I'm writing code. It only adds to the considerations I have to make. To be clear, I do believe you need to learn and know SQL well in order to be able to effectively use a relational database ever, whether you're using an ORM or not. For an engineer to desire to avoid learning and understanding SQL is lazy. Perhaps someone has indeed invested the time in learning it well and decided that they really would rather have the abstraction over it; this is an informed opinion which I can respect, and I'm sure such an engineer would be grateful for what they did learn from SQL (my favorite straw men are the respectable sort). I don't know that you can ultimately use any tool to make up for not understanding the fundamental working of databases and their query engines - just as in sports, it's not the tools that make one successful.

So all that considered, my recommendation is to just use SQL. That, to me, has the lowest burden across the board. Since I need to learn SQL I know it, when I write or read SQL I don't have to jump through translations, and when it comes to mapping or deserializing the response from the database I have all the control I need. But wait! Doesn't that lead to so much more boilerplate code? No, and I'm disinclined to elaborate for I've never seen a proof of this and I believe the burden of proof is on the individual making this assertion. Please leave your proof in the comments! To anticipate a response, the boilerplate which I use over ADO.NET and copy/paste/modify for any particular project clocks in at under 100 lines, I and everyone else in the codebase know exactly what it does, and it outperforms your ORM.

As a final note, there are a category of ORMs which assist you in writing actual SQL then handling just the mapping; they don't abstract over the SQL, which is my main objection. In .NET the most popular one by far is [Dapper](https://github.com/DapperLib/Dapper). Considering the low cost of entry to rolling my own mapping, I prefer and recommend that approach. However, there are projects or environments where Dapper can be useful, and I would recommend that everyone look to this library for these cases, or as an inspiration for what you can achieve on your own.

**Watching**

* [I Would Never Use an ORM - Matteo Collina (2022)](https://www.youtube.com/watch?v=atABji4xqiI)
* [The Only Database Abstraction You Need - CompSciGuy (2023)](https://www.youtube.com/watch?v=tbfKZy7Y1pc&t=0s)
* [I tried 8 different Postgres ORMs - Beyond Fireship (2023)](https://www.youtube.com/watch?v=4QN1BzxF8wM)
* [Why your DBA hates your ORM Hibernate or Entity application - Shad Sluiter (2020)](https://www.youtube.com/watch?v=evLx2TKTFI8)

**Reading**

* [What ORMs have taught me: just learn SQL - Geoff Wozniak](https://wozniak.ca/blog/2014/08/03/1/index.html?amp%3Butm_medium=referral)
* [Node.js ORMs: Why you shouldn’t use them - Thomas Hunter II](https://blog.logrocket.com/node-js-orms-why-shouldnt-use/)

I know that dev.to and Medium articles are a bit of a mixed bag, but these are well-considered with good, practical examples:

* [Why ORMs Aren't Always a Great Idea - Harsh Singh](https://dev.to/harshhhdev/why-orms-arent-always-a-great-idea-41kg)
* [Why ORMs Are Not Always the Way To Go - Joseph Pellegrini](https://betterprogramming.pub/why-orms-are-not-always-the-way-to-go-6aa578026a16)