;;;
{
	"title": "Book Club 11/2023: New .NET, New C#",
	"description": "The release of .NET 8 brings a lot of features I'm excited for!",
	"date": "22 November 2023",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
	"series": "bookclub",
    "related": [
		{ "title": "A Scrum Odyssey", "description": "A journey away from daily scrum meetings, as a cycle of eight Shakespearean sonnets.", "fileName": "a_scrum_odyssey" },
		{ "title": "Book Club 9/2023: Papers I Love", "description": "Reflecting on the final Strange Loop conference, having attended several 'Papers We Love' talks, I'm motivated to share five papers I love.", "fileName": "book_club_9-2023" },
		{ "title": "Book Club 10/2023: Functional Patterns in C#", "description": "This month I've focused on functional domain modeling and related patterns. We're just a few weeks away from the release of the next version of C#, and like each previous version it'll introduce even more functional features.", "fileName": "book_club_10-2023" }
    ]
}
;;;

I'm looking forward to turkey day tomorrow gobble gobble! This year I'm thankful that I work in ecommerce so I get to have a peaceful extended weekend because nobody visits ecommerce sites on Thanksgiving weekend. At least that's what they told me in the interview before they hired me. _Insert joke about how it's better to be working in ecommerce than at OpenAI this weekend regardless..._

Anyway, last week we got the new release of .NET, which brings langauge and tooling updates across the board, so I want to focus on some of those fun things.

First, Blazor has taken the third (or is it fourth now) of the 1,000 steps it needs to take to become a viable platform for SAAS, with hybrid client/server rendering. I don't have a lot to say there, but I use Blazor for a number of personal projects when I need to quickly draw up a UI to look into some .NET backend scenario or another.

Since Microsoft started down the path of .NET Core, the whole ecosystem has been embracing OSS and free software in a way that's completely rewritten the whole modus operandi of Microsoft under Nadella. Indeed, it seems like at this point in time, you can use .NET without a single worry about vendor lock-in. Well, Microsoft is here to save you from that horrible wasteland of unrestricted freedom with [.NET Aspire](https://devblogs.microsoft.com/dotnet/introducing-dotnet-aspire-simplifying-cloud-native-development-with-dotnet-8/). Nevermind that [you probably don't need a distributed system](https://www.fearofoblivion.com/build-a-modular-monolith-first), and even if you did [you almost certainly don't need microservices](https://renegadeotter.com/2023/09/10/death-by-a-thousand-microservices.html), they have cloud computes to sell you! Aspire makes it easy to avoid footgunning yourself as you begin your next project distributed from the start by skipping you right to the step where you blow your foot off with a bazooka - all hail the mighty Azure! Or, you know, if like 99.99% of all apps out there you'd be fine with it deployed in a Docker container with a couple of related services, [you could just use Railway](https://ian.wold.guru/Posts/deploying_aspdotnet_7_projects_with_railway.html). _Note that I'm definitely queuing up an article on using Aspire with Railway despite my skepticism that Aspire is a good idea._

[C# hasn't gotten too many updates](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12), but we have two syntactical updates that are essential and should have been included much earlier: collection expressions and primary constructors for classes.

Collection expressions, or perhaps "enumerable literals", should have been a part of the language from the start, and you should convert all of your code over to using these.

```csharp
var list = [1, 2, 3, 4, 5];
```

Records have had primary constructors since they were introduced, and I think that was largely related to the desire to have tuple interop (is interop the right word here?), but now classes have them too _except quite different_. The parameters in a primary constructor for a class are, more logically than records, private members of that class, significantly reducing the amount of boilerplate [if you're still using dependency injection](https://ian.wold.guru/Posts/book_club_10-2023.html). It's been how many years since Scala came out, but now we can be one of the cool kids on the block too! Right?

Anyway, I'll just leave you with a few talks from the .NET conference with some of the other tidbits that should be used in .NET going forward:

* [Improving your application telemetry using .NET 8 and Open Telemetry](https://youtu.be/BnjHArsYGLM?si=NsnqXLMKwcmirGZM)
* [Tiny, fast ASP.NET Core APIs with native AOT](https://youtu.be/FpQXyFoZ9aY?si=qhDqySjMAOrxa_9x)
* [From IL Weaving to Source Generators, the Realm story](https://youtu.be/qXsRz0YWvu4?si=p9oaPMq8h4an1Fq5)
* [All About C# Source Generators](https://youtu.be/Yf8t7GqA6zA?si=WoidTSJRaUe4be-0)

And then a couple of talks from the lead designers of C# and F# regarding the history and direction of each language:

* [Where's C# Headed? - Mads Torgersen (2022)](https://www.youtube.com/watch?v=v8bqAm4aUFM)
* [The Functional Journey of C# - Mads Torgersen (2022)](https://www.youtube.com/watch?v=CLKZ7ZgVido)
* [The F# Path to Relaxation - Don Syme (2021)](https://www.youtube.com/watch?v=sC0HUq2KkFc)
* [What's new in F# 5 & 6 - Don Syme (2021)](https://www.youtube.com/watch?v=MXKM5dSk_8o)
