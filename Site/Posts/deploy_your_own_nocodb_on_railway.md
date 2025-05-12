;;;
{
	"title": "Deploy Your Own NocoDB on Railway",
	"description": "NocoDB is an interesting new OSS project that's easy to deploy for free on Railway!",
	"date": "12 May 2025",
	"contents": false,
	"hero": "photo-1456428746267-a1756408f782",
    "topics": ["Deployment", "How-To"],
    "related": [
		{ "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "fileName": "deploying_aspdotnet_7_projects_with_railway" },
		{ "title": "Postgres: Use Views to Refactor to Soft Delete", "description": "Refactors are tough, database refactors are scary. Being a bit clever can save us a lot of pain!", "fileName": "postgres_use_views_to_refactor_to_soft_delete" },
		{ "title": "90% of my Homepage was Useless", "description": "In a few days, I reduced the size of my homepage to 10% of what it had been, and sped it up by 50-66%.", "fileName": "90_of_my_homepage_was_useless" }
    ]
}
;;;

[NocoDB](https://github.com/nocodb/nocodb) is an interesting open source project I came across some weeks ago; it aims to be an OSS alternative to [Airtable](https://www.airtable.com/). Airtable is a "nocode" solution for line-of-business applications, focusing on business process automation. It's like an enterprisey [IFTTT](https://ifttt.com/) (although; IFTTT would probably want you to think of them as the enterprisey IFTTT). NocoDB is in its early stages but seems to want to occupy the same niche, but there's a couple differences that make it useful to me for other purposes.

Crucially, it's built as a UI wrapper around a database of your choice. Their documentation is a bit sparse right now, but I think I'm right in saying they support Postgres, MySQL, and SQLite at the time of this writing. If you've read my work before, you know that [I like Postgres](https://ian.wold.guru/Posts/just_use_postgresql.html). There's a lot of UIs you can use with Postgres, even some that have fancy visualizations like this NocoDB, but the difference with NocoDB that's impressed me is in their support for automation: by developing their software into the no/low-code niche, it becomes more useful for those cases where I _just_ need to set up some trigger here or interaction there.

I've been wanting to replace all my spreadsheets with Postgres for a while now, and NocoDB might just be the UI that lets me do that. Being OSS, I can deploy it myself with my own Postgres database for free! Here I'm going to outline how that's done in [Railway](https://railway.app), my preferred cloud host. The relevant docs from NocoDB are their [Docker install](https://docs.nocodb.com/getting-started/self-hosted/installation/docker-install/) and [environment variables](https://docs.nocodb.com/getting-started/self-hosted/environment-variables).

The setup will be quite simple: we'll just need a Postgres database and a Docker container (with a volume) for NocoDB. We'll start by creating an empty project:

![nocodb-railway-new-project](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/nocodb-railway-new-project.png)

To which we can add a Postgres database:

![nocodb-railway-postgres](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/nocodb-railway-postgres.png)

I was fine with the default Postgres settings, but feel free to change yours how you need.

Creating the NocoDB container is easy enough; the image `nocodb/nocodb:latest` is available from docker hub. We can create the container by selecting `Docker Image` when we go to create the container:

![nocodb-railway-new-docker-image](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/nocodb-railway-new-docker-image.png)

and entering the image name

![nocodb-railway-new-nocodb](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/nocodb-railway-new-nocodb.png)

You can deploy this now if you like, but we're going to want to attach a volume and connect it to Postgres to work. To attach the volume, right-click on the NocoDB container:

![nocodb-railway-attach-volume](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/nocodb-railway-attach-volume.png)

And mount the volume to `/usr/app/data/`

![nocodb-railway-volume-mount](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/nocodb-railway-volume-mount.png)

In order to connect to the postgres database, we can set the `NC_DB` variable on the NocoDB container. You can click on the container and then the Variables tab:

![nocodb-railway-variables](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/nocodb-railway-variables.png)

Railway has support for referencing other services in its environment variables, which we can take advantage of in constructing our connection string. When I added my variable, I used the following to get a connection string in the format that matches NocoDB's docs:

```plaintext
pg://${{Postgres.RAILWAY_PRIVATE_DOMAIN}}:${{Postgres.PGPORT}}?u=${{Postgres.PGUSER}}&p=${{Postgres.PGPASSWORD}}&d=${{Postgres.PGDATABASE}}
```

![nocodb-railway-attach-variable](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/nocodb-railway-attach-variable.png)

That's all we need now in order to deploy and check that we're successful. Once that's verified, you can [configure a public URL](https://docs.railway.com/guides/public-networking) for your NocoDB container and start using it!