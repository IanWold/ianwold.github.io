;;;
{
	"title": "Adding a Database to our Railway App",
	"description": "Last time I looked at Railway, I got it up and running with a Blazor WASM app. Now, I'll look at adding a PostgreSQL database to it.",
	"date": "8 November 2023",
	"contents": false,
	"hero": "photo-1541509411630-9c14b774ecba",
    "topics": ["Databases", "Deployment", "Dotnet", "How-To"],
    "related": [
        { "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "fileName": "deploying_aspnet_7_projects_with_railway" },
        { "title": "Book Club 10/2023: Functional Patterns in C#", "description": "This month I've focused on functional domain modeling and related patterns. We're just a few weeks away from the release of the next version of C#, and like each previous version it'll introduce even more functional features.", "filename": "book_club_10-2023" },
		{ "title": "An Introduction to Sprache", "description": "Sprache is a parser-combinator library for C# that uses Linq to construct parsers. In this post I describe the fundamentals of understanding grammars and parsing them with Sprache, with several real-world examples.", "fileName": "sprache" }
    ]
}
;;;

[Last time I looked at Railway](https://ian.wold.guru/Posts/deploying_aspdotnet_7_projects_with_railway.html), I got it up and running with a Blazor WASM app. Now, I'll look at adding a PostgreSQL database to it. As with the app I got working, Railway's interface will make it incredibly simple to provision the database, and we'll need to do minimal work to get the connection info to our app.

# Provisioning the Database

From the Railway dashboard, you can click into your project, and there should be a New button at the top (as in a buttno that says "New", not a button whose appearance would not heretofore be expected):

![New Project button in Railway](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/railway-database-new.png)

This brings up the familiar dialog to provision resources, from which we'll select Database:

![New Project button in Railway](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/railway-database-new-project.png)

And then I'm going to choose PostgreSQL here. Note that if Railway doesn't have an option here for the database you want, you can always create a plain Docker image from an image of your preferred database.

![New Project button in Railway](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/railway-database-new-database.png)

And look at that, it deploys a new PostgreSQL database! This is one of the things I love about Railway - their interface to set up resources has eliminated _all_ of the steps to setup that aren't 100% necessary, and the default settings they choose are logical and easily overwritten later if we need.

Notice also the attached `pgdata` volume. As discussed before, Railway just stores all of your resources in Docker images. This doesn't require that you create a `dockerfile` for each of your projects and resources, they can do that for you if you don't want to. It is to say that they give you all the tools you might want to use to be able to manage your resources as docker containers and volumes, so you can more or less choose the level of control you need over your resources.

The way I use Railway, I let it manage all my resources for me, and I don't bog myself down in the weeds of Docker as much. I've got plenty of time in my day job to get frustrated with Docker!

# Connecting to our Database

If you click on your new database resource and then the `Variables` tab, you should see a bunch of variables:

![New Project button in Railway](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/railway-database-variables.png)

This contains all the information that our app will need to connect to it. Railway allows the resources within a project to reference each others' variables, so we just need to know which ones to reference to build a connection string.

Now, they've all got incredibly obvious names so you can certainly accomplish this by guessing in as much time as it takes you to read this article, but for thoroughness' sake I've got a connection string here:

```plaintext
User Id=${{Postgres.PGUSER}};Password=${{Postgres.PGPASSWORD}};Host=${{Postgres.PGHOST}};Port=${{Postgres.PGPORT}};Database=${{Postgres.PGDATABASE}}
```

If you copy that as the value of a new variable in your application, it will fill in each of the references with the values from your database's variables, and that should be a sufficient connectino string to test that it works:

![New Project button in Railway](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/railway-database-connection-string.png)

Note that I'm using the variable name `ConnectionStrings__Database`, with two underscores, which behaves as though I'm inserting the following into my `appsettings` (in a .NET context):

```json
"ConnectionStrings": {
    "Database": "..."
}
```

And that's all the configuration needed for your app to be able to consume your new database!
