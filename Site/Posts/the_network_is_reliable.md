* Any network is inherently unstable
* General solution: always code for failures
* Modern networks require packets go through a lot of systems - gateway API, firewall, load balancer. Each system that receives and forwards packets is a potential point of failure.
* Calls can succeed but responses still be dropped.
* Solution: idempotency
* Udi Dahan recommends messages queues, asynchronous communication, and store-and-forward: https://particular.net/blog/the-network-is-reliable
* Problem though: Asyncronously communicating systems cannot guarantee consensus (see ACM)
* At the very least, redundancy needs to be built in so store-and-forward is best practice
* Problem here too from ACM: "Perhaps more concerning is [Microsoft's and University of Toronto's] finding that network redundancy improves median traffic by only 43%; that is, network redundancy does not eliminate common causes of network failure."
* Non idempotent operations complicate this. Issues here can be guarded against by operation ids
* Also message ordering may be necessary for non-idempotent endpoints
* Eventually consistent event sourced systems are an interesting, if heavy-handed, way around this
* Extending that example, we can say that the more reliable of a system you engineer, the more costly it is. I have no evidence for this, but it is maybe helpful to assume a sort of exponential growth in cost vs. reliability. 100% reliability can never be achieved, so there is some point of reliability which becomes cost ineffective for your firm. This is a point to stimulate conversation: at what level can we accept packet loss? Refer to ACM for figures in performing this calculation. This is where, when delivering my professional opinion, I shrug and say we should follow best practices.
* Partition-aware designs (see ACM) should be used
* Failure is only an option if you've explicitly coded for it. And that doesn't mean try { doEverything(); } catch { print("oops!"); }
* ACM: https://cacm.acm.org/magazines/2014/9/177925-the-network-is-reliable/fulltext
* Great quote from ACM: "Moreover, in this article, we have presented failure scenarios; we acknowledge it is much more difficult to demonstrate that network failures have not occurred!"

