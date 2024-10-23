
The eigth and final fallacy of distributed computing is "the network is homogenous," originally referring to network hardware. The caution at the time was to not overlook the challenges in dealing with different physical routers, switches, servers, and the like when dispatching requests across a network.

Over time software protocols have been able to alleviate most hardware concerns for developers of modern distributed systems, so the fallacy is more often used now to caution that the _software_ dimension is heterogeneous. Changing topologies, different protocols or message formats, and evolving network configurations and policies are some examples in this dimension. As components of distributed systems are increasingly tended to by greater numbers of administrators, the degree of network heterogeneity at the software level increases.

The next dimension above this is maybe the most interesting one - architectural heterogeneity. This would encompass everything from differences in the semantics of contracts and business objects to persistence and consistency models to SLAs (service level agreements). This dimension is not frequently touched on in discussions regarding the fallacies of distributed computing, and I think that's a shame. I typically think of a distributed system being more difficult and/or complex than a monolith by a factor of 10 in _every_ aspect - 10x more surface area for bugs, 10x as many considerations to guard against, 10x as much development time required, etc. It occurs to me then that there would be 10x as much architectural burden from these systems, and the heterogeneity of the constituent parts of the system is directly contributing to that.

Having already discussed most technical aspects of the fallacies in my other articles on the matter, here I want to focus on the various dimensions in which distributed systems are heterogeneous. Just as standardized abstractions and a defensive attitude are able to stave off the complications which were initially observed stemming from hardware heterogeneity, I think we'll find the same approach will adapt well to these other dimensions.

# The Software Dimension

If we review my previous articles on distributed computing, we'll quickly find that distributed systems often do not - nay, typically do not - have homongenous [latency](https://ian.wold.guru/Posts/lateny_is_zero.html), [bandwidth](https://ian.wold.guru/Posts/bandwidth_is_infinite_ly_troublesome.html), [security](https://ian.wold.guru/Posts/the_network_is_secure.html), [topologies](https://ian.wold.guru/Posts/topology_doesnt_change.html), nor [administration](https://ian.wold.guru/Posts/there_is_one_admin.html). We've well-explored the software causes and solutions to these in the past. There are plenty of software-level concerns apart from these networking ones to explore.

## Protocols and Formats

## Configuration and Policy

## Software Stacks

It's been said quite a bit about microservices - less and less lately now that the trend is subsiding - that one of their great advantages is the ability for separate teams to use different technology stacks - even different languages - in building out their individual services. Certainly this is a _significant_ side effect of the architecture, thogh depending on what kind of organization you're working for this can be significantly good or significantly bad. The idea of watching the ripple effects across a department of 50 C# engineers slowly coming to the realization of the implications of one team having gone rogue in deploying a Clojure gRPC service is an entertaining one.

Not that I've ever observed such a phenomenon.

But here you might say: "Well Ian, this is simple! We can have some organizational coding standards, enforce C# and some common Nuget libraries, and avoid the concern!" If this is you, then it is you for whom I am writing this article, for this line of reasoning is to fall victim to one of the classic blunders: _it is this fallacy!_

Separate teams deploying separate services will diverge and have separate tech stacks. Separate tech stacks communicating with each other will have inconsistencies. Tautologies are always true. Being clear, this will always be a bug vector, but to be aware of this fact is to minimize the surface area of the vector. Do vectors have surfaces? I'm bad at geometry.

This usually crops up in small things. Do the two separate systems have the same ability to parse dates? Do the email validations follow the same rules? Do you have addresses? Do you have international addresses? Do you have a button on your site that says "I know you weren't able to parse this address but keep the one I entered"? My disdain for handling addresses aside, just in data interpretation alone there are so many points for slight divergence between different software systems - different libraries, languages, configuration defaults thereof, and so on.

Let's throw some load balancers, firewalls, and caches on top. What kinds of request policies do these expect of your client? Is the client service capable of acting in a manner friendly with this? You'd expect so, but the point is to not assume so. All of these concerns are, in theory, wrapped up into the contract negotiated between the two entities. And how many contract negotiations have happened that excluded, uh, _most_ of the things it should include? Haha, this article doesn't seem so pointless all of a sudden!

