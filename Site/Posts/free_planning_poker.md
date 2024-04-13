;;;
{
	"title": "Thing I Made: FreePlanningPoker.io",
	"description": "I made a free planning poker tool and named it aptly.",
	"date": "13 April 2024",
	"contents": false,
	"hero": "photo-1596451190630-186aff535bf2",
    "related": [
		{ "title": "Reclaim Your Agile: The One Clever Trick Agile Coaches Don't Want You to Know", "description": "What if I told you there's one trick to being able to reshape your team's development process without your company knowing it? What if I told you that you can achieve actual Agile even though you work in a Scrum firm?", "fileName": "reclaim_your_agile" },
        { "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "fileName": "deploying_aspdotnet_7_projects_with_railway" },
        { "title": "A Scrum Odyssey", "description": "A journey away from daily scrum meetings, as a cycle of eight Shakespearean sonnets.", "fileName": "a_scrum_odyssey" }
    ]
}
;;;

For the last week and a bit I've been working on [FreePlanningPoker.io](https://freeplanningpoker.io) and I think it's ready enough to show off. It's a Blazor app that uses SignalR and Redis to maintain state across several connected clients.

I'm not an advocate for using points, and [I'm certainly no fan of scrum](https://ian.wold.guru/Posts/book_club_12-2023.html) but the fact of the matter is that a lot of us get stuck in plenty of card pointing meetings. There's various tools to facilitate this online, and none of them had all the features I wanted, or they weren't _just right_. Some of them incentivise practices that I think are detrimental. As many of us are prone to, I asked myself "How hard could it be to make one?"

Very easy. Actually, extremely easy. Sure there's probalby a minor race condition or two but let's not let perfect be the enemy of good enough here. _(obligatory "/s")_

# The Code

If you've worked with SignalR before, this is very straightforward. If you haven't, you're going to be up-to-speed in a few sentences. SignalR is Microsoft's websockets implementation for .NET. It works reasonably well, especially with ASP and Blazor. You're able to define interfaces for the server methods and the notifications the client will receive for a hub, then you can use source generators on the client to hook it up to the server. Though it's not _quiiiite_ as performant as I might appreciate at scale, it does the trick handily with a small amount of code.

SignalR can use Redis as a backplane, and that's advantageous because this app is a sort of state machine without much data, so Redis is a good way to keep state on the server. Alas, there doesn't seem to be a good PostgreSQL backplane for SignalR otherwise [I probably would have used that](https://ian.wold.guru/Posts/just_use_postgresql.html), but Redis is fast and lightweight for this sort of application.

The code is open source, of course, and I've got [much finer-grained descriptions](https://github.com/IanWold/PlanningPoker) of it in the readme.

# Goals and Principles

My intial goal was met in about a week: I wanted a cheap app that solved my problem the way I wanted it solved. I'm very happy with that result! I want to keep this around though, I think it can be a benefit in a few ways. First, I think it's a good example of using the strongly-typed client source generators for SignalR in Blazor, which are to my knowledge not documented apart from [one post from Kristoffer Strube](https://kristoffer-strube.dk/post/typed-signalr-clients-making-type-safe-real-time-communication-in-dotnet/). In fact I think it's generally a good sample Blazor application.

I think an application like this should be very minimal - not just in the UI and user experience, but it should take almost nothing to run the application. Sure, I chose Blazor which isn't exactly the most lightweight SPA framework, but overall the footprint is quite light. The server costs very little to run, being only an ASP server with a single SignalR hub and a Redis instance. It's got a low memory footprint and doesn't consume a lot of computing resources. Apart from resulting in a fast and maintainable application, this is a greener way to approach software.

There should also be more software which encourages users to make it their own. I don't mean from a UI styling perspective, but from the code itself. If you want to take my code and deploy it yourself for you or your team to use on your own infrastructure, that should be easy, well-documented, and the code should provide all the environment variables you might need to tweak your setup. This application only really needs a Redis connection string but you get the idea. If you want to fork the code and make modifications on top of that, you should be well-supported by the code and its author in doing so.

I chose the [Unlicense](https://en.wikipedia.org/wiki/Unlicense) for this project for all those reasons too - more free code is more good.

Finally, I think users' data should be respected. How much sensitive information is being transacted over a planning poker app? I hope none. Nonetheless, it's your information and that should be respected. This application deletes all the data from each session as soon as the session is left by the final participant. I don't log or retain any information from the sessions, and [I'm going to be adding client-side encryption](https://github.com/IanWold/PlanningPoker/issues/8) to completely remove that as an issue.

# Soliciting Feedback

If you use this, please do let me know if there's anything to improve, or if there's something you like about it. You can comment or [webmention](https://ian.wold.guru/Posts/ive_indiewebbed_my_site.html) on this post, you can open an issue [on the repository](https://github.com/IanWold/PlanningPoker/issues) or you can [connect with me](https://ian.wold.guru/connect.html) through several other avenues.

If you'd like to add something to the application, I'm happy to entertain PRs! I'd like to add some fun features to help distract the participants that they're in a planning meeting; that might do some more good for the world.