{
	"title": "The Network is Unreliable and There's Nothing You Can Do About It!",
	"description": "Indeed the network is unreliable, and this is especially concerning for modern, distributed system. The catch though is that it never can be 100% reliable, and we can't create systems that perfectly compensate for this.",
	"date": "21 February 2024",
	"contents": false,
	"hero": "photo-1451187580459-43490279c0fa",
	"series": "Fallacies of Distributed Computing",
	"related": [
		{ "title": "Book Club 9/2023: Papers I Love", "description": "Reflecting on the final Strange Loop conference, having attended several 'Papers We Love' talks, I'm motivated to share five papers I love.", "fileName": "book_club_9-2023" },
		{ "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "fileName": "deploying_aspdotnet_7_projects_with_railway" },
		{ "title": "Daily Grug", "description": "Need inspiration start day, made API.", "fileName": "daily_grug" }
	]
}

When the [Fallacies of Distributed Computing](https://en.wikipedia.org/wiki/Fallacies_of_distributed_computing) were first written in the 90s, networks were unreliable. The internet was unreliable, intranets were unreliable, even radio was sometimes spotty back then. In the last thirty years, we as an industry have taken this unreliable infrastructure and ... left it there. Packet failures, client timeouts, and the occasional solar flare continue to be a problem not because of any inadequacy on our part but because it's a flaw which is inherent in the system; no network can ever guarantee reliability. Radio is still sometimes spotty because, just like the internet, sending any information over large physical distances is always going to have interruptions and loss. The first Fallacy of Distributed Computing is to assume the opposite of this - _the network is reliable_ - and it's first for a good reason: it really matters.

Now, I wonder if in the last thirty years we haven't actually made this problem worse. In the 90s there weren't a lot of microservice systems making exorbitant use of load balancers, firewalls, gateway APIs, and the like. These are useful tools, but each new component in the distributed stack adds a point of failure. These systems can and do fail in their own right, but the incidence rate of network failures specifically will increase as more of these components are included.

It seems then that we have a good candidate for a first fix: simplify the architecture! Does every microservice need its own firewall? Do we have multiple gateway APIs? A serious and focused audit of the system architecture can reduce a lot of layers in the distributed stack, and overall improve the reliability of the system - the most simple systems tend to be more reliable. However, this only gets us to a certain point - a lot of systems still require load balancing, and you're going to need a firewall _somewhere_.

# Store and Forward (and Retry)

The simplest implementation to get some fault tolerance is to implement a retry when we see a network error after sending a request. This is [store and forward](https://en.wikipedia.org/wiki/Store_and_forward), but I prefer to call it "store/forward/retry" as these all tend to be related. Intermediate systems like gateway APIs and load balancers and the like might have simple implementations of this pattern themselves. To implement this, you'll need to "store" the message, send it ("forward"), and then you can retry on failures as needed by resending the stored message. This pattern works well because it's very simple and provides a good level of recovery from some network errors. It's best practice to implement some sort of retry system - if the packet is dropped en route from client to server, you'll want to be able to resend that.

DIAGRAM store/forward/retry pattern

## Idempotency

There is a problem here though - it is possible for our client to get a network error even if the operation did succeed on the server: suppose the response packet was dropped or the client timed out before receiving the success response. That means that we will potentially send the same request to the server more than once, leading to potential error on the server from reprocessing the same valid, successful request more than once. This is solved by ensuring the operations on the server are [idempotent](https://en.wikipedia.org/wiki/Idempotence) - that replaying the same request twice won't cause these sorts of errors. Indeed, idempotency should be a default for all operations in a distributed system.

Problem: Some operations cannot be idempotent
One solution: Operation IDs
One consideration: Message ordering

## Boilerplate and Complexity

There's another problem, potentially, with implementing store-and-forward everywhere: now I'll have a bunch of boilerplate around my code! This is a bigger problem for some codebases and less a problem for others. If your application doesn't have a lot of requests and you only need a base level of resilience, then moving the boilerplate into a shared library, or consuming an existing third party library for this, can be sufficient.

On the other hand, if you're setting up a distributed system of even a moderate size, you've probably got a fair amount of traffic going around, and it might make sense to set up a more comprehensive scheme. Some third party libraries do help out here, and allow a sharing of settings across components or provide more intricate solutions for orchestrating some of the request policies across a whole system. Still there are problems here - suppose the client application goes offline before it's able to resend its request, now I might need to add some persistence somewhere if the system requirements need that level of resiliency.

A more powerful alternative to solve this problem is a [message queue](https://en.wikipedia.org/wiki/Message_queue) (MQ). An MQ acts as a standalone message bus for a distributed system, allowing your clients to send their requests into the queue and letting the queue handle all of the considerations to make sure it gets to the client. Most of them offer robust UIs to give you a good level of insight and control over the system, and there are several options that are widely used for this purpose. Now your client needs very little logic in the way of sending a request - it just needs to make sure the request gets to the MQ.

DIAGRAM how MQs work

# More Complicated Patterns

At the very least, redundancy needs to be built in so store-and-forward is best practice
Complication from ACM: "Perhaps more concerning is [Microsoft's and University of Toronto's] finding that network redundancy improves median traffic by only 43%; that is, network redundancy does not eliminate common causes of network failure."

## Asynchronous Communication

Complication: changes from request-response model to fire-and-forget model
Solution: Asynchronous communication
Problem: Asynchronous systems can never guarantee consensus
    Therefore, list pros and cons of async communication. When to employ, when to not
Store-Forward-Forget must be considered best practice for redundancy
    Explain soft proof of this (ACM; partition-aware systems)

## Outbox

One common point raised is that as more logic is added around outbound requests, the slower it is to handle those requests. In cases where my hot path is very hot and still needs to produce a fair number of outbound requests (as you might need to if you're notifying on all data change operations), I'll want to optimize my logic as much as I can. Perhaps it will seem attractive to not provide adequate robustness around my requests to make them faster.

The obvious pattern to use here would be to shuffle your message off to a queue running in a background process that will eventually publish the message, just outside the thread the hotpath is on. This works in a lot of scenarios, but there are robustness concerns yet with this. A helpful pattern here is the [Outbox Pattern](https://microservices.io/patterns/data/transactional-outbox.html). The core concept is the same - we maintain a background process in a separate thread which handles sending messgaes with proper resiliency against the faulty network, however the enqueueing mechanism is the clever bit.

This pattern suggests that your database should have a table, or tables, containing the messages which you want to enqueue - this is the "outbox" table. When your application makes the update to the business objects in the database, in the same transaction it would add the messgae to be sent to the outbox table. The background message sending process then listens to this table (either by polling or by having the database raise events) to perform the sends. This is clever because, while you do need to write the logic to insert the message into the table, you don't specifically need to call the message publishing service to enqueue the message. On top of this, that you're using your database as the queue gets you a persisted queue for free.

DIAGRAM outbox pattern

This pattern is worthwhile if you've got a _hot_ hot path, need the extra resiliency in your queue, and the extra cost of running the background process is worth it to you.

## Event Sourcing

Some applications have a high need to preserve message ordering, usually because the state of the system is dependent on the temporal changes over the course of several events. These systems are good candidates for [event sourcing](https://www.eventstore.com/blog/what-is-event-sourcing), and this pattern can help us alleviate some of the pain of hardening our system against a faulty network.

This pattern imposes (very broadly) that you should save all of the events which alter your state, and that the state should subsequently be _derived_ from these events. This is opposed to our traditional way of persisting data, where we process an event, update the state to reflect the changes specified by that event, and then forget the event. Event sourcing allows a number of benefits like being able to replay state, but what's interesting to us is that it allows inserting an event _in the middle_ of a set of events which have already been processed.

DIAGRAM event sourcing

This is beneficial to us if message ordering is high on our considerations list. If we're implementing proper resiliency when messages are dropped on the wire, we're going to be retrying messages, and there's a fair chance we're going to be sending some messages out of order in this scenario. As long as our events are properly dated, they can be ordered appropriately (and change the state appropriately) in our eventually-consistent system. This pattern also has the power to transform some non-idempotent operations into idempotent ones - instead of changing the state directly in a non-idempotent way, we'd be inserting/upserting/updating the single event in an idempotent way.

One word of warning though - event sourcing is a huge pain to implement and maintain. This pattern can very quickly get very complicated, and if you're careless then you can mangle data over time. Systems like [EventStoreDB](https://www.eventstore.com/) or the [postgresql-event-sourcing plugin](https://github.com/eugene-khyst/postgresql-event-sourcing) can help to make this easier, but that of course requires an investment in those systems. This is a pattern to study carefully and only use if it's appropriate for your use case.

# Chosing the Right Solution

Trend: solutions have drammatic rippling effect on infrastructure.

## Cost-Benefit

Extending that example, we can say that the more reliable of a system you engineer, the more costly it is. I have no evidence for this, but it is maybe helpful to assume a sort of exponential growth in cost vs. reliability. 100% reliability can never be achieved, so there is some point of reliability which becomes cost ineffective for your firm. This is a point to stimulate conversation: at what level can we accept packet loss? Refer to ACM for figures in performing this calculation. This is where, when delivering my professional opinion, I shrug and say we should follow best practices.
DIAGRAM COST-BENEFIT, DIMINISHING RETURNS

## Tradeoffs

Explain tradeoffs when choosing the level of reliability, redundancy, etc. in these systems.
DIAGRAM ASYMPTOTE - NO SYSTEM 100% ERLIABLE
Explain that these must be explicitly coded for
Failure is only an option if you've explicitly coded for it. And that doesn't mean try { doEverything(); } catch { print("oops!"); }

## How Do We Know Our Solutions Work?

How do we know we've engineered a system which is appropriately reliable given our domain?
Great quote from ACM: "Moreover, in this article, we have presented failure scenarios; we acknowledge it is much more difficult to demonstrate that network failures have not occurred!"
Explain some notes about metrics collecting. This is more the "art" side than the "science" side.

# Conclusion

TBD