This bleeds nicely into the next dimension: if we can't trust the components of a distributed system to be architecturally homogenous with each other, can we at least trust the network itself to be architecturally homogenous?

# The Architecture Dimension

To immediately answer the above question: no, of course not!

For a long time I disregarded the eighth fallacy - the network is homogenous - as its original sense doesn't really apply as much any more. It gained a new life for me in this more abstract sense having read [David Boike's article for Particular](https://particular.net/blog/the-network-is-homogeneous) on this same fallacy. It's unfortunate that article is so short beause there's plenty more to be said regarding architectural heterogeneity.

## Semantics: Objects, Actions, and Contracts

Now you might be tempted to dismiss this as just a difference in semantics (hahaha ... please laugh) but this really does matter. In fact, semantics is a lot of what we do in modeling the business, whether we know it or not. Ensuring consistency between our semantics is paramount to ensuring a correct mapping of the business domain. Misunderstandings - even at a fine-grained semantic level, is an ever-present vector for bugs.

I currently work in ecommerce, so I can take the business object "item" to demonstrate semantic difference. Items exist, obviously, all over the place on a shop website. You can view an item, maybe customize it with some options, and you can add it to a cart or a registry. This involves at least three teams: an items team, a cart team, and a registry team. Does item mean the same thing to all these teams?

Let's say the items team is tasked with maintaining a service that stores all the items; this is the source of truth for what the "Luxury Dinner Plate" and other items are. Their service lets you query the details for this item and produces events when it becomes no longer available and the like. To the items service, that's what an item is: It has details (name, price, personalization options) and can go in and out of availability, and it only ever has a single "Luxury Dinner Plate".

But now take the cart or the registry. Those can have multiple "Luxury Dinner Plate"s in them, maybe with different personalization options. Therefore you'd expect the cart and registry services to have different identifiers for their items than the item service; an item for these other services is actually an _item in cart_ or _item in registry_. Hopefully it's clear then the importance that when these teams work together (and consequently when the services interact with each other, directly or indirectly) that everyone understands the semantic difference. For quick example, it would be insufficient for an "item shipped" event to only include the identifying item information used by the item service but exclude information required by the registry service.

That example might or might not be a recent problem I had to tackle.

This naturally extends beyond the objects and their properties to the sorts of actions taken on an object. Of course you can't _purchase_ an item stored in the item service, you can only purchase an item stored in the cart service. If you feel compelled to interject here and try to correct me with "but you can also _purchase_ an item from the registry service!" you'd be wrong! Haha, got you! You cannot, but you can _add to cart_ an item from the registry service. "Oh but come on," I can hear you objecting, "that's just a sematic difference!" _Exactly!_ And if it's left unconsidered you're going to end up with a real fun issue in production.

These two aspects - what an object are and what its actions are - are the basis of contracts between components in the system, and their semantics form part of the semantics of the contracts. I'm being very careful to not accidentally assert that they are _all_ of what the contracts are. No, unfortunately the real world is more complicated, but they're the two halves of domain modeling and consequently I think that object and action semantics are the two main bug vectors  from the contract/semantic side. The hope is that this impresses the importance of thoroughness in negotiating and defining these contracts. If you encounter a colleague accusing you of getting too granular or "semantic" about it, remember that _that's the whole point_!

## Persistence and Consistency Models

That a distributed system would have a mixture of persistence and consistency models is, I think, an absolute certainty. If you're not dealing with enough data or transactions to have to soften data requirements _somewhere_ then your application is probably a poor candidate for distribution.

Persistence models mostly affect individual services while consistency models apply across the system. Ideally components of a distributed system are isolated and one needn't know about the internal function of another (persistence being an "internal function"), though these two do affect each other; different persistence models can allow a service to support a given consistency model more or less easily. A hard persistence model such as [ACID](https://en.wikipedia.org/wiki/ACID) ensures a service can maintain data integrity and best supports a hard consistency model, while at the other end a soft soft models such as [BASE](https://www.geeksforgeeks.org/acid-model-vs-base-model-for-database/) are entirely contiguous with the softer consistency models.

ACID and BASE, when applied to database transactions, are typically only describing persistence within a discrete service - a single actor against the database. When we move up a level to consider separate pieces of related data in a system, we might find that we need to maintain data integrity across distributed systems, and we might find ourselves tempted to consider support for single transactions distributed across these components. My advice is to avoid distributed transactions at all costs; it is typically an antipattern. If you encounter the need for them this is a smell which indicates that you should either combine the components which share the related data, or you should instead rearchitect the system to accept softer consistency in that data. If you really do need distributed transactions though then use a single database which [supports such transactions](https://www.postgresql.org/docs/current/two-phase.html).

The overall trend though is that distributed systems will result in components with softer persistence models, which in turn results in softer consistency models across the system. I think it would be difficult for a distributed system to avoid having any eventually-consistent components. Considering the technical constraints which [we explored with the other fallacies](https://ian.wold.guru/Series/fallacies_distributed_computing.html), asynchronous communication is an essential tool in solving some of the more difficult technical issues. Not only does that require accepting eventual consistency, but it circles back to having to accept a soft presistence: packets might get dropped without the downstream system being any the wiser. State drift!

For an example that demonstrates these effects, consider the ecommerce system from before with the items, cart, and registry services. Suppose that to save having too many calls into the items service, we update the items service to send out an event when data about the item changes (let's say name and price), and asking consuming systems to listen to these events and record what they need themselves. It's easy to see a potential issue here: suppose an event goes out and it is received by the cart service but not the registry service: you might get a situation where a customer sees the "Luxury Dinner Plate" for $10 in a registry, adds it to their cart, and then sees it as $12 in the cart because the registry service never got the event that updated the price.

This seems like a glaring issue with event-driven systems, but the key is on data requirements. Perhaps an item price is a poor choice for such a setup, since many systems have a hard requirement to be aligned on that data. How about the name of an item though? That's probably only going to change if there's a typo; this is an excellent candidate to be updated via an asynchronous channel. We can say that item name can accept softer persistence and consistency models, while the item price does not have that luxury.

We do have other options with the item price though. While we might need to accept a very hard persistence model for the item price data, we might be able to identify some components in the system that do not need to have as hard a consistency with item price data than other components. That is to say, some components need _real-time_ item prices, while others would be satisfied with only _near-real-time_ prices. For those latter components, we might be tempted to replicate the item price data geographically so that requests to the item data can still have high throughput with minimal strain on the broader of the network. In such a scheme, the item servie would expose two separate interfaces for the real-time and near-real-time flows; the former requests would query against the main dataset (the one which is always written to from which the other replicas copy) guaranteeing real-time data, while the latter requests would query against the closer, faster replica data. Feasibly the items service could be broken out so that the item price data is managed by a separate service, considering the different architectural requirements for this data.

To bring it back around, we can see that different components of the sytem will require different persistence models, and this has a knockon effect of supporting many different (though potentially _similar_) consistency models. That becomes a cycle where consistency models will influence individual persistence models, with the asterisk in the whole cycle being that business requirements will set lines in the sand as to how far in any direction the system can move here. Components with different models will interact with each other in interesting ways, 

## SLAs and QoS

GPT suggestions:

Disparities in SLAs Across Services
Different components of a distributed system may operate under distinct SLAs. For example, one service might guarantee 99.99% uptime, while another only guarantees 99.9%, leading to potential inconsistencies in user experience or system reliability.
Discuss the impact of heterogeneous SLAs on system-wide performance, especially in critical paths where a lower-SLA service could become a bottleneck.
Strategies for mitigating this include service degradation techniques, such as graceful failover or fallback mechanisms that handle service failures more elegantly.

QoS Variations in Throughput and Latency
Some services may prioritize low-latency, high-throughput operations, while others are optimized for processing large batches of data over longer periods. These mismatches can create performance issues, especially in tightly-coupled systems where services are expected to operate in concert.
Explore the use of mechanisms like rate limiting, service throttling, or traffic shaping to manage QoS across heterogeneous services.

Handling Different Availability and Fault Tolerance Models
Services may also differ in their fault tolerance strategies (e.g., one service may have built-in redundancy and replication, while another might rely on simpler failover techniques). This can lead to uneven behavior in response to failures.
Discuss best practices for aligning fault tolerance strategies, such as employing a consistent approach to retry logic, circuit breakers, or distributed tracing for better observability across services.

# Conclusion