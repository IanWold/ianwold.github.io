;;;
{
	"title": "Book Club 3/2024: Simplicity",
	"description": "Everything is too complicated.",
	"date": "25 March 2024",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Processes", "Architecture"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 2/2024: Recovering from TDD and Unit Tests", "description": "TDD and unit tests are overused and often misprescribed. What do we really hope to gain from our tests, and what testing practices support our goals?", "fileName": "book_club_2-2024" },
		{ "title": "Book Club 1/2024: What is a Software Architect?", "description": "A few meandering and maybe unhelpful thoughts on the title \"Software Architect\"", "fileName": "book_club_1-2024" },
		{ "title": "Book Club 12/2023: Workflow, Process, and Agile", "description": "Some thoughts on how to organize software development and teams, and how non-technical factors help (or hinder) us developing better software.", "fileName": "book_club_12-2023" }
	]
}
;;;

Everything is too complicated. Not just in software, everything! This isn't a novel concept, Teddy Roosevelt would have us believe that anything worth doing involves difficulty. I think this is a widely-adopted notion; simplicity in software seems to be a whole genre of software blogs. That makes my job here of sharing links quite easy though!

To spare all of the usual writing, simplicity is software is valuable because it _makes all the things better_. Simple code is easier to read! It's easier to maintain! Your code will perform 10x faster if it's _simple_! Simple code is more extensible, testable, and debuggable! Then we might consider the software itself - the UI, UX, and all the user-considerations. Simple software is easier to use! Users are less likely to reach an unintended state with simple software! You'll have fewer support calls and lower burden of feature growth with simple software!

These are all true, but a trap a lot of us fall into is neglecting complexity. Sure, our software _could_ be extremely simple, but it has to solve real user requirements. This is the main source of complexity. Sometimes a very simple piece of software can solve most requirements of most users mostly well. Sometimes a more complicated piece of software can solve those requirements _very_ well. We can't let ourselves want software more simple than the use case requires. I could just as easily rattle off plenty of pro-complexity platitudes: Complexity is necessary for real-world scenarios! Complexity is required to adequately address security and performance concerns! Architectures that maximize extension and innovation are necessarily complex!

Both simplicity and complexity are necessary. The difference here is that the necessity of complexity always comes with that asterisk - the _kind_ and _level_ of complexity that is required is the minimal amount required to satisfy the requirements you have for your system. Any complexity too far beyond that is detrimental; all of the listed benefits of simplicity are, in fact, true.

Understanding requirements is essential then. User requirements only capture so much. What are the environmental requirements? What quality attributes does the code need to support? These can introduce necessary complexity but often go overlooked. A complete understanding of the requirements of the code will allow you to develop a holistic view of the complexity that you need to introduce. This is a sort of _focused_ complexity - this is not just complexity that is _necessary_ but that is also _understood_; you know why it's there. Unfocused complexity is surely just as bad as excessive complexity. No doubt we all have codebases that have the wrong _level_ of complexity, but ask also whether they have the wrong _kind_ of complexity. Maybe indeed the level of complexity is right but it's unfocused.

**Watching**

* [Large-Scale Architecture: The Unreasonable Effectiveness of Simplicity - Randy Shoup (2022)](https://www.youtube.com/watch?v=oejXFgvAwTA)
* [Managing Complexity in Software - Hadi Hariri and Kevlin Henney (2022)](https://www.youtube.com/watch?v=P7CfWtR-ECk)
* [Software Architecture, Team Topologies and Complexity Science - James Lewis (2022)](https://www.youtube.com/watch?v=uAwJEFLJunk)
* [Highly Cohesive Software Design to tame Complexity - CodeOpinion](https://www.youtube.com/watch?v=r0-GC3Y_OME)

And surely [Jonathan Blow](https://www.youtube.com/watch?v=4oky64qN5WI) has something to say on the topic.

**Reading**

* [Learn Code Complexity: Understanding Code Complexity with Ease - Mohit Trivedi](https://devdynamics.ai/blog/learn-code-complexity-understanding-code-complexity-with-ease/)
* [Why embracing complexity is the real challenge in software today - Ken Mugrage](https://www.thoughtworks.com/insights/blog/technology-strategy/why-embracing-complexity-real-challenge-software-today)
* [Why Simple is So Complex - Joe Crick](https://itnext.io/why-simple-is-so-complex-362bc835b763)
* [Software Complexity: Essential, Accidental and Incidental - Alex Bunardzic](https://dev.to/alexbunardzic/software-complexity-essential-accidental-and-incidental-3i4d)
* [What Is Complexity? - Alex Kondov](https://alexkondov.com/what-is-complexity/)
* [Simplifying Software Development - Dick Wall](https://www.developer.com/guides/simplifying-software-development/)

These two are kind of related:

* [Clean your codebase with basic information theory - Taylor Troesh](https://taylor.town/compress-code)
* [Metric-centered and technology-independent architectural views for software comprehension - Luis F. Mendivelso, Kelly Garcés, and Rubby Casallas](https://jserd.springeropen.com/articles/10.1186/s40411-018-0060-6)

**From Me**

I've had a couple articles recently that more or less relate to "simplicity":

* [Just Use PostgreSQL](https://ian.wold.guru/Posts/just_use_postgresql.html)
* [It's Better to be Consistently Incorrect than Inconsistently Correct](https://ian.wold.guru/Posts/its_better_to_be_consistently_incorrect_than_consistently_correct.html)
* [Eight Maxims](https://ian.wold.guru/Posts/eight_maxims.html)