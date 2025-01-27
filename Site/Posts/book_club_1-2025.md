;;;
{
	"title": "Book Club 1/25: Results, Railways, and Decisions",
	"description": "A story on a successful result and insights gained from some more heady research this month.",
	"date": "27 January 2025",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Projects", "Testing", "Learning", "Patterns"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 12/2024: Team Metrics and KPIs", "description": "Allow neither ignorange nor complacency to abuse metrics or KPIs; these are important tools for the competent professional.", "fileName": "book_club_12-2024" },
		{ "title": "Book Club 11/2024: No", "description": "On forming quality opinions and saying 'no'", "fileName": "book_club_11-2024" },
		{ "title": "Book Club 10/2024: Fallacies of Distributed Computing", "description": "Putting a pin in the series I wrote this year on the topic of these Fallacies", "fileName": "book_club_10-2024" }
	]
}
;;;

I've written before on my enthusiasm for the result pattern, but I'm going to expand on that a bit here and how it relates to testing. I've spent a fair chunk of my past work-month standing up a new distributed service at work, and it's such a simple project as to almost have completely written itself, except for one opportunity I took: I wanted to see how many ways I could approach black-box testing the API. If you know me, you know I'm a big fan of writing automated tests ... in _one_ way - I really _really_ like integration-level tests, and even then just covering successful vs failing input and checking state change is robust enough for the vast majority of our systems. I usually do this and then move on to actually implementing the darn thing, and that's perfectly fine. I'm poorly-versed in other methods, and so that's what I've been reading this past month.

I promise that the result and railway design patterns _do_ fit into my black-box testing ideas here; let me take a step back to motivate that. Last month, I spoke with a number of folks about the requirements of this API. It's only got a few endpoints, but there was some complexity around external systems it would need to consult - _who_ has _what_ data - so there were a number of models of the API developed before I coded anything. Goodness knows there are dozens of ways to model systems, but these (advantageously) took the form of control flow diagrams. Here's a simplified version of one for an endpoint to get a resource, which attempts to create the resource if it doesn't yet exist:

1. Get the resource from the database
    * If it exists, go to step 4
    * If it does not exist, proceed
2. Consult a cache holding some data about the resource
    * If there is no data in the cache, respond not found
    * If there is data in the cache, proceed
3. Create the resource in the database with the data from the cache
    * If this fails, respond unexpected error
    * If this succeeds, proceed
4. Consult _another_ cache to get more data about the resource
    * If this succeeds, merge the datem and respond successfully
    * If this fails, respond with the partial content from the database

This is a real treat to have! For all purposes, each step represents a different result-returning method - and the results are right there, in easy-to-read markdown no less! Not only does this neatly model my results, it also neatly models the railway of the system. If you're new to [the railway concept](https://fsharpforfunandprofit.com/posts/recipe-part2/), it's quite straightforward. Almost all of our business logic can be thought of as a railway in that it has two tracks. There's the success tack and the error track. The logic progresses through several steps along the success track, but any step might be unsuccessful and divert the control flow onto the other track. My model above is a bit more complicated because of the branching control flow and there being multiple success and failure paths, but this demonstrates the utility of this model: it keeps the extra complication straightforward and comprehensible. If I were to draw a railway diagram I might have (again, simplified):

![Railway diagram for GET endpoint](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/book-club-1-25-railway.png)

This might be a stylistic preference of mine, but there are two things at the top of my mind when I develop business logic. The first is what the railway of the logic looks like - I encounter _incredibly_ little business logic that is not made more clear by railwaying. The second thing at the top of my mind are the results of the operations that create decision points where I might switch tracks. Those two things wholly constitute the structure of most any business logic: what tracks I have and the events that move between them. The business logic queues operations in order; those operations _operate_ and report result; the business logic decides which results, if any, change tracks; repeat. I hope I can motivate my enthusiasm at being able to have these models as an output of my engagement with the stakeholders in the system - the code really does darn-near write itself.

_Side note: "railway" is more of a way of thinking, not really a pattern and not really an orientation. Maybe I could say "the rail...way of thinking!" Please laugh._

