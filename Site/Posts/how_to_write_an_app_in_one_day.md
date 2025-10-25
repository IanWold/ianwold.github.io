;;;
{
	"title": "How to Write an App in One Day",
	"description": "A couple thoughts on being intentional and focused when writing a bit of software",
	"date": "25 October 2025",
	"contents": false,
	"hero": "photo-1758689401435-96aaec5b4697",
    "topics": ["How-To", "Processes", "Projects"],
    "related": [
		{ "title": "Shenanigans as an Accelerator", "description": "A short story about Wordle and scripts", "fileName": "shenanigans_as_an_accelerator" },
		{ "title": "The Art of Hype-Driven Development", "description": "The five simple steps to embracing and weilding the hype cycle for drudgery and someone else's profit.", "fileName": "the_art_of_hype_driven_development" },
		{ "title": "Thing I Made: FreePlanningPoker.io", "description": "I made a free planning poker tool and named it aptly.", "fileName": "free_planning_poker" }
    ]
}
;;;

I've been pretty short on time in the last few months; my recent rate of output on this blog might attest to that. Between learning the ropes at a new role at work and a large number of personal engagements, I haven't had a lot of time to get a lot of coding in.

Nonetheless, I've gotten a number of projects up and going. I previously wrote about [what makes a successful personal project](https://ian.wold.guru/Posts/successful_personal_projects.html), but I'm thinking now I left a lot out: what is there to be done in the face of serious time constraints? Or, potentially more interesting, are these time constraints a benefit? I think I can give some good thoughts on the former question, at least.

I've found that you can get a lot done in a single day. You can even get a lot done in a few hours in the morning in a cafe; I'm writing this now from a quiet cafe on a rainy day (this is a productivity cheat code, at least for me). Building something meaningful in this time frame just reduces down to being organized: set a reasonable goal, know what that goal is, implement realistic steps to getting it done, pay for your coffee, and you're now the proud owner of a whatever you just made!

I guess this is just a more practical elaboration on my personal projects article...

## Rome wasn't built in a day but "todo" apps can be built in a couple hours

Most of the projects I have in incomplete states are there not because I didn't set a realistic goal for something to build but because I didn't understand well-enough what I wanted to build. I don't think it's reasonable to expect specific definitions to rise out of an active coding session; if you don't know what you want to code then writing code is putting the cart before the horse.

This is maybe homework to do before sitting down then, but the advantage is it can be done over several days during the free moments we get for thinking. It pays to be more thorough in this thinking than less - what are all the models I need to handle, what user actions are there, what's the service architecture going to look like, how do the user actions inform the code architecture, what technologies do I know that will enable me to implement this, what am I lacking in knowledge that's going to be a blocker?

To boot, these questions don't just need answers but reasonable answers. You're not going to build your cool app if you start in a language you've never seen before. It's perfectly reasonable to have a project in a brand new language, but this article is focusing on trying to maximize _how much thing_ can be built in a short time.

If you know your service architecture, user actions, and models pretty well then you're 80% of the way there, and that's usually where I start. It's good to know your knowns and unknowns, and it's good to know the specific technologies you'll need to employ to get the thing off the ground. To be very thorough, it's great to think through all the extra tidbits that tempt us on the way: does the project really need a full OTEL dashboard, do I really need a revproxy, does a side project really need automated tests, and so on (the answer should probably be "no" for most of these).

## Just use the thing you know, and use it well

To reiterate, this is from the mindset of wanting to focus on delivering the thing; I don't want to be taken as advocating for never learning new bits. I have a grab bag of technologies, providers, and libraries that I reach for when I need something that isn't going to be a blocker. When I need a database [I just use Postgres](https://ian.wold.guru/Posts/just_use_postgresql.html), I almost exclusively use C# or Go, I do exclusively use GitHub and [Railway](https://railway.com/) to build and deploy. I like [Clerk](https://clerk.com/) for auth, [Stripe](https://stripe.com/) for payments, and [NATS](https://nats.io/) as a message bus.

You'll have a different grab bag than I do, to be sure. The point is that these are things that I _know_, I don't have to spend time wondering how to do _this_ or _that_ as I'm building an application. If I want to deliver the thing, I want to minimize the number of things I don't know, and focus my energy on tackling those things. If you think about it, the amount of time required to deliver a project is a sum of two numbers: the time required to get through the setup for all the things I know, and the time required to think through, experiment, and debug the things I don't know. The size and complexity of your app influence the first number, and the number of unknowns influences the second number; both potentially exponentially.

What's just as important as choosing the right tools is using them correctly. If you're reading my blog, you should know the software development loop: 1. make it work, 2. make it work right, 3. make it pretty. This loop should be as tight as possible, you should _not_ skip step 3, and you should start with the most difficult and necessary things.

Let me elaborate on that. When we start a project, we're probably going to have some kind of startup template for whatever language/library we've chosen. Maybe too frequently, we see that run locally and then we dive into sketching out the UI or server calls. This is not just disrespecting the loop, it's also not tackling the hardest problem first! We've done step 1, but no doubt the standard app templates aren't _quite_ right for you; you'll need to change some low-level things and clean up the code to your preference. Do that first!

Then, what's the next hardest thing in setup? Yes, setup is generally easy, but what's the next hardest thing? I find most bugs come from deploying the app on the cloud if you're doing a website, or setting up the installer if it's a desktop app, or whatever deployment deal for whatever other thing. If I'm setting up a web app my next step is to get a GitHub repo and deploy it on Railway, and I'll even just start with the server at that. Invariably there's something that's a different version or been improved since I did this last month that needs to be ironed out. Get it working right, then go over it all again to make sure it's pretty. Keep the work area clean and you can go much faster.

Then when I add my Postgres or Clerk auth or whatever else I need to set up, I can see it work locally _and_ right away I'll validate that it works when deployed. Sooner than later I'll also deploy a production version (unless that deployment _is_ the production version; that's a great time-saver). Particularly if you have third-party auth, separate environments can be a little hairy. After we're set up, what are the unknowns we've identified? These are the next hardest things. 

By always tackling the difficult things up front you're always keeping your time use most efficient. Practicing the development loop tightly means that at every step you're not just marching towards the finish line, but you're resolving error states, edge cases, code quality, and keeping the UI looking right. You're not deferring little things to the end of the project because those little things add up (note here: this is how you avoid the 80/20 trap). The development loop means we're not exploding our backlog, our app will be done when we're finished working through our user actions.

## And don't forget to pay for your coffee

I like working from the cafe near my house, that's a productive spot for me. Environment can be such a huge factor in our ability to function, not just at work or when writing an app but always. If you understand the environment that makes you happy you'll find it's much easier to sort through a project.

I suppose "do what makes you happy" is just obvious life advice though?
