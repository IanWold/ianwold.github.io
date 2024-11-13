;;;
{
	"title": "Hot off the Press: .NET 9, C# 13",
	"description": "The AI will continue until morale improves!",
	"date": "13 November 2024",
	"contents": false,
	"hero": "photo-1697577418970-95d99b5a55cf",
    "topics": ["Dotnet"],
    "related": [
        { "title": "Learn the Old Languages", "description": "New languages are hip, old languages are erudite. Don't neglect these languages as you round out your skills.", "fileName": "learn_the_old_languages" },
        { "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "filename": "deploying_aspdotnet_7_projects_with_railway" },
		{ "title": "Book Club 11/2023: New .NET, New C#", "description": "The release of .NET 8 brings a lot of features I'm excited for!", "fileName": "book_club_11-2023" }
    ]
}
;;;

The new .NET and C# are out as of yesterday, and I'm underwhelmed. Well, maybe since we've known for some time what was in this release I should just be whelmed.

AI being the current hype, Microsoft is placing front and center what it can find related to AI. On the positive side, this means some updates to `Tensor<>` and vectors, though these don't impact me on the day-to-day. Most of what they've got seems to just be a version bump for AI clients from OpenAI and such. In fact, I'm a bit hazy on what real problems the set of "AI" features are solving other than giving them the ability to do presentations about how ".NET 9 enhances support for AI." If you've got more information please comment below.

Then they're really pushing .NET Aspire, which I continue to skeptically watch; is Aspire not a conspiracy by big Azure to sell more cloud? I can't imagine that's not part of the calculus, but Aspire has a couple cool things, the local dashboard is neat, as is the container orchestration. It still seems like a quite opinionated sort of boilerplate I need to write to get it up and going when existing tools (docker compose and the tooling around that) take just as much code, have been around longer, and give me most of the same advantages.

Aspire has some neat logging improvements now on their nifty dashboard, but you get this with other standard otel tools like [Honeycomb](https://www.honeycomb.io/). Even the local development experience doesn't seem to be necessarily _better_ than the experience with existing tools, just _different_. Aspire does make it very easy to throw on distributed components once I've got an app up and going with it, but not _easier_ than my existing setup. I feel like I have to be missing something here, but if I'm not then this is just a more architecturally-constraining way to distribute your application. Am I becoming the old man yelling at the cloud? If you have an answer, please let me know.

Then there's a decent smattering of improvements across the board for ASP, SignalR, and Blazor. I'm excited to update a few applications for these benefits, but they're just the standard sort of regular improvements. Which is good - don't get me wrong - just whelming.

The coolest such improvemnt is that SignalR now allows covariant types, with the caveats that you're using `System.Text.Json` and are comfortable throwing some attributes on your models. I'm going to see about setting up covariant result types with this, possibly as a follow up to my article on [properly using the result pattern in C#](https://ian.wold.guru/Posts/roll_your_own_csharp_results.html) for use in SignalR.

C# doesn't have a lot going on, which is maybe a welcome change of pace considering each version since 7 has introduced very new things. It does leave a conspicuously discriminated-union-shaped hole in these release notes, though. To be fair, it's my impression that such an addition has a fair enough amount of complexity that a slower and more deliberate approach is needed. Fair.

The annual [performance article by Stephen Toub](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-9/) is excellent as always, and excitingly (for me at least) seems to reveal a lot of work put into compilation in this release. That's the best news - it suggests improvements across the board.