This brings me to testing, and a hypothesis: I figure that just as these models were able to prefigure the structure of the system's code, so too they would prefigure its tests. This seems somewhat obvious: the model is a graph and by traversing each possible path through the graph I can generate all of the test scenarios necessary to cover all of the branches in control flow and ensure proper responses. For the graph I've given you it's quite easy to do this manually, however larger graphs whose nodes have more results (e.g. different types of successes or failures) will become unwieldy. This is the main thrust of my research this month, with which I [hacked together a tool](https://github.com/IanWold/StateFusion) to generate test scenarios for me from these shared models.

Owing to the large number of different ways we can diagram our systems, there are an equally large number of ways to generate test scenarios from these models. A collection of knowledge on this sort of practice can be found under the term [model-based testing](https://en.wikipedia.org/wiki/Model-based_testing). This sort of approach certainly has limits though. In my case the models were a product of the _kind_ of collaboration I ended up doing for this project, and surely the utility of the specific testing approach I chose here is also influenced by the architecture of the broader system and the culture at my firm. Indeed, the approach might have to be quite different (or not used at all) on other projects.

It is a very interesting consideration though, along with the rest of the resources available for test generation. I'm quite contented by the results of my exploration here; it's given me a more robust and flexible framework to approach testing!

**My related writing:**

* [Roll Your Own C# Results](https://ian.wold.guru/Posts/roll_your_own_csharp_results.html)
* [Testing Logging in ASP.NET Core](https://ian.wold.guru/Posts/testing_logging_in_asp_net_core.html)
* [It's Okay to be a Bit Techy in Your Gherkin](https://ian.wold.guru/Posts/its_okay_to_be_a_bit_techy_in_your_gherkin.html)
* [Guerrila DevEx Testing](https://ian.wold.guru/Posts/guerrila_devex_testing.html)
* [Book Club 10/2023: Functional Patterns in C#](https://ian.wold.guru/Posts/book_club_10-2023.html)
* [Book Club 2/2024: Recovering from TDD and Unit Tests](https://ian.wold.guru/Posts/book_club-2-2024.html)

**On results and railways:**

* [Functional Error Handling in .NET With the Result Pattern - Milan Jovanović](https://www.milanjovanovic.tech/blog/functional-error-handling-in-dotnet-with-the-result-pattern)
* [Is the result pattern worth it? - Andrew Lock](https://andrewlock.net/working-with-the-result-pattern-part-4-is-the-result-pattern-worth-it/)
* [The Operation Result Pattern - Gary Woodfine](https://threenine.blog/posts/operation-result-pattern)

A final note on the result pattern (on which I should probably elaborate in a future article) is its utility lies in modeling _expected_ failure cases which have associated business logic. This is a distinction between bug, error, failure, or the other sorts of paths the code might take.

* [Software Testing: Defect, Bug, Error, and Failure - Rahmat Ullah Orakzai](https://www.baeldung.com/cs/software-testing-defect-bug-error-and-failure) gives a good overview of this
* [Bug, Fault, Error, or Weakness: Demystifying Software Security Vulnerabilities - Irena Bojanova and Carlos Eduardo C. Galhardo](https://tsapps.nist.gov/publication/get_pdf.cfm?pub_id=936191)
* [The Error Model - Joe Duffy](https://joeduffyblog.com/2016/02/07/the-error-model/) gives my favorite explanation and prescribed solutions here

I don't know of a better overview of the railway concept than that from Scott Wlaschin. He's also written a bit on property testing:

* [Railway oriented programming](https://fsharpforfunandprofit.com/posts/recipe-part2/)
* [The "Property Based Testing" series](https://fsharpforfunandprofit.com/series/property-based-testing/)

**On testing**

* [Geeks for Geeks](https://www.geeksforgeeks.org/model-based-testing-in-software-testing/) gives an acceptable overview of concepts
* [Control Flow Software Testing - Geeks for Geeks](https://www.geeksforgeeks.org/control-flow-software-testing/)
* [Introduction to Model-Based Testing by TorXakis](https://torxakis.org/userdocs/stable/mbt/mbt.html) begins a large repository of knowledge on the topic
* [Automated Combinatorial Test Methods – Beyond Pairwise Testing - D. Richard Kuhn, Dr. Raghu Kacker, and Dr. Yu Lei](https://tsapps.nist.gov/publication/get_pdf.cfm?pub_id=152162)
* [Combinatorial Methods for Trust and Assurance - NIST](https://csrc.nist.gov/projects/automated-combinatorial-testing-for-software)