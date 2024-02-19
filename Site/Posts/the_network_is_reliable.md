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

When the [Fallacies of Distributed Computing](https://en.wikipedia.org/wiki/Fallacies_of_distributed_computing) were first written in the 90s, networks were unreliable. The internet was unreliable, intranets were unreliable, even radio was sometimes spotty back then. In the last thirty years, we as an industry have taken this unreliable infrastructure and ... left it there. Packet failures, client timeouts, and the occasional solar flare continue to be a problem not because of any inadequacy on our part but because it's a flaw which is inherent in the system; no network can ever guarantee reliability. That's the first fallacy - _The network is reliable_ - and it's first for a good reason: it really matters.

Now, I wonder if in the last thirty years we haven't actually made this problem worse. In the 90s there weren't a lot of microservice systems making exorbitant use of load balancers, firewalls, gateway APIs, and the like. These are useful tools, but each new component in the distributed stack adds a point of failure. These systems can and do fail in their own right, but the incidence rate of network failures specifically will increase as more of these components are included.

It seems then that we have a good candidate for a first fix: simplify the architecture! Does every microservice need its own firewall? Do we have multiple gateway APIs? A serious and focused audit of the system architecture can reduce a lot of layers in the distributed stack, and overall improve the reliability of the system - the most simple systems tend to be more reliable. However, this only gets us to a certain point - a lot of systems still require load balancing, and you're going to need a firewall _somewhere_.



Explain the store-forward-retry pattern: store the request, send it, retry on failure until success
Explain why this works well
DIAGRAM

Problem: It is possible for the operation to succeed on the server machine but for the response back to our client to be dropped.
Explain idempotency and how it solves the above problem

Problem: I have to write (and maintain) a bunch of store-forward-retry code.
One solution: library to do this. List pros and cons
Other solution: MQs
Explain MQs, how they work
DIAGRAM
Complication: changes from request-response model to fire-and-forget model
Solution: Asynchronous communication
Problem: Asynchronous systems can never guarantee consensus
    Therefore, list pros and cons of async communication. When to employ, when to not
Store-Forward-Forget must be considered best practice for redundancy
    Explain soft proof of this (ACM; partition-aware systems)
Complication from ACM: "Perhaps more concerning is [Microsoft's and University of Toronto's] finding that network redundancy improves median traffic by only 43%; that is, network redundancy does not eliminate common causes of network failure."

Problem: Some operations cannot be idempotent
One solution: Operation IDs
One consideration: Message ordering

One solution for all of the above: Eentually consistent event sourced systems
    Pros, cons. When to employ and not.
DIAGRAM?

Trend: solutions have drammatic rippling effect on infrastructure.
Extending that example, we can say that the more reliable of a system you engineer, the more costly it is. I have no evidence for this, but it is maybe helpful to assume a sort of exponential growth in cost vs. reliability. 100% reliability can never be achieved, so there is some point of reliability which becomes cost ineffective for your firm. This is a point to stimulate conversation: at what level can we accept packet loss? Refer to ACM for figures in performing this calculation. This is where, when delivering my professional opinion, I shrug and say we should follow best practices.
DIAGRAM COST-BENEFIT, DIMINISHING RETURNS

Explain tradeoffs when choosing the level of reliability, redundancy, etc. in these systems.
DIAGRAM ASYMPTOTE - NO SYSTEM 100% ERLIABLE
Explain that these must be explicitly coded for
Failure is only an option if you've explicitly coded for it. And that doesn't mean try { doEverything(); } catch { print("oops!"); }

How do we know we've engineered a system which is appropriately reliable given our domain?
Great quote from ACM: "Moreover, in this article, we have presented failure scenarios; we acknowledge it is much more difficult to demonstrate that network failures have not occurred!"
Explain some notes about metrics collecting. This is more the "art" side than the "science" side. 