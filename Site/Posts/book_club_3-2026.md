;;;
{
	"title": "Book Club 2/2026: Nerdy Type Theory Links",
	"description": "Yeah I just read a bunch of boring nerd things this month",
	"date": "29 March 2026",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Patterns", "Languages", "Standards"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 1/2026: Constraints", "description": "Taking a top-level look at the varied and nerdy topic of 'constraints'", "fileName": "book_club_2-2026" },
		{ "title": "Book Club 1/2026: C#, Without Allocation", "description": "Some resources on writing allocation-less (or -free) C#", "fileName": "book_club_1-2026" },
		{ "title": "Book Club 10&11/2025: .NET 10, C# 14", "description": "Another Thanksgiving, another .NET!", "fileName": "book_club_10-11-2025" }
	]
}
;;;

This book club is a bit of a throwaway on a subject that's more esoteric than my usual; in my defense, I've been spending most all my time on a major effort at work (hello, boss: I promise I'm actually working!) Following my [last book club post](https://ian.wold.guru/Posts/book_club_2-2026.html), I've been reading more type theory things in my free time.

I figure that type theory is an important thing for most software engineers to have. Heck, most software engineers have a degree in Computer Science (myself included), and though that seems like overkill for most of us (myself certainly included) I would wager that most of us do have an interest in proper CS anyway. I would argue that fundamental knowledge of type theory is at least up there with performance, runtime, and complexity analyses.

Unless you only program in JS and Python, you've got _some_ intuition of type theory. `int a = "hello"` does not compile in most languages. We can get very far with this intuition indeed, but for the extra-curious I think that some basic academic exploration can double our ability to do useful things with types. I enjoy whenever I can teach someone that because type systems are (typically) isomorphic to the predicate calculus they can (typically) be used to write proofs - sometimes even about the validity of their own programs. That sentence usually takes some practical examples to properly elucidate but I'll just leave that here.

It is sufficient to say that _very powerful_ things can be done with types. Every time someone tells me they thing I've written a particularly interesting piece of code, it's because I've done something clever leaning on the language's type system. While we keep in mind that _clever_ code is more often _unmaintainable_ code, I can at least rest assured that I have the coolest party tricks to whip out. At least at the ... stupid code party. Or something. Let's move past that.

The Lambda Calculus is the foundation for most thining in type theory, understanding it is important. I've linked a very nice explainer I've come across. From there, I link a bunch of odd type things I've read this month. I make no promise that reading these in any order will give you the powers of the cool party tricks at the type theory party. These are just what caught my eye recently.

* [A Look at Typed Lambda Calculus - Nikolay Yakimov](https://serokell.io/blog/look-at-typed-lambda-calculus)
* [An Operational Semantics of Simply-Typed Lambda Calculus With String Diagrams - Emily Riehl](https://golem.ph.utexas.edu/category/2024/07/an_operational_semantics_of_si.html)
* [Relating Homotopy Equivalences to Conservativity in Dependent Type Theories with Computation Axioms - Matteo Spadetto](https://arxiv.org/pdf/2303.05623v4)
* [Advanced Topics in Type Theory - Sarah Lee](https://www.numberanalytics.com/blog/advanced-type-theory-dependent-types-homotopy) (More links)
* [Isomorphism invariance and isomorphism reflection in type theory - Andrej Bauer](https://math.andrej.com/2023/06/15/types-2023-isomorphism-invariance-and-isomorphism-reflection/)
* [A bunch of research by Dan Licata](https://dlicata.wescreates.wesleyan.edu/pubs.html)