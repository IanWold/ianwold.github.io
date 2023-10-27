;;;
{
	"title": "Giscus Is Awesome",
	"description": "I can add comments to my statically generated blog? Using GitHub Discussions?? For Free??? And it works????",
	"date": "14 September 2023",
	"contents": false,
	"hero": "photo-1591799589332-7a340007f79f",
    "related": [
        { "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "fileName": "deploying_aspdotnet_7_projects_with_railway" },
		{ "title": "An Introduction to Sprache", "description": "Sprache is a parser-combinator library for C# that uses Linq to construct parsers. In this post I describe the fundamentals of understanding grammars and parsing them with Sprache, with several real-world examples.", "fileName": "sprache" },
        { "title": "The Outrage Engine", "description": "Perhaps an ASCII game in the Windows Console is ridiculous. Something akin to Dwarf Fortress comes to mind, so it's not entirely off the mark. But a game engine devoted to ASCII games in the console? Perhaps that's outrageous. I don't know if it's been done (or is being done) currently, but that's what I'm doing right now, and I've called it the Outrage Engine.", "fileName": "outrage_engine" }
    ]
}
;;;

[giscus.app](https://giscus.app/) is really awesome!

Last week I posted for the first time in six years and I figured I wanted to see about adding comments to this site. A Google search got me to Giscus really quick, and I was able to wire it up in just ten minutes. The thing that's still blowing my mind is that _it works_.

Behind the scenes, it syncs up with the GitHub Discussions tab on the repo that hosts this website, and it matches a discussion to a page based on the the page's title.

When somebody adds the first comment to a page, it creates a corresponding discussion thread for the page. When a page loads, it checks to see if there is a corresponding discussion and it loads the conversations from that discussion thread.

I can do comment moderation and whatnot on GitHub discussions, and if somebody stumbles upon my website on GitHub they can see the conversation right there. If the tool stops working, the conversations still exist in GitHub, living right alongside the source for this site.

And - I can't emphasis this enough - _it just works_. I see so few tools that _just work_ and this one does.

Check it out!
