;;;
{
	"title": "Latency is Zero and the Speed of Light is Getting Faster",
	"description": "Latency is a constant and unavoidable fact of nature, but we can plan for it, work around it, and respond to it.",
	"date": "29 March 2024",
	"contents": true,
	"hero": "photo-1451187580459-43490279c0fa",
	"series": "Fallacies of Distributed Computing",
	"related": [
		{ "title": "The Network is Unreliable and Reliability is Scary", "description": "Indeed the network is unreliable, and this is especially concerning for modern, distributed system. The catch though is that it never can be 100% reliable, and we can't create systems that perfectly compensate for this.", "fileName": "the_network_is_reliable" },
		{ "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "fileName": "deploying_aspdotnet_7_projects_with_railway" },
		{ "title": "Daily Grug", "description": "Need inspiration start day, made API.", "fileName": "daily_grug" }
	]
}
;;;

Did you know that the speed of light is getting faster? It is! 10 years ago it was 10 mm per clock cycle. 5 years ago it was 5 mm per clock cycle. Today it's 3 mm per clock cycle! This is good news to those of us attempting to solve latency issues in distributed systems because the speed of light is a natural barrier that cannot be overcome; this imposes a limit on all travel, including _information travel_, so the universe dictates a natural level of latency in all of our applications. A distributed system at scale will be particularly affected by this. "Latency is Zero" is the second Fallacy of Distributed Computing because of this - we can't ignore latency or assume it away because it is _always_ a baseline effect on _every_ operation.

David Boike, writing for Particular, [gives a succinct definition of Latency](https://particular.net/blog/latency-is-zero) in terms of transmission time, size, and bandwidth: `TotalTime = Latency + (Size / Bandwidth)`. Latency is an additional cost incurred by _every_ operation. Even local, CPU-bound operations still need to travel over a short wire within the client machine to resolve. This is low latency. A request from a client to a server is going to incur a _significantly_ higher amount of latency - this must be accounted for. In a distributed system, every single inter-service operation incurs this latency. That makes this a big problem at scale.

# Resolving Latency We Control

Unlike [network reliability issues](https://ian.wold.guru/Posts/the_network_is_reliable.html), the latency issue actually can be mostly overcome. Well, in theory it can; in practice probably not. And it depends on what kind of latency you care about. And only within the confines of your server components. Do you have total control over all of the latency in your system, and have you designed your system in such a way that no component is temporally dependent on another? I'm not sure it's possible for any real-world system to match that, but no doubt this is an area where theory can inform practice.

## Maximize Utility-per-Call

Latency occurs on each operation we perform, but it's most egregious - and most impactful to us - on network calls. If we have more network calls we have more latency, and less latency with fewer calls. This doesn't eliminate latency itself from being a problem, but it lessens the effects.

Consider a microservices system where a gateway API might call into an orders service, which itself needs to call into an items service to get item information to return with an order, which itself might need to call into an availability service to return that data with the item. Assuming a baseline 50ms latency per network call (sometimes this is a lot more) our total latency in a single call is 200ms from the client. If we could shave two calls off that reduces our total latency to 100ms. Keep in mind though latency is usually more than 50ms!

Eliminating calls works to a point, obviously we still need to make network calls at some point. When we do make network calls, we want to make sure we _need_ to make that call, and we need to make sure they're returning exactly the data we need. On top of this, sometimes calls can be combined - if we observe a pattern where service A regularly makes two calls to service B, that's an indication that service B should make a new endpoint available that condenses the data from both calls. This halves the latency incurred between the two services.

Remember that maximizing call utility does not mean "cram as much crap as possible in each request". Bandwidth still matters, and each temporally-coupling call must be absolutely necessary.

An important pattern to consider in maximizing the utility of each request is database connection pooling. Opening a new database connection can be costly, so this pattern suggests that we should maintain a "pool" of open database connections that can be reused for different requests. For applications which frequently interact with their database, this technique can significantly reduce latency in this specific scenario. This is especially important since _most_ services will maintain some form of persistence, so database interactions are a key target to improve metrics in this respect.

## Asynchronous Communication and Eventual Consistency

As I've covered before [in my series on the Fallacies](https://ian.wold.guru/Series/fallacies_distributed_computing.html), asynchronous communication can help resolve a lot of network issues. To review, "synchronous" communication is when we need to communicate between distributed components in real-time to satisfy a request. When a system need to receive a response to a request before it can proceed with its task, it is said to have a temporal coupling here.

With respect to latency, these temporal couplings are a primary concern. Moving to a pattern of asynchronous communication - where communication between two distributed components is _not_ required to satisfy a request - removes latency as a concern with respect to satisfying these requests. Asynchronous communication is achieved by having the components of the system publish and subscribe to events on state change. Instead of requiring Service A to request a resource from Service B when it needs it, we would rather have Service B publish an event whenever its resources are modified and to have Service A listen to these events and update its own persistence with the information it requires from that resource.

To be clear, this shifts where the latency is in our system. When I make a request from Service A, I will _not_ experience the latency that would otherwise occur if that call was temporally coupled to Service B. However, there is an observable period of time between when Service B's resource is updated and when Service A is able to reflect that update. This resource is said to be eventually consistent between the two services; there are periods of time where they have different understandings of the resource. Sometimes this is OK; other times not.

If you have a system which is 100% eventually consistent and communicating entirely asynchronously, then you've eliminated request latency as a concern. But I'm betting your client still needs to contact its server, and I'm betting you'll need a temporally-coupled call here or there.

To be clear also, this strategy introduces its own concerns. You will end up with inconsistent data, for example - how do you resolve this? _Can_ you?

## Beware the Cache Demon

Caching is a tempting solution for a lot of network issues since it eliminates the need to make network calls under some circumstances. This is the catch though, our caches need to be particularly aware of what those circumstances are. When to cache, what to cache, and when to clear what from the cache are difficult questions, and misapplying a cache can lead to confusing and difficult errors. As the saying goes, there's only two hard things in Computer Science: naming things, cache invalidation, and off-by-one errors. Cache invalidation is difficult enough that I do a double take whenever the word is used in a meeting.

Understanding cache patterns will help to determine where, if anywhere, to introduce one. A caution though, fewer caches is always better - if you're able to avoid them entirely that might be just as well so as to eliminate a potential headache. Ryan French has an excellent overview of some related cache patterns on [his article on this Fallacy](https://ryan-french.medium.com/the-fallacies-of-distributed-computing-latency-is-zero-14a02a73f43a).

# External Factors

Truly though, we don't have control over all of our latency. Most systems of an appropriate size or utility need to connect to some external services. Cloud providers are a primary culprit here since most of our server computing uses them nowadays. Are you using AWS lambdas or Azure's identity provider? You might own the application logic within the Lambda or the data within the ID provider, but that's wrapped in their own system; you're interfacing with a component that you don't necessarily control.

So while you can't control all of your latency, you still do get to control your network topology. By carefully designing and managing the configuration and traffic patterns between the system components you can "box in" and reduce reliance on potentially volatile areas. I'd suggest that understanding network topology is more essential for identifying and addressing latency issues within distributed environments.

## Topology Management

I considered naming this section "alphabet soup" - there's lots of patterns with three-letter acronyms in this domain. This can be its own article entirely, and topology will be related to the other Fallacies as well, so I'll just touch on a couple of things to consider for further research.

[Software-defined networking (SDN)](https://en.wikipedia.org/wiki/Software-defined_networking) is the commonly-cited approach here; this suggests that software should be employed in defining the network topology and intelligently routing communication across the network. By allowing software to intelligently control network traffic, your system can more intelligently react to various network conditions. This is overkill if you've got a client-server setup, but necessary if you're operating at any scale.

It's a good idea to couple this with [network function virtualization (NFV)](https://en.wikipedia.org/wiki/Network_function_virtualization), which suggests that various vital components of the network can be run on virtualized environments to allow for better management. Resources like firewalls and load balancers that are necessary across the entire system can be scaled horizontally as needed. If all of your components are operating in one or several container clusers, then this is probably already done for you.

Both of these strategies allow us to use existing systems that can react to latency in the network and aids the system at scale. They don't eliminate latency as a problem, they mitigate the effect of the problem when it happens.

## Geography

The physical distance between server and client is a fundamental factor affecting latency—data packets don't travel instantaneously. This isn't just theory; it's a practical concern that affects the responsiveness of distributed systems on a global scale. A request sent from Minneapolis to St. Paul is naturally going to complete faster than one sent from New York to Hong Kong, mostly because the data has less distance to travel but also because there is much more bandwidth between Minneapolis and St. Paul - the wires between the two cities support more traffic than they need, while the wires between New York and Hong Kong are strained with much more traffic.

We might be attracted to [edge computing](https://en.wikipedia.org/wiki/Edge_computing) to alleviate this, and intelligently-applied this can be quite good. We do want to keep resources geographically close to their consumers. This is quite the double-edged sword though. Moving too many processes to the edge can decentralize system management to a problematic degree, leading to difficulties in maintaining consistency, security, and operational oversight. There's a balance to be struck between leveraging the edge to improve latency and ensuring that the system remains manageable and secure. [Theo-T3 has an interesting perspective](https://www.youtube.com/watch?v=ze3uhkC4534) moving away from edge computing recently.

## External Systems

In my mind, the most concerning latency issues is that we (probably) can't control all of our own sources of latency. As I mentioned, you're almost certainly communicating with an external system, and if you're not then you're probably on a cloud provider that's giving you some form of abstraction.

Suppose one of these systems changes their network topology, or some other information. Suppose _that_ system moves its Kubernetes cluster from a fast cloud provider to a slow one. You've suddenly got more latency in your request.

To mitigate this, it might be necessary to adopt more aggressive strategies towards these external services. Implementing timeout policies, circuit breakers, or message queues (_ahem_ did somebody say "Enterprise Service Bus"?) can safeguard your system against the unpredictability of external dependencies. These measures give you some control back, but there's no approach that keeps your system perfectly safe.

# Catching Latency When it Happens

Because we can have latency thrust upon our poor systems, part of our job is to be able to react to it when it happens. We can proactively guard against latency enough to be reasonably sure that we probably aren't going to hurt ourselves, but we need a reactive posture against that latency we don't control. This means testing and monitoring, of course.

While the specific, technical considerations in setting up monitoring are out of the scope of this article, it's really not difficult nowadays to set up a basic, sufficient level of latency monitoring - there are tools which can watch and log the times of network calls, and these systems can usually be configured to ping you (or, ideally, someone else who gets paid more than you) if latency spikes at 2 AM. You might not need to react immediately in these cases, but it's vital here for the team owning a distributed system to set up a monitoring feedback loop, and maybe to include performance targets (latency being key here) as one of their performance indicators.

## Here we Load Test

Because of the relationship between bandwidth and latency (they're defined in terms of each other) issues with the latter are going to arise in environments with issues with the former. That means that latency issues become more apparent as bandwidth issues do, and that means we need to understand the system under load.

Indeed, load testing systems can capture problems in all the areas we've discussed, giving important insights. We're bound to have bottlenecks _somewhere_ in our systems, so the task is to identify where those bottlenecks are and whether they're beyond the acceptable parameters of the system. Use incremental load testing to ramp up and identify these areas with more precision. Having a solid grasp of the topology of your network will allow you to design precise load tests that can simulate load across particular geographical or logical areas.

# Conclusion

As with each of the Fallacies - heck, the whole point - is that they need to be kept in mind designing distributed systems. As far as we're concerned about latency, understand what latency you can control and what you can't. Understand your network topology and use monitoring to your advantage to be reactive where you need to be.

Alas, maybe this is all for naught. After all, the speed of light is getting faster!
