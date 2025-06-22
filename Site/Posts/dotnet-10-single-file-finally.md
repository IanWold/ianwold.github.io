;;;
{
	"title": ".NET 10's Single File Feature is Perfect ... Almost",
	"description": "Being able to run standalone CS files has been a huge gap in .NET preventing C# from being able to properly address scripting scenarios, but the initial look at the solution is very promising.",
	"date": "22 June 2025",
	"contents": false,
	"hero": "photo-1546638008-efbe0b62c730",
    "topics": ["Dotnet", "Projects"],
    "related": [
		{ "title": "Why I Have This Blog", "description": "Reflecting on the last year of blogging.", "fileName": "why_i_have_this_blog" },
		{ "title": "Publish Your Blogroll Now", "description": "I've done it and so can you; in fact, I've made it easier for you - way easier!", "fileName": "publish_your_blogroll_now" },
		{ "title": "Four Deeply-Ingrained C# Cliches", "description": "There's a lot to love about C# and .NET, and there are some things that I don't love as much. Then there are four bad habbits that are so deeply ingrained they've become cliches within our codebases.", "fileName": "four_deeply_ingrained_csharp_cliches" }
    ]
}
;;;

.NET 10 is finally introducing a feature that is some 20 years (give or take a few) too late, allowing a single CS file to be ran as a standalone program, not requiring any solution, project, or Nuget files. This is something I've been needing for some time - yes, needing, not wanting - so I just installed the preview version for .NET 10 and I'm glad to see that it's very nearly working perfectly out of the box.

For background, my use case is using C# to script builds in GitHub actions, particularly the static generator for this blog. For some time I've been (more or less) keeping up my project [Metalsharp](https://github.com/IanWold/metalsharp), a C# version of the JS static site generator [Metalsmith](https://metalsmith.io/). I'm really happy with the project and I and other colleagues have used it successfully in a pretty wide set of applications. The only problem is that C# doesn't _really_ work as a scripting language; it's compiled. It is a bit ridiculous to suggest that a build agent using my tool should include a build step to build the script that builds the site, so I turned to the [dotnet-script](https://github.com/dotnet-script/dotnet-script), a dotnet tool that allows a single CS-esque file to be executed with a single command.

There were a few troubles with that workflow: The build agent needed to include a separate step to install the dotnet-script tool, I generally prefer not to rely on third party things for very foundational components of any project, and the builds took a long time to execute. What would take a fraction of a second on my local machine debugging with a CSPROJ file took 60 seconds in GitHub actinos with dotnet-script. The biggest problem though was the local development experience: I've never been able to get any intellisense for dotnet-script in any IDE, so I'd have to build the project to see errors. Alas, there was no other way to run a single CS file without a build step.

This all prevented me from considering Metalsharp to have reached v1, and I haven't yet published or publicized the project. I hope this sets up how excited I am that .NET 10 is going to be including this feature: I'll be able to "officially" release this years-long project if it's a stable foundation for development and production.

As I mentioned, I did get it up and going and the initial indication is a great success. Importantly, it works out of the box exactly how I'd expect; there are no surprises or hidden configurations or the like to get it to work locally or in my GitHub Actions. It does indeed solve most of the problems I've had with dotnet-script: I need no extra step to include the feature with `dotnet`, the feature is a first-party _part_ of .NET, and what had been taking 60 seconds to run in GitHub is now taking 8. To me, this is a runaway success, except for this one issue: it _still_ doesn't have intellisense support, at least not that I can find.

This is a preview feature of .NET so I'm expecting that this will be fixed when it is released in November, but for the time being it is still quite annoying. Nonetheless, in the next couple of months I'm going to be preparing my Metalsharp project to bump to v1 with .NET 10, and I'm very happy about that!