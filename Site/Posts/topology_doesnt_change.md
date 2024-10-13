;;;
{
	"title": "The Topologies They Are a-Changin'",
	"description": "Okay, dumb title, but could you really have done better? Shifting topologies have always presented problems for distributed computing, and modern infrastructure systems sometimes leave us worse off than ever before.",
	"date": "19 June 2024",
	"contents": true,
	"hero": "photo-1451187580459-43490279c0fa",
    "topics": ["Distribution", "Architecture"],
	"series": "Fallacies of Distributed Computing",
	"related": [
        { "title": "There's More to Network Security than the Network", "description": "Assuming a secure network in a distributed system loses sight of all the ways vulnerabilities can creep into our systems. Just as distributed computing makes our systems 10x more complex, the same effect is felt on security.", "fileName": "the_network_is_secure" },
        { "title": "Bandwidth is Infinite ... ly Troublesome", "description": "The bandwidth of the world-wide web has increased dramatically, but so has its demand. There's an abolute limit to how much data we can all transmit, and working around that requires dilligence.", "fileName": "bandwidth_is_infinite_ly_troublesome" },
        { "title": "Latency is Zero and the Speed of Light is Getting Faster", "description": "Latency is a constant and unavoidable fact of nature, but we can plan for it, work around it, and respond to it.", "fileName": "latency_is_zero" }
	]
}
;;;

Some of my earliest memories are from stories that my grandfather used to tell. He had an interesting career and knew a lot of story-able folks, so like a lot of grandfathers he had a compendium of fantastic stories. One of my favorites was a time he taught a course to his colleagues at his work. He was an engineer at Honneywell and was required to give this lecture in spite of his less-than-adequate abilities as a teacher, and it happened to be on an unfamiliar subject. After this lecture his colleagues were asked to fill out a short review form for him; the punchline of the story was that my grandfather's favorite review was "Leigh took a boring subject and ... left it there!" This was greatly amusing to him.

The topic on which he was required to lecture was, of course being germane to this article, topology. _His_ was a lecture on the subject in mathematics, so I'm hoping that network topology is a more interesting subject for us here, but be warned that I will take some delight if you leave a comment about this being a boring subject! I do not think so - network topology consists of the actual layout of all of the components and connections between them within a network, which is an area with which I think we all have hands-on experience.

The fifth fallacy of distributed computing is "topology never changes", the implication being that it changes constantly. Twenty years ago when these fallacies were written that was a concern, and I will suggest that it's a significantly higher concern today. Not only do we tend to build systems with more distributed components and more connections than we did twenty years ago, but nowadays we have _managed_ systems, we throw load balancers everywhere, and I'm not sure any of us knows _everything_ that cloud providers do behind the scenes. Heck, each time a client connects or disconnects that's a change in the topology, and sometimes it's not a trivial one.

Our topologies change constantly and mostly due to automated processes. Thankfully, this is an area where practice has mostly kept up with the times, and the industry actually seems to be relatively good at writing systems which are resistant, to some degree, to changes in topology. Cloud providers assign names to services by default, modern application frameworks make some best practices the default, and so on. This is an excellent start but we oughtn't allow ourselves to be lulled into a false sense of security here. Just as with the other fallacies, errors induced by topology can cause some wild behaviors in our systems.

# Problems from Topology

Topology changes can be the cause of any of the problems covered in [the other fallacies](https://ian.wold.guru/Series/fallacies_distributed_computing.html). Changing the location or address of a service can prevent its dependents from being able to contact it, adding ride-along services can induce latency, closely locating services can surface bandwidth issues, and so on for each of the issues we have (and will) discuss. To some extent then, applying the lessons learned from our exploration of the other fallacies will guard us against the problems we can encounter from topology changes.

Were the list of fallacies just a list of sources for errors from distributed computing, then including topology changes might well be redundant. Instead of being _that_ list though, the fallacies are a matter of mindset - a list of human assumptions we ought keep ourselves from making. By adopting the assumption that the topology _will_ change, and frequently, we can eliminate this category of network instabilities from affecting our systems.

## Services

Obviously, a service failing and removing itself from the topology can have knockon effects if there are other services which depend on it; this creates a [network instability](https://ian.wold.guru/Posts/the_network_is_reliable.html) and can be protected against with a number of patterns. Equally concerning though is when services add themselves into the topology.

These situations can cause balancing errors, sometimes localized to the area of the service itself, sometimes with ripple effects across the whole network. The configurations of the load balancers take on a greater importance here, even small inconsistencies can cause a service to underperform. At scale this can introduce issues related to latency and bandwidth.

Even just when the services _change_, say when an update is rolled out, should not be discounted as a change to the topology - while the new version occupies the same node as the previous version and fulfills the same contract, but it's not the same as the topology before the update. This makes these update times sensitive; the team should monitor the system at the time of the update, and it would also be best to retain with your historical logs when updates are pushed out. The more subtle a problem, the further down the line it's likely to be caught.
 
## Connections

Services can be connected with each other in various ways. If a service needs to dispatch a call to another, that's an obvious connection. If one service consumes events which another is responsible for producing, that's another more abstracted connection. These connections can chain into very complicated dependency chains, and that complexity is a source of problems itself. If the complexity grows to the point that it can't be understood, or if appropriate telemetry isn't set up to give you insight into the system, those introduce human problems. Human problems are the most difficult to solve.

There's a physical aspect to the connections as well - services which are physically, geographically more separated will have more connection issues. Even in a perfect world without packet drops, those communications will be slower. [Software-defined networking](https://en.wikipedia.org/wiki/Software-defined_networking) approaches can really help, depending on the scope of the system, and this introduces the same sort of configuration issue I mentioned with the load balancers. These systems can also become very much affected by service updates or other triggers that might cause the dependency graph to change - either by calling different systems or by routing more or less traffic over a particular connection.

This is where it's important to have a proper understanding of the conceptual topology; you might not be able to predict all the ramifications of a change, but at least you can get some good insights into what you might need to test. This is not just useful for large changes; if your topology is well-understood, this sort of analysis can become part of the working process.

# Decouple Services from Topology

_The_ way to prevent changes in topology from inducing these errors in the system is to decouple the system from the topology. This is almost a question-begging argument: "Disconnecting the system from the topology disconnects the topology from the system!" However, the system can't be disconnected from the topology, not entirely, as the topology arises from the system itself. The statement rather is to say that the system should be written with abstractions over the pieces which interact with the topology such that a specific topology is not required for any component to work correctly.

Maybe the most obvious example of decoupling services from the specifics of the topology is IP and DNS. I'm sure you would give me quite a quizical look if I were to suggest that send requests from one service to another using the target service's IP address instead of its domain name; the IP address of the service will almost certainly change, so we'd definitely break our connection if we did that! We use DNS to escape this problem. Well, others too, but we're focused on topology. Instead of having to rely on a specific about the topology (IP), we use an abstraction (DNS) that hides underlying changes from us.

Just as we have an expectation that services' IP addresses _will_ change, we should expect that _all_ aspects of the topology can change. I'm almost always a fan of not introducing abstractions until a codebase or system reveals a pattern that is clearly asking for an abstraction, but this is not a case for that. Distributed computing increases the complexity of our systems tenfold (at least), and a significant contributing factor to that is the _necessity_ of a host of abstractions. 

## Abstract the Message from its Delivery

This seems simple conceptually but ends up being more interesting in practice. The idea is that messages dispatched across a network have a _message_ that's being sent and some _address_ it's being sent to; these should never be coupled with each other. That seems simple - we'll just throw the URL of the target service (ServiceA) in the configuration of the service making the request (ServiceB)!

That's a start - suppose we include a `ServiceABaseUrl` config in ServiceB, then when ServiceB needs to send a request it can send it thus:

```csharp
result = httpHandler.SendRequest(
    url: $"{serviceAConfig.BaseUrl}/some/endpoint",
    request: packet
);
```

Oh wait, ServiceB is coupled to the _endpoint_ of ServiceA now! In some very simple scenarios this could well be enough, but we would probably be wise to be more resilient. Sure, we could throw all of the individual endpoints into the configs as well, and that gives us more flexibility. But there's still a couple problems. Less importantly, we're going to have tons of config values. More importantly, even with the values of ServiceA's address(es) in configs in ServiceB, the latter is _still_ coupled to specific addresses about the former. We'll need to involve another party in order to properly decouple these.

A configuration management system might be a good next step given our hypothetical ServiceB -> ServiceA topology. This would be a third party in our architecture, distinct from ServiceA or ServiceB, which would manage the configuration values across the system, or across parts of the system you care about. Instead of having ServiceB's configurations specified in a configuration file or some other scheme deployed with the service, they'd be in this distributed component. Not only would ServiceB dynamically access these values at runtime, but the configurations in the management system can be dynamically updated at runtime.

Thus, when the location of ServiceA changes, the configuration in the management system would be updated to point to the new location at ServiceA. This can get quite fancy if you also introduce a "downtime" config, specifying whether ServiceA is available or not, as it might be down momentarily while changing locations. While this gives us the proper level of abstraction, it has some negative effects.

By introducing this distributed component, ServiceB will have to dispatch a network request each time it needs a config value. Sure, we could do a cache about that, but now ServiceB has to go through the whole rigamaroll of handling that. Caches are difficult and we want to avoid them. This ties in with the other problem with using a configuration management system in this way - it adds a fair bit of complexity, needing multiple config values potentially in order to satisfy the abstraction requirements we have.

Fortunately, this idea describes the widely-known pattern _service discovery_, and there are dedicated systems which can support this. Employing one of these systems is essentially the same pattern as above but with the complex logic already built in. If your use case(s) are appropriately small, using your own configuration management with your own logic on top might be the right solution, otherwise I'd encourage you to look into dedicated service discovery systems.

You might notice a flaw or two with this pattern though. Each dependent service will need to maintain its own logic to connect to and understand the service discovery system; more microservices in the network will result in more updates to that state being required; the dependent services still need to dispatch requests in order to understand addresses; and so on. In short, if I have a complex topology, then this pattern may be insufficient.

The next step up would be a _service mesh_ to provide an independent component dedicated to managing the traffic across the network, rather than just managing address configurations. Service mesh systems like [Envou](https://www.envoyproxy.io/) or [Istio](https://istio.io/) will have you deploy a _sidecar_ alongside each service to handle inbound and outboud communication. These are centrally coordinated through a control plane to route requests correctly (as well as security, telemetry, and whatnot). This allows each service to minimize the amount of configuration required to dispatch its requests.

## Async Communication

It might seem quite dramatic to deploy a sidecar application along with _every_ service in a system, and indeed it's more resource-intensive and complicated on the infrastructure side. In practice it isn't terribly burdensome from an engineering standpoint, and it's not terribly complicated for the infrastructure team once they get it up and going. However, I sympathize with the inclination to want to avoid this if possible. Less is more.

I've touched on the concept of asyncronous communication in [my other posts on the fallacies](https://ian.wold.guru/Series/fallacies_distributed_computing.html), but to quickly recap, this pattern can essentially be thought of as _reacting to events_ instead of _sending requests to get a response_. In our previous example, ServiceB needed some resource from ServiceA so it dispatched a request to get that resource. In an asynchronous context, ServiceA would be publishing events notifying the system about updates to that resource, which ServiceB could then consume in order to maintain its own state for this resource.

From a data normalization mindset this can seem like an antipattern, but in a distributed context it's often quite beneficial, as it allows an ultimate abstraction between services (particularly within the context we're discussing related to topology) while minimizing processing and communication overhead. Not all communication can be made asynchronous, but adopting asyncronous communication largely obviates the need for the more complicated patterns discussed earlier to handle request-response communication.

## Self-Healing Architecture

This is a key concept running a line through the considerations I've made so far. _Self-healing architectures_ are, as their name implies, architectures which are designed to automate fault recovery as much as possible. For example, _service discovery_ and _service meshes_ provide tolerance by adjusting the routing through a system in the event of a failure at one point. Just as our services can be coupled to the topology when they dispatch requests onto the network, they can also be coupled to the topology when they encounter errors from it. Thus, implementing a self-healing architecture is a necessity in ensuring a proper decoupling between the services and topology.

Implementing proper deployment strategies with automated rollbacks are quite beneficial at the service level, and these are facilitated by the patterns discussed earlier. [Blue-green deployments](https://en.wikipedia.org/wiki/Blue%E2%80%93green_deployment), whereby a new version of a service is installed alongside the old version, allows requests to be rolled onto the new version and rolled off if necessary. This can be done intelligently in a [canary release](https://martinfowler.com/bliki/CanaryRelease.html) strategy for more control. Using appropriate health check or other monitoring tools, these can be entirely automated.

Looking at a bit of a higher level, some of our self-healing patterns will themselves alter the topology, and so need to be carefully considered in the whole context so as to not introduce the errors we've discussed. Auto-scaling systems and load balancers will provide isolation across services and communication. The [circuit breaker pattern](https://en.wikipedia.org/wiki/Circuit_breaker_design_pattern), also discussed previously, is widely employed to isolate failures by breaking the connection to failed services.

Each of these, combined with rollback strategies at the service level, provide an excellent resilience for the system. Their necessity also demonstrates the central point that the _topology is always changing_, giving us a strange feedback loop where the topology has to change in order to react, in part, to the effects of a changing topology. This cascade is an interesting subject for study!

# Communication and Monitoring

I've discussed communication, monitoring, and _people process_ at the end of each of my posts on the fallacies, and I'm curious whether I've said enough or whether this discussion should take up half of each fallacy. Indeed, our processes are as important as the software we develop. We're never going to be able to engineer distributed systems which are perfectly resilient to the constantly-changing environments they run in, so errors are going to happen. _People_ who implement a _process_ involving both proactive and reactive _communication_ with each other (very difficult, I know) and _monitoring_ the production systems are the glue that holds it all together.

This is very important then. Proactively, when manual changes are made to the topology these should be communicated in a way that all stakeholders can efficiently be made to know about them and have an opportunity to react if necessary. Most changes in topology are automatically done nowadays, I think, so it occurs to me that manual changes are probably quite serious or impactful. All the more reason to communicate.

But that the vast majority of topology changes are minor and done without human intervention mean that we can only react to unhandled errors that crop up as a result. Where possible, systems which do these sorts of alterations should log what they do and when, allowing us to use our normal log monitoring tools to be able to correlate errors we observe with these updates.

[Chaos engineering](https://principlesofchaos.org/) is, for a lot of engineers I know, the most exciting related process - probably more so for the destructive aspect. I don't know that employing this strategy is right for most organizations though, and this testing strategy really benefits larger distributed systems. In order to effectively deploy it, not only do you need excellent communication and collaboration, but the folks running the tests need to be quite diligent and competent.

# Conclusion

The saving grace is that topology errors will impact more complicated systems over time. Sure, that can probably be said about all the fallacies, but I think this is the one where that primarily shows through. Given my simple example topology of ServiceB -> ServiceA, the topology isn't going to change that much and simple strategies will resolve problems there. However, that network still requires that we set up proper security, and network failues and latency still impact the user experience.

I tried to outline, particularly with request-response messages, a way to be able to ramp up onto more elaborate strategies to deal with topology abstraction. If you're developing a new system or distributing a legacy system, you might not need to worry about these issues as much up front, but you certainly need to plan for worrying about them. Architect your components in a way that they can be updated with these considerations in future, and ensure you have proper monitoring and processes in place to start with.

I think that network topology - understanding it and coding around it - is one of the more interesting aspects in distributed systems. I think I share that opinion with several colleagues, but maybe not most. If you find this subject boring generally, leave a comment as to whether I was able to convey a sense of interest or whether I left the subject there, so to speak!