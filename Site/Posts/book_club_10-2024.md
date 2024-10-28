;;;
{
	"title": "Book Club 10/2024: Fallacies of Distributed Computing",
	"description": "Putting a pin in the series I wrote this year on the topic of these Fallacies",
	"date": "28 October 2024",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Blogging"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 9/2024: Blogroll", "description": "I've been dying to ask you; I really want to know: where do you get your ideas from?", "fileName": "book_club_9-2024" },
		{ "title": "Book Club 8/2024: Labor", "description": "Using Labor Day as an excuse to wonder about some recent trends in the industry", "fileName": "book_club_8-2024" },
		{ "title": "Book Club 6&7/2024: Postgres", "description": "I have become further radicalized to the opinion that we should all just be using PostgreSQL", "fileName": "book_club_6-7-2024" }
	]
}
;;;

Happy Halloween! Boo! I'm celebrating now for two reasons. First, the weather is very nice where I'm at; I prefer wearing more layers and Fall is the perfect month for that. Second, I just finished my [series on the fallacies of distributed computing](https://ian.wold.guru/Series/fallacies_distributed_computing.html) - perhaps the world didn't really need it but I was here to provide it.

Much earlier this year I figured that writing this would be a good way to exercise the writey muscles and allow me to express some thoughts on a topic I surely have some thoughts on. I think I achieved _those_ goals, though perhaps such endeavors in the future could focus on being more interesting. This is a relatively dry topic as it stands, and I don't know that I put a lot of creativity into it. Nonetheless, while this is a frequently-commented-on topic, I feel that I was able to go more in-depth on a number of points which are lacking in other writing.

In particular, a lot of writing on this subject doesn't emphasize the difficulty of distributing. It's not just non-trivial; I'm firmly committed to the position that a distributed system has 10x as many _things_ as monolithic systems - 10x the number of bugs, 10x the amount of development effort, 10x the number of meetings (both to plan and to figure out what's going on), 10x the maintenance cost, and so on. To me, this is the implication of the fallacies: there's a ton of work you can't forget to do. Why would anyone ever distribute their system?

Of course, there are problems that are unsolvable but by distribution. It's the only option if you need to independently scale some component, which perhaps _any_ system at a sufficient size will need to do. Before we can ask how to distribute a system, we need to ask whether we should. Once we've identified a need for a discrete component to be distributed, then we need to go about the _how_. That's going to be different in different cases, and the fallacies do a great job, in my mind, of guiding our conversation around that question. I didn't prescribe a lot of "do this and that" in my series because I don't see that as the point. It's the mindset that's the point, the architectural mindset: how do I consider all the tradeoffs there are?

Being clear, there are major tradeoffs. You can't solve for all of the potential issues. Look back at latency and bandwidth: these two are related and trying to reduce one will (almost certainly) increase the other, unless we can consume more resources. We can only consume more resources to a certain point, so what gives? Well, nothing gives. You're going to be vulnerable somewhere, and you can choose where you want that. This is, of course, the point to reiterate that the going advice ought be that we shouldn't distribute in the first place unless it's absolutely necessary.

So the best _default_ we have in doing development is to create a well-architected monolith. One with really good principles and domain segregation. Yeah that's tough, but you're not decreasing _anything_ by distributing - especially if you look to it as your savior. With load there'll come components which require distribution, and we separate those out at this point. This architectural prescription will (probably) not ever result in a microservices system. Rather, we'll end up with a very dapper trunk-and-branch architecture, where we have a monolithic trunk surrounded by individually-distributed branches. You might have a "leaf" layer of common services, and that's it! Keep the possible paths through the system shallow. Easy monitoring, easy debugging, you can keep everything under control.

The distribution-skeptical monolith-to-trunk-and-branch engineer will typically architect a more resilient and adaptable system than the distribution-zealous microservices engineer. I wonder if coming years will be characterized by an equal zeal for the "modular monolith", which would end up being - to my estimation - hilarious. Ours is an industry continuously held hostage by trends.

Today I'll share a bunch of related links, particularly ones which came up in research for this series. It's helpful to see what others are writing, both to get a sense of the area and to avoid repeating what others have offered. Particular Software, which maintains the popular NServiceBus, keeps a blog that's worth reading. Naturally they also have a series on the Fallacies, and while sparse it's a good resource. I was particularly influenced by their [final post on the eighth fallacy](https://particular.net/blog/the-network-is-homogeneous), which enlightened me to the idea that the eighth fallacy can look at every aspect of heterogeneity between system components.

Particular is run by Udi Dahan, who has given a number of talks on distributed computing in the .NET world, and [has a series focusing on the fallacies](https://www.youtube.com/watch?v=8fRzZtJ_SLk&list=PL1DZqeVwRLnD3EjyciYAO82dT9Owiq8I5).

Other writing on the fallacies:

* [Fallacies of Distributed Computing Explained - Arnon Rotem-Gal-Oz](https://pages.cs.wisc.edu/~zuyu/files/fallacies.pdf), a paper from the University of Wisconsin that seems to be frequently shared.
* [The Fallacies of Distributed Computing — Latency is Zero - Ryan French](https://ryan-french.medium.com/the-fallacies-of-distributed-computing-latency-is-zero-14a02a73f43a) seems to be part of an unfinished series.
* [The Network Is Reliable - Peter Bailis and Kyle Kingsbury](https://cacm.acm.org/practice/the-network-is-reliable/)
* [A Guide to Managing the First Fallacy of Distributed Computing - Anadi Misra](https://dzone.com/articles/a-guide-to-managing-the-first-fallacy-of-distribut)
* [What are the main Cloud Design Patterns? - Milan Milanović](https://newsletter.techworld-with-milan.com/p/what-are-the-main-cloud-design-patterns)

I think Renegade Otter's article [Death by a Thousand Microservices](https://renegadeotter.com/2023/09/10/death-by-a-thousand-microservices.html) is one of those articles we'll still be sharing in ten years. The ugliness created by the zealous pursuit of distribution is described well here. My favorite observation from that article is that Shopify is a Rails monolith (_presumably_ a trunk with branches) and has a revenue in the several billions.

Finally, [Mark Richards](https://developertoarchitect.com/) is an architect who has made a fair career out of being good at explaining fundamental concepts in architecture. Two recent talks of his which I think are particularly good and pertinent to this topic are [Elements of Distributed Architecture](https://www.youtube.com/watch?v=1kESDzfEaxo) and [Decomposition Patterns](https://www.youtube.com/watch?v=wiWjX9yaXTY).