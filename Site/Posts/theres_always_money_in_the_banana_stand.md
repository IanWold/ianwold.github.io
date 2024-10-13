;;;
{
	"title": "There's Always Money in the Banana Stand",
	"description": "Except the 'banana stand' is the transport layer and instead of saving the money for later you're just always setting it on fire.",
	"date": "30 August 2024",
	"contents": false,
	"hero": "photo-1451187580459-43490279c0fa",
    "topics": ["Distribution", "Architecture"],
	"series": "Fallacies of Distributed Computing",
	"related": [
        { "title": "There Are Infinite Administrators", "description": "Yes, infinite, and they're inventing more each day! The larger the system, the greater the problem that nobody really knows how it all works.", "fileName": "there_is_one_admin" },
        { "title": "The Topologies They Are a-Changin'", "description": "Okay, dumb title, but could you really have done better? Shifting topologies have always presented problems for distributed computing, and modern infrastructure systems sometimes leave us worse off than ever before.", "fileName": "topology_doesnt_change" },
        { "title": "There's More to Network Security than the Network", "description": "Assuming a secure network in a distributed system loses sight of all the ways vulnerabilities can creep into our systems. Just as distributed computing makes our systems 10x more complex, the same effect is felt on security.", "fileName": "the_network_is_secure" }
	]
}
;;;

The penultimate fallacy of distributed computing is "transport cost is zero." Anyone who has stood up their own test project on a cloud provider can intuit the fallacy here: dispatching any message across a transport layer requires more processing and uses network resources, each of which cost runtime and ultimately money for our application. In the context of a distributed system these costs can end up being huge, making it important to not just avoid the fallacy but to always be cognizant of the costs associated with any transport.

Naturally there are plenty of options to mitigate this issue, as I've explored on previous posts in this series. If (de) serialization is causing poor performance at the application level, [MessagePack](https://msgpack.org/) or [ProtoBuf](https://protobuf.dev/) can save you a lot of resources - [LinkedIn reduced latency by 60% with ProtoBuf](https://www.infoq.com/news/2023/07/linkedin-protocol-buffers-restli/). Keeping inter-service communication on a local network and geolocating friendly services will further reduce latency and isolate bandwidth concerns.

I think the 80/20 rule applies here - 80% of the achievable cost reduction will take 20% of the effort. That's not to say that it's easy work, but that it's _relatively_ easy. How easy do you think it would be to switch your production distributed system to ProtoBuf? Now imagine how much work there is in the other 80%! I take this to indicate that there's a practical plateau as far as cost saving is concerned - at a certain point the juice is no longer worth the squeeze. That said, even if you were to realize _all_ the potential cost savings in a distributed system, you _are still_ distributing, which is inherently more costly than not distributing.

With everything optimized, you still incur a baseline cost in latency and network resource consumption with the message. On either end of that message, you incur a serialization cost. You incur a cost from the systems required to guarantee your required level of resiliency and fault tolerance (retry logic, load balancers, etc), and you incur a cost from your security systems (firewall, auth, etc).

In a way, each of the previous fallacies has preempted this fallacy, so there's not a lot to say here that isn't review. The key lesson here is that if you are distributing, then you are spending more money, period. Depending on your domain you might be spending a lot more money than if you weren't distributing. There are plenty of problems with monolithic systems, and while some of those problems are only solvable with distribution, most of the common problems that arise in these systems are just regular problems. Distribution isn't the magic wand to unspaghetti your bad codebase.

Looking more broadly than the costs associated in just transporting one message, you will incur costs in the telemetry, logging, monitoring, and alerting required to properly maintain a distributed system, even if your observability requirements aren't terribly deep. You'll incur costs with the engineering resoruces required to develop these systems, the operations resources required to deploy and maintain these systems, and the organizational costs required to develop and enforce the procedures that arise from owning these systems.

These fallacies demonstrate that all of these costs are _necessary_ for distributed systems, and they reinforce the big leap in effort to create and maintain these systems. Taken all together, I see the fallacies all pointing to one glaringly obvious conclusion: unless the problem you're trying to solve cannot be solved but by distributing, then don't distribute. Maybe you've got a problem-ridden legacy monolith and you're looking at a large expenditure involved in refactoring. Microservices look attractive in this situation, but beware of underestimating the proper costs associated with the microservices transformation. Remember [Grug on Microservices](https://grugbrain.dev/#grug-on-microservices):

> grug wonder why big brain take hardest problem, factoring system correctly, and introduce network call too
>
> seem very confusing to grug
