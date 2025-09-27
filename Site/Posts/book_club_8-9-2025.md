;;;
{
	"title": "Book Club 6&7/2025: Async",
	"description": "Not the keyword, but asynchronous communication. It just seemed that 'Async' as a title was more interesting.",
	"date": "27 September 2025",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Patterns", "Standards", "Architecture", "Distribution"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 6&7/2025: OOP", "description": "Just some blabbering about OOP, paradigms, and being non-dogmatic", "fileName": "book_club_6-7-2025" },
		{ "title": "Book Club 4&5/2025: Incidents and Resiliency", "description": "Thinking more about responding to and preventing incidents", "fileName": "book_club_4-5-2025" },
		{ "title": "Book Club 2&3/2025: Slack: Communication, Organization, Teams", "description": "On various aspects of teamwork, communication, and the like", "fileName": "book_club_2-3-2025" }
	]
}
;;;

These last two months, the interesting thing that I'm researching is asynchronous communication patterns. Last year I wrote a whole [series on the Fallacies of Distributed Computing](https://ian.wold.guru/Series/fallacies_distributed_computing.html) exploring some common dos and don'ts for systems that rely on network communication. Asynchronous communication (which, I'm just going to type "async" here on out) is a common solution to reach for as the number of nodes in the distributed network increases. It solves the problem of huge latency in requests that need to chain through several services, though at the expense of system complexity and comprehension.

In an ideal world, systems would be quite "flat", with user-level requests being able to be serviced in only a single network call to the server. Plenty of systems can't accommodate this for one reason or another, a backend might need to consult other services to fulfill a request; here though, plenty of systems can be designed so that _those_ bottom-level services don't need to make blocking calls to each other to fulfill their own requirements. Commonly, there might be a _backend-for-fronend_ that owns all of the synchronous calls, making it easier to manage temporal dependencies by isolating them all to a particular area.

Suppose the lower-level services _do_ need to exchange data? Alternately, suppose we end up with too many temporal dependencies, resulting in long-running calls even though we've made them architecturally understandable within a single layer? That's where we should be inclined to reach for async. I'm supposing here that we're imagining a relatively well-designed system; that the call patterns are well-considered to fulfill actual business needs.

How does async work? At the top level, when transformations happen to any data or state in the system, the service doing the changing emits an event through a shared bus, which is then read by systems that care about that transformation. Those systems will store their own representations of the data they care about, and are free to service requests on the "hot path" without having to introduce blocking calls. This has several implications: data is duplicated in many points throughout the system, the data is eventually consistent, the code for each service becomes more complex, considerations need to be made to prevent data drift, and plenty more there.

Given the set of problems solved by async and the implications of adopting it, there are many patterns and ideas around them. I haven't been terribly organized or disciplined in this research; this is a grab-bag of things that have caught my eye these last two months:

* [Architectural approaches for messaging in multitenant solutions - Microsoft](https://learn.microsoft.com/en-us/azure/architecture/guide/multitenant/approaches/messaging) Surprisingly fine overview of patterns.
* [Lessons in Asynchronous Messaging: Patterns, Pitfalls, and Best Practices - Jared Hatfield](https://medium.com/@jaredhatfield/lessons-in-asynchronous-messaging-patterns-pitfalls-and-best-practices-35254b3218e8)
* [Microservices and the First Law of Distributed Objects - Martin Fowler](https://www.martinfowler.com/articles/distributed-objects-microservices.html)
* [The Notifier Pattern for Applications That Use Postgres - brandur](https://brandur.org/notifier)
* [Asynchronous Everything - Joe Duffy](https://joeduffyblog.com/2015/11/19/asynchronous-everything/)
* [Asynchronous, High-Performance Login for Web Farms - Udi Dahan](https://www.infoq.com/articles/async-high-perf-login-web-farms/)
* [Orchestrating Resilience Building Modern Asynchronous Systems - Sai Pragna Etikyala and Vikranth Etikyala](https://www.infoq.com/articles/orchestrating-resilience-modern-asynchronous-systems/)
* [Dropbox’s Asynchronous Platform Evolution: from Challenges to a Unified Messaging System Model - Aditya Kulkarni](https://www.infoq.com/news/2025/02/dropbox-messaging-system-model/)

Kind of related:

* [Service Oriented Ambiguity - Martin Fowler](https://martinfowler.com/bliki/ServiceOrientedAmbiguity.html)
* [Services By Lifecycle - Michael T. Nygard](https://www.michaelnygard.com/blog/2018/01/services-by-lifecycle/)
* [SOA at 3.5 Million Transactions Per Hour - Michael T. Nygard](https://www.michaelnygard.com/blog/2008/05/soa-at-3.5-million-transactions-per-hour/)

Watching:

* [Better CQRS through asynchronous user interaction patterns - Udi Dahan](https://particular.net/videos/cqrs-user-interaction-patterns)
* [No REST - Architecting Real-Time Bulk Async APIs - Michael Uzquiano](https://www.infoq.com/presentations/rest-evolution-async-operations)
* [Distributed Systems Theory for Practical Engineers - Alvaro Videla](https://www.infoq.com/presentations/distributed-systems-theory/)
* [Loosely coupled orchestration with messaging - Udi Dahan](https://www.youtube.com/watch?v=FPBkz24QkZI)
* [Microservices communication patterns, messaging basics, RabbitMQ | Messaging in distributed systems - DevMentors](https://www.youtube.com/watch?v=eW4JgrkwWEM)

This has all taken me down a rabbit hole about [AsyncAPI](https://www.asyncapi.com/), but that may be for another time...