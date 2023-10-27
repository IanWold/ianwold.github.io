;;;
{
	"title": "Book Club 10/2023: Functional Patterns in C#",
	"description": "This month I've focused on functional domain modeling and related patterns. We're just a few weeks away from the release of the next version of C#, and like each previous version it'll introduce even more functional features.",
	"date": "27 October 2023",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "related": [
		{ "title": "An Introduction to Sprache", "description": "Sprache is a parser-combinator library for C# that uses Linq to construct parsers. In this post I describe the fundamentals of understanding grammars and parsing them with Sprache, with several real-world examples.", "fileName": "sprache" },
        { "title": "Meanwhile in the Windows Console ... Minesweeper!", "description": "My tyranny of classic game implementations in the console expands. Knows it no end?", "fileName": "console_minesweeper" }
    ]
}
;;;

_My Book Club is a monthly curated list of things I've been reading or watching, sent out via my newsletter. If you'd like to follow along with me, please [subscribe to my newsletter](https://buttondown.email/ianwold)._

Happy spooky season!

This month I've focused on functional domain modeling and related patterns. We're just a few weeks away from the release of the next version of C#, and like each previous version it'll introduce even more functional features. We still aren't getting discriminated unions, but as C# becomes more functional, these patterns are becoming increasingly more attractive. Traditionally, C# is written in OO or procedural styles, and from my perspective there doesn't seem to be a great deal of discussion among C# engineers about incorporating functional patterns. Maybe you run in different circles, but I think there's room for improvement across the board here. Even the result monad, which can be used within an entirely OO context, is infrequently implemented.

I think it's important to be discussing functional patterns in C#, for a few practical reasons:

* Eventually we're getting DU, and that's going to change a lot of things
* Our F# colleagues are doing great work in this area, we should engage them more
* Using functional patterns is cool

But above all, if C# supports these patterns and they can help us write better code, _why would we neglect them_? To explore this topic, I've curated a set of talks by Scott Wlaschin and Mark Seemann who both do a great job explaining functional programming from a conceptual perspective, demonstrating its power in F#, and ultimately demonstrating C# equivalents.

I hope these talks make you all as excited for discriminated unions in C# as I am!

* [Domain Modeling Made Functional - Scott Wlaschin (2019)](https://www.youtube.com/watch?v=2JB1_e5wZmU)
* [Functional Design Patterns - Scott Wlaschin (2017)](https://www.youtube.com/watch?v=srQt1NAHYC0)
* [The Power of Composition - Scott Wlaschin (2018)](https://www.youtube.com/watch?v=WhEkBCWpDas)
* [The Lazy Programmer's Guide to Writing Thousands of Tests - Scott Wlaschin (2020)](https://www.youtube.com/watch?v=IYzDFHx6QPY)
* [Functional Architecture - The Pits of Success - Mark Seemann (2016)](https://www.youtube.com/watch?v=US8QG9I1XW0)
* [Get Value out of Your Monad - Mark Seemann (2018)](https://www.youtube.com/watch?v=F9bznonKc64)
* [From Dependency Injection to Dependency Rejection - Mark Seemann (2017)](https://www.youtube.com/watch?v=cxs7oLGrxQ4)
* [Dependency Injection Revisited - Mark Seemann (2018)](https://www.youtube.com/watch?v=qBYVW4ghMi8)
