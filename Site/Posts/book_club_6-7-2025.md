;;;
{
	"title": "Book Club 6&7/2025: OOP",
	"description": "Just some blabbering about OOP, paradigms, and being non-dogmatic",
	"date": "30 July 2025",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Postgres", "Databases"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 4&5/2025: Incidents and Resiliency", "description": "Thinking more about responding to and preventing incidents", "fileName": "book_club_4-5-2025" },
		{ "title": "Book Club 2&3/2025: Slack: Communication, Organization, Teams", "description": "On various aspects of teamwork, communication, and the like", "fileName": "book_club_2-3-2025" },
		{ "title": "Book Club 1/25: Results, Railways, and Decisions", "description": "A story on a successful result and insights gained from some more heady research this month.", "fileName": "book_club_1-2025" }
	]
}
;;;

Happy summer! It's been a couple months since I checked in on my newsletter; I've been busy enjoying sweltering heat, lots of rain, and wildfire smoke. In truth, I've been focusing more on a new role at work than my blog, but I promise I have found plenty of gripes there that I will resume my regular posting cadence here shortly! These last two months though, I've been thinking about object-orientation, so today you get some ramblings at a high level.

As ideas ebb and flow in popularity in the software world, OOP is perhaps a bit on the ebb nowadays. Certainly it's not difficult to understand a contemporary distaste with OOP. At the risk of gross oversimplification, Java and C# engineers (and others I'm sure) have over the last several decades developed a style of programming which emphasizes overengineering and overarchitecting, developing massive codebases of passthrough layers, pointless interfaces, useless abstractions over sometimes useful abstractions; no doubt all reflections of the sometimes bizarre bureaucratic structures of the larger firms that typically employ these languages. This is the monster a lot of our colleagues envision when hearing "object-oriented programming," and they're not entirely off-base.

While this is what OOP means these days - at least colloquially - any rigorous definition of OOP isn't going to make it obvious that such code necessarily results from the paradigm. Indeed, surely in a decade there will be a revival of "classic OOP" or some nonsense as the popularity pendulum overcorrects far to the other side. These "classic OOP"ers will insist that OOP is _solely_ about objects, or encapsulation+polmorphism, or _message passing_. I feel confident in my prediction here as this sort of defense has already begun in some circles, and again it's not entirely off base. These are all suitable definitions.

On that last one, message passing: if you spend a sufficient time reading about OOP you'll uncover the group of colleagues who insist that Smalltalk is the only real OOP language. Avoiding the lower-level debate about what it means for a word to mean some thing (in this case, "OOP" to definitionally include "Java"), such a claim doesn't quite pass the sniff test to anybody with a decent understanding of the history of programming languages; the OOP ideas all came through in various stages and together form a consistent thread of ideas for approaching software architecture and engineering. Nonetheless, I'd be hard-pressed to defend the idea that the present understanding of OOP is better than the more limited Smalltalk-esque understanding; the more focused principle (or, maybe, more principled focus) of the messaging notion of OOP might well provide the necessary constraints for software to be developed better. Perhaps that's why so much new software is being developed in Smalltalk these days! Well...

Perhaps it's unfair to conclude from Smalltalk's failure that its particular paradigm is also destined to failure, but I wouldn't hold my breath. So too for other paradigms that claim some sort of purity; there are no silver bullets. Today's OOP has developed as a hodgepodge of many different ideas, forms, and learnings; this makes it as undefinable as unfocused, or maybe un-specific. There's sometimes many valid approaches to the same problem within the present OOP canon. Yet, it would be a mistake to conclude that this is a bug instead of a feature, for the very reason that OOP has developed in this way is precisely _because_ it has been forced to solve just about every programming task under the sun. Recognizing that different problems will require (sometimes very) different paradigms to optimally solve, we must conclude that our languages and architectures must have the flexibility to accommodate many different approaches at once, thus making the hodgepodgeness of OOP a practical necessity.

So the contemporary, undefined, colloquially-understood OOP is not such a bad thing, from a certain point of view. What though when we come upon a situation that objects are insufficient bags for our logic, or that a more pure functional language compiles much more efficiently for some or another problem? Today's software is more varied and diverse than ever before, so our incidence rate of stubbing our toes on these situations is quite high now. Hence the unpopularity of OOP: the traditional languages within the paradigm have not been flexible enough for some of the most basic procedural or functional styles best-suited to many of these tasks. On the other hand, many traditionally-OO languages have been incorporating support for other paradigms to quite a great extent; anecdotally, much of the C# world (including myself) has moved on to quite functional-looking styles as that language has (not insignificantly) morphed to accommodate it.

The ideal future of OOP, should we all continue to agree this is the desirable path, is maybe a sort of fading into oblivion in which its constituent ideas are considered coequal with those from the other paradigms, and our languages and runtimes support a fluid combination of whichever structures we might discover are the most efficient solutions to our problems. There's certainly plenty of momentum in this direction, but making our tooling more permissive won't stave off any potential future hype for some "pure" form of OOP or functional or what-have-you programming; nothing can replace solid principles. OOP, by any definition, isn't good or bad, nor is it any one specific thing.

**Watch**

* [The Big OOPs: Anatomy of a Thirty-five-year Mistake - Casey Muratori](https://www.youtube.com/watch?v=wo84LFzx5nI)
* [Object-Oriented Programming is Good](https://www.youtube.com/watch?v=0iyB0_qPvWk), [Object-Oriented Programming is Bad](https://www.youtube.com/watch?v=QM1iUe6IofM), and [Object-Oriented Programming is Garbage](https://www.youtube.com/watch?v=V6VP-2aIcSc) by Brian Will
* [Odin creator Ginger Bill on his programming language and state of software! - Wookash Podcast](https://www.youtube.com/watch?v=0mbrLxAT_QI)
* [Stop Writing Classes - Jack Diederich](https://www.youtube.com/watch?v=o9pEzgHorH0)
* [Seminar with Alan Kay on Object Oriented Programming](https://www.youtube.com/watch?v=QjJaFG63Hlo)
* [Cuis Smalltalk and the History of Computing’s Future (with Juan Vuletich) - Developer Voices](https://www.youtube.com/watch?v=sokb6zZC-ZE)