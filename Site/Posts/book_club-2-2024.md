;;;
{
	"title": "Book Club 2/2024: Recovering from TDD and Unit Tests",
	"description": "TDD and unit tests are overused and often misprescribed. What do we really hope to gain from our tests, and what testing practices support our goals?",
	"date": "24 February 2024",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": [Architecture", "Testing"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 1/2024: What is a Software Architect?", "description": "A few meandering and maybe unhelpful thoughts on the title \"Software Architect\"", "fileName": "book_club_1-2024" },
		{ "title": "Book Club 12/2023: Workflow, Process, and Agile", "description": "Some thoughts on how to organize software development and teams, and how non-technical factors help (or hinder) us developing better software.", "fileName": "book_club_12-2023" },
		{ "title": "Book Club 11/2023: New .NET, New C#", "description": "The release of .NET 8 brings a lot of features I'm excited for!", "fileName": "book_club_11-2023" }
	]
}
;;;

I'm on the record as having labeled myself a [unit test hater](https://ian.wold.guru/Posts/four_deeply_ingrained_csharp_cliches.html#unit-tests). This is perhaps a bit extreme taken at face value; I like to advocate for approaches which consider the pros and cons of all solutions, and matching the right solutions to the problems they best solve. More often than not, unit tests are not the right solution; largely I find they corrupt our codebases, encouraging us to twist our architectures and use poor engineering practices. To be sure though, there's instances that unit testing is appropriate.

But what kind of "unit testing" is good anyway? Forgive my diversion into definitions here, but this is important: are we talking about the same thing? One engineer might take "unit testing" to mean any form of testing where I'm only testing a single function or method, while another might take the same term to mean the specific practice of abusing mocks to white box test the various code paths of each method containing business logic. Can you see my bias? The former is quite agreeable, the latter is the source of much consternation. Insofar as the latter definition is a subset of the first, then I might say that I prefer the difference of the two.

Given the general kind of unit testing which is good then, we can perhaps more specifically define the proper sort. A coupling of the test itself to the implementation of the code it tests should, in all but extreme cases, be off the table. Further, [example testing](https://en.wikipedia.org/wiki/Data-driven_testing) whereby we match outputs to known inputs leads to fragile tests, missed cases, and generally avoids addressing the purpose of the method in question; the proper sort of unit tests test the relationship between the input and output, not the specific cases. Given these two properties of proper unit tests, I could specifically cite forms of parameter-based testing such as [property testing](https://en.wikipedia.org/wiki/Property_testing) and [fuzz testing](https://en.wikipedia.org/wiki/Fuzzing) as being ideals. Some others on the edge can be interesting too: [mutation testing](https://en.wikipedia.org/wiki/Mutation_testing) and contract tests both have their uses but should be strictly used within the confines of our testing rules.

Now, let me be quite clear here: drawing a line in the sand saying a unit test can't be coupled to the implementation it tests means we can't use mocks or fakes to instantiate a class to test its methods. Indeed, these testing methods are probably only proper for "pure" functions. I think this derivation is a sign I'm on the right path here; I take that indication from two more fundamental principles I hold. First, our testing is in place to test the behavior of our system, not to double or triple our implementation. Second, maximizing the amount of logic in "pure" functions is good. Perhaps you disagree with me on these; that may well lead you to a very different conclusion.

Understanding why we are testing - what specifically we want to gain with the tests - is perhaps the most important thing we need to sort out. As I stated, I'm interested in automated tests which verify that the system I've built satisfies the requirements it was given. Realistically, this should mean a full decoupling between the suite of tests and the code it tests. Here's the catch then: if I'm testing individual methods, I don't have total decoupling. I couldn't (or it would be quite difficult to) write my tests in one language and the system itself in another language, for a more extreme example. This is where I come in with functional testing strategies like integration tests - not that I'd necessarily want to use separate languages, but I want to achieve that sort of decoupling.

This doesn't work in all cases. Can I test a mathematics class library with integration tests? Almost certainly not; testing for an example like that belongs to the domain of parameterized testing and the like. A microservice API though? I'm not sure that there's a lot here for which integration tests aren't the best and obvious choice. At least insofar as I want to test the business requirements of my system, integration tests are ideal for most of our CRUD systems. These are the tests that best let me define the constraints of the system, and that guarantee that it conforms.

This brings me to the topic of BDD and TDD. You might guess that I dislike TDD, and you'd be right. Well, a bit. I dislike the idea that TDD should be prescribed. If you want to write unit tests coupled to the implementation before you write that implementation, you do you. Throw those away before you commit your changes, but bring whatever tools to the table make you the most effective engineer you can be. BDD - Behavior-Driven Development - on the other hand is something I can get behind. By my reading, BDD is just giving a name to the way I'd want to develop software anyway: that everyone developing the system should be aligned on how it's supposed to behave _before_ we implement it.

It's on this subject - TDD and BDD - that I focused most of my research this month. As I outlined, it seems to me that most issues we have with our automated testing strategies is that we seem to have a tendency to stray from the fundamentals here. Before even considering automated tests, are we aware of all of the testing strategies and patterns, and the scenarios in which each works the best? Have we fully thought through _why_ we need tests and _what they provide_, not just in general but for the specific codebase we want to test? As with any other coding tasks, we first need to approach testing from the right orientation before we can start engineering. BDD and TDD are overarching process ideas which seek to orient our approaches to testing, so I think that these and other processes are the interesting thing to consider when studying testing strategies.

Finally, I feel I should touch on _code coverage_. If my goals for my automated tests are to ensure that it meets all of its requirements, then it seems to me that I'd want some kind of metric of "requirements coverage" or the like. If my code is meeting all of its requirements - that this is genuinely a big if - then I don't really care about code coverage, do I? Take an API for example - if I have integration tests set up which cover 100% of my business cases - both happy and sad path - then code coverage is just a metric of how much code I have in my codebase that isn't getting hit. It seems to me that the utility of code coverage is not as a primary indicator of the quality of my software, but rather an incidental heuristic which I could choose to use from time to time to help refactor bits of code.

**Videos**

* [TDD, Where Did It All Go Wrong - Ian Cooper (2017)](https://www.youtube.com/watch?v=EZ05e7EMOLM)
* [TDD Revisited - Ian Cooper (2023)](https://www.youtube.com/watch?v=IN9lftH0cJc)
* [TDD, BDD & ATDD - Allen Holub (2014)](https://www.youtube.com/watch?v=-022ONzvQlk)
* [The lazy programmer's guide to writing thousands of tests - Scott Wlaschin (2020)](https://www.youtube.com/watch?v=IYzDFHx6QPY)
* [Test Driven Development: That’s Not What We Meant • Steve Freeman (2017)](https://www.youtube.com/watch?v=yuEbZYKgZas)
* [An Ultimate Guide to BDD - Dave Farley (2022)](https://www.youtube.com/watch?v=gXh0iUt4TXA)
* [STREAM VOD: ThePrimeagen vs Theo - Dev Debates on the FIRST DevHour Podcast (2022)](https://www.youtube.com/watch?v=o-HTsJ1-wdI)

**Writing**

* [Unit Testing is Overrated - Oleksii Holub (2020)](https://tyrrrz.me/blog/unit-testing-is-overrated)
* [Why I Don't Do TDD - Shai Almog (2022)](https://debugagent.com/why-i-dont-do-tdd)
* [Test-Induced Design Damage - David Heinemeier Hansson (2014)](https://dhh.dk/2014/test-induced-design-damage.html)
* [Test Coverage - Martin Fowler (2012)](https://martinfowler.com/bliki/TestCoverage.html)

The last two posts are referenced (and elaborated on) by a series of conversations by Kent Beck, David Heinemeier Hansson, and Martin Fowler: [Is TDD Dead?](https://martinfowler.com/articles/is-tdd-dead/)
