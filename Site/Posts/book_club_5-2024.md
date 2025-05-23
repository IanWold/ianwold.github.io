;;;
{
	"title": "Book Club 5/2024: SOLID",
	"description": "Is SOLID still relevant?",
	"date": "29 May 2024",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Architecture", "Patterns"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 4/2024: I Don't Like ORMs", "description": "Object-relational mappers are more trouble than they're worth.", "fileName": "book_club_4-2024" },
		{ "title": "Book Club 3/2024: Simplicity", "description": "Everything is too complicated.", "fileName": "book_club_3-2024" },
		{ "title": "Book Club 2/2024: Recovering from TDD and Unit Tests", "description": "TDD and unit tests are overused and often misprescribed. What do we really hope to gain from our tests, and what testing practices support our goals?", "fileName": "book_club_2-2024" }
	]
}
;;;

Happy Memorial Day to those in the US, I haven't had time to post a lot this month but hopefully this one is controversial enough to make up for it! [SOLID](https://en.wikipedia.org/wiki/SOLID), as I imagine you know (having subscribed to this newsletter), is a set of principles for developing software introduced a little over 20 years ago by - look at that - Uncle Bob. It's five guidelines which seek to constrain how we write software in such a way as to make it more likely that "clean", maintainable code is written.

I have some problems with each of these principles specifically, but in the general sense I can say a couple things. It's my (admittedly evidentially unsubstantiated) observation that we as an industry tend to latch on to dogmas very quickly. Various buzzwords, libraries, architectures, and patterns all have their five minutes of fame, so to speak, finding themselves the focus of momentary and ill-considered obsession. Some of these come and go very quickly (As an example, I don't know what's popular on NPM these days but ... probably that), and some of them last a bit longer (microservices). Some of them last far too long, and SOLID is one of those.

By hyper-focusing on fad concepts, we are doing ourselves and an industry a disservice in several ways. Obviously, saddling future engineers with the bad ideas we didn't think through now is bad, but the more interesting aspect to me is that by focusing our individual attentions on these fads we're stealing our own time away from ourselves to develop thorough and rigorous skills in actually foundational skills. Taken to the extreme, an obsession with silver bullets blinds some among us from the understanding that there's nothing but hard work and careful consideration at the center of this industry.

So I tend to avoid the fads as they come and go, and looking around it seems to me like SOLID is on the way out, but I'd like to contribute a drop in the bucket of the anti-SOLID sentiment. Really I'd like to be writing in _favor_ of diligence and developing an understanding of which tools fit which jobs, but SOLID serves as a great contrary example. I'll briefly touch on each of the five principles -

**S**ingle Responsibility Principle - this causes more arguments and mangled code than it helps anything. At the surface it is absolutely correct to say that any given module should have a single purpose, but the trouble comes not just in defining what a "single responsibility" is but also defining how to define what a "single responsibility" is. Great, now I need a whole ontology of orders for definitions to resolve a PR! Should "single responsibility" mean that each iota of code is doing the bare minimum of things? Should it mean what Uncle Bob suggests, that it only has one "business reason" to change? Good luck defining that one too. And what is a module anyway - do we apply this to methods, classes, _and_ namespaces? Oh, not namespaces - why? There's another fight.

**O**pen-Closed Principle - This gets into the difficulties with inheritance in general, that being the complexity incurred by using it. This principle doesn't exacerbate the troubles with inheritance, but in my opinion it doesn't really do anything to constrain inheritance problems. Superficially sure - it says I shouldn't modify the base class! Okay, fair enough. In practice I don't really see that ever being an actual issue that comes up, rather that any use of inheritance adds complexity. There are limited cases that inheritance is useful when developing boilerplate logic, but it's never necessary to use. I think this principle just disappears by properly limiting or eliminating your use of inheritance.

**L**iskov Substitution Principle - Again runs into the problems with inheritance, but this principle also applies to interfaces, which I like [when used appropriately](https://ian.wold.guru/Posts/four_deeply_ingrained_csharp_cliches.html#interfaces). I think that overall the use-case for interfaces is less than they tend to be used for - certainly so in the C# world - but when they're used I actually think the Liskov principle is a good one to keep in mind, generally. That said, when's the last time you saw some code checked in that might have actually violated this principle? A lot of the times this might come in from one interface implementation violating the not-documented contract for a particular method (i.e. "return null in this case to signify such and such"). While Liskov could be said to capture this, I don't think it does so explicitly enough, and so a better solution might be to adopt some principle which ameliorates the contract violation concern, specifically.

**I**nterface Segregation Principle - It's fair enough that a consumer of a module shouldn't be required to consider a non-relevant interface. This principle is good in that sense, but it runs afoul of being too nonspecific - we start getting arguments again. "This interface should really be broken into three interfaces" when the interface declared two methods is a frustrating conversation I've had in some form twice. Being clear, this isn't nearly as much a problem as SRP is, and this principle has its utility. My issue with it is similar to Liskov, I think there are other practices out there which more appropriately constrain our manner of development with respect to interfaces. That would be a good blog for me to blog...

**D**ependency Inversion Principle - the way this typically gets implemented, at least common to C# codebases, really grinds my gears. I wonder if this one ends up being misunderstood a lot, but I can't really say, it does seem to imply that every class should have an interface. This is a ridiculous notion, as I've discussed previously and in my article I linked above for Liskov. Utility classes with few methods and only one implementation definitely don't need an interface, and anything with only one implementation doesn't. An often-used objection here is that I might want an interface if I ever need to refactor by creating a parallel implementation, but the response is obvious: create the interface at that point in time. Maybe this was more difficult back in the day, but with modern development tools it's not difficult to do that.

Upon reflection, it's quite funny how SOLID is one bad argument-generating principle followed by four milquetoast suggestions regarding interfaces. It seems like SOLID might do well to be replaced by a set of principles on the appropriate handling of abstractions and interfaces, but to my previous point I think it's a disservice to focus on these catchy - for lack of a better word - memes. Properly, it would be better for the industry to discuss principles in an `if-this-then-that` format: given situation X, principles [A1, A2, ..., An] are beneficial because [R1, ... Rn]. I hope you all concur with me that the use of quasi-mathematical notation immediately makes my point more rigorous.

**Reading and Watching**

* David Bryant Copeland has a whole series on SOLID: [SOLID Is Not Solid](https://naildrivin5.com/blog/2019/11/11/solid-is-not-solid-rexamining-the-single-responsibility-principle.html)
* [Inheritance Is Evil. Stop Using It. - Nicolò Pignatelli](https://codeburst.io/inheritance-is-evil-stop-using-it-6c4f1caf5117)
* [Is SOLID Still Relevant in Modern Software Architecture? - Vasco Veloso](https://www.infoq.com/news/2021/11/solid-modern-microservices/#:~:text=According%20to%20Orner%2C%20while%20the%20practice%20of%20software,to%20functional%20programming%20and%20microservices%20architecture%2C%20with%20examples.)
* [Solid Programming - No Thanks - The Primeagen](https://www.youtube.com/watch?v=TT_RLWmIsbY) - this just happened to come out this month
* [Where Does Bad Code Come From - Casey Muratori](https://www.youtube.com/watch?v=7YpFGkG-u1w)
* [Ranking the SOLID Principles - Nick Chapsas](https://www.youtube.com/watch?v=ETdulc1xk04)