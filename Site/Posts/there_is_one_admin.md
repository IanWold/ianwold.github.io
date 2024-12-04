;;;
{
	"title": "There Are Infinite Administrators",
	"description": "Yes, infinite, and they're inventing more each day! The larger the system, the greater the problem that nobody really knows how it all works.",
	"date": "23 August 2024",
	"contents": true,
	"hero": "photo-1451187580459-43490279c0fa",
    "topics": ["Distribution", "Processes", "Architecture"],
	"series": "Fallacies of Distributed Computing",
	"related": [
        { "title": "The Topologies They Are a-Changin'", "description": "Okay, dumb title, but could you really have done better? Shifting topologies have always presented problems for distributed computing, and modern infrastructure systems sometimes leave us worse off than ever before.", "fileName": "topology_doesnt_change" },
        { "title": "There's More to Network Security than the Network", "description": "Assuming a secure network in a distributed system loses sight of all the ways vulnerabilities can creep into our systems. Just as distributed computing makes our systems 10x more complex, the same effect is felt on security.", "fileName": "the_network_is_secure" },
        { "title": "Bandwidth is Infinite ... ly Troublesome", "description": "The bandwidth of the world-wide web has increased dramatically, but so has its demand. There's an abolute limit to how much data we can all transmit, and working around that requires dilligence.", "fileName": "bandwidth_is_infinite_ly_troublesome" }
	]
}
;;;

In my recent exploration of the eight Fallacies of Distributed Computing, the first five have all regarded, almost exclusively, the technical aspects of distributed systems. However, as is the case for every software project, the human aspect is equally important. Each project is a combination of technical knowledge and humans - intelligent and flawed - capable of wielding it. This fallacy however - that there is one administrator - is almost entirely about the people aspect, and I'm very excited about that.

One of the things I find fun about producing small, personal projects (like [FreePlanningPoker.io](https://freeplanningpoker.io/)) is that I can know everything about the system. I know exactly how to build, deploy, and monitor the system, through the code, configuration, and cloud environment. I am capable of being the _one administrator_ for this system.

The systems we're generally paid to develop, however, are rarely as simple as our small, personal projects. Indeed, even our personal projects can grow to the point that we don't know everything about them! That's generally the point where my velocity on them drops off a cliff, but that's another matter. What I mean to get at though is that our professional projects tend to be the size or complexity (often both) that there can't possibly be one administrator - one person (not one DevOps team, not one manager, but one individual human) who knows everything about running the system.

On the whole, it's not even advisable to only have one administrator even if it's possible for a given system. People come and go from their positions or firms with some frequency, or sometimes they just forget things; knowledge and experience needs to be distributed throughout the firm. As projects grow in size and complexity it's generally more efficient anyway to distribute the administration across the firm. And not only does our company have multiple administrators, the administrators aren't the only administrators! Engineers are administrators, managers are, BAs are. External companies can be, too, and that idea becomes very interesting considering cloud providers might introduce a _lot_ of potential administrators.

The problem of course is that each administrator has a different understanding of the system, and sometimes these understandings conflict with each other. Each administrator's understanding of the system changes over time, and to successfully keep our system going they all need to interact with each other, with their overlapping knowledge. These administrators aren't just tasked with observing the system to diagnose problems, but they also need the ability to change the system in various ways. This introduces the issue that they can - and will - change our system architecture _on the fly_! *Ahem* [topology doesn't change](https://ian.wold.guru/Posts/topology_doesnt_change.html) *ahem*

So our systems need to be open to change from various sources, and resilient enough to keep running if these changes aren't, shall we say, properly thought out. They can't become reliant on huge configuration files though, as we lose the ability to teach others about the workings of the system. Finally, they do need to have clear and high quality observability so that folks who might not be terribly knowledgeable about their workings can diagnose problems. This is particularly key in our distributed environment, where problems are frequently the result of many complex interactions between various components.

# Service Design

Aside from some specific considerations regarding configurability, this fallacy doesn't inform our service design more than the other fallacies do, but it does serve to reinforce many of the same learnings we've taken from the others. It's another pin in the hat to reinforce our understanding that distributed systems are _difficult_ and do genuinely require that much extra care.

## Decoupling and Isolation

One of the primary themes (for lack of a better word) around developing distributed systems, and consequently my writing on these fallacies, is isolation. This is a double-edged sword which too easily looks like a cudgel to wield against any of the destabilizing factors which affect distributed services. On one hand, after doing the difficult task of identifying the unstable areas of a system, it's very easy and obvious to say that this is the part we need to code defensively against.

Indeed, there should be some such defense. The trouble comes in that each layer of defense is its own layer of complexity. Never forget [evil demon spirit complexity](https://grugbrain.dev/). Isolating and decoupling services are thus better suited as guiding principles when approaching architectural decisions, with the goal being to find the simplest architecture, most parsimonious with the domain requirements, maximizing te degree of isolation without growing the architecture. This is a major reason that message queues come up so frequently in discussions regarding distributed systems; I think it's fair to say they _can_ provide the most intelligent sort of isolation.

This is all with respect to the sometimes _interesting_ effects which administrators can have on the system. In short, administrators can be the direct causes of the problems we explored with the other fallacies, so in a sense we are providing some degree of protection against the crazy and whacky admins by following best practices there.

Admins can (and will) [change around the network topology](https://ian.wold.guru/Posts/topology_doesnt_change.html), [alter security management and configuration](https://ian.wold.guru/Posts/the_network_is_secure.html), and tinker with any number of properties (particularly with respect to cloud providers) that can cause any number of [network unreliabilities](https://ian.wold.guru/Posts/the_network_is_reliable.html). Those articles which I linked are focused on _guarding against_ these negative properties of distributed systems, however the outlook is slightly different when we're considering this from the administration perspective. Admins _need_ to have the ability to change these things, even though they _can't_ know all of the deleterious ripple effects a change might have throughout the system. This means that the systems we create need to go the extra step to _enable_ these changes to happen to the system.

## Configuration and Observability

I had tried writing about these two under separate headers so as to explain them in more detail, but as far as the administrators are concerned they go hand-in-hand. The configurability of the system allows the administrators to be able to adapt the system in response to new errors, a change in the environment, or whatever they might need to do. Typically, it's the observability of the system that is able to give them the insight that something changed, there's a problem, and points to where the fix needs to go. Sure, often we do need to reengineer parts of our system's components in response to errors we observe. However, as we've discussed at length on this series on the fallacies, a lot of system errors come in through the environment and configurations, and configuration _can_ be the solution - if we let it.

Both the observability of the whole system and the configuration of each individual component needs to be _obvious_. Remember - there is no one individual with everything in their head, nor can we count on everyone's knowledge being up-to-date. As engineers, we tend to talk a lot about self-documenting code, but remember that my observability graphs and the system configurations should also strive to achieve the same level of clarity!

Clarity being the most important factor in designing this aspect of our system, it should also be designed to fit its _purpose_ - observability should be strictly focused on identifying _what_ issues are happening, _why_ they are happening, and _where_ they are happening. A report on the throughput of the entire system is useless, as while it might identify that throughput drops too low it cannot identify where the issue is. Nonspecific reports shouldn't be included, and each report needs to be able to be correlated - easily - to activity and system components.

Then too there's a potential problem in moving from an identified issue to a configuration fix in a particular area of the system. Configurations need to be designed with a particular mind for _who_ might be updating these configurations and _why_. Just the other week I was attempting to diagnose an issue in a microservices architecture and identified that I wanted to attempt increasing the timeout policy for outbound messages from a microservice, only to open up its config file and find a separate timeout configuration for _each_ outbound request, each named for the method in its code which made the outbound request. Needless to say, I increased every one seeing as I wasn't going to spend a day reading its source.

## DevOps and Deployment

Sometimes I take this for granted (and you might too) but it's worth writing out: Continuous integration and continuous delivery (CI/CD) is one of the most important practices for _any_ software system, distributed or not. These practices emphasize that engineers should _continuously_ integrate their working code with the main codebase, and that the software should be _continuously_ delivered as updates are made to it.

To me, the gold standard for components of a distributed system is to deploy automatically when _any_ code is integrated into the main branch. This requires a fair amount to be in place in order to be effectively supported. First and most obviously, you do need a build gate guarding the main branch, and it's incredibly unwise to not include a comprehensive suite of automated tests along with that. You do need some kind of review process so that a single engineer can't yeet whatever they please into prod, and both the review and build processes need to be fast - like, measured in minutes fast. Finally, you _must_ have appropriate monitoring and a people process around reacting to it.

With all of these in place, I _can_ get features out quickly, but importantly I can get a bugfix out quickly. At my current firm, I've been able to have prod issues discovered, triaged, coded, reviewed, and deployed in 20 minutes. 

These practices serve to reinforce robustness and redundancy in the system not by giving us a way to roll back, but a way to roll forward. I notice that having more components - particularly distributed components - in a system makes it more and more difficult for any one component to be able to roll back in the case of a failure. These system components are all connected by (sometimes not entirely explicitly defined) contracts that cause a high level of complexity in any system, well-maintained or not. Rolling forward is sometimes - a lot of the time even - the only realistic option.

# Process Design

If the technical considerations I listed above seem a bit surface-level and handwaving at my previous writing, I think you're getting the same feeling that I do. As I mentioned before, this fallacy really concerns the human aspect of software development more than the technical aspect - that there is more than one _human_ administrator is a recognition that impacts how we design the _human_ administration of the system.

To reiterate, these human administrators need to be able to jump into parts of a system at any time and understand what it is doing, if something is going wrong, and the options available to them if something is going wrong. Then they need to be able to manipulate the system (as much as they can reasonably be afforded) without needing to engage engineers to change the code. Indeed, these administrators might well be engineers too, but when facing a high-impact incident it's really nice to be able to click a few buttons to get customers back up and going.

Each human administrator is going to have a different understanding of the system. Some will have a greater familiarity with larger parts of the system, but it's incredibly rare that anyone knows everything about the system. To complicate it, our memories aren't as solid as we typically like to think. Each person's knowledge of the system will change over time, and that doesn't just mean that they will eventually become more and more familiar with the system. On the contrary, they will forget certain aspects or old knowledge might become stale as components or the environment updates over time. Yet, each one needs to be able to administer the system, particularly if you're not sure which of them might be able to respond to the 2 AM P1 on Christmas.

## Knowledge Sharing

This is maybe the most crucial thing to integrate into our processes at all levels - it's not just for administrators! This is the only tool you have to [improve your bus factor](https://lananovikova.tech/posts/bus-factor/), and the only tool you have to increase the creativity and maximize the contribution from your whole team. Continuously knowledge sharing is the boyscout juice of productivity.

But today we're talking about administrators, so today we'll share knowledge about them. Adopting a continuous, diligent practice of sharing knowledge will help administrators understand _how to understand_ the system, the options they have for manipulating it, and staying abreast of changes and updates.

I have no evidence for this claim, but it seems to me that the majority of teams and firms have a limited approach to knowledge sharing, and I wonder if this would be one of those cases where if we polled the teams on how they felt that they would feel that they do a better job of it than they actually do. I mean to say that there tends to be a bit of complacency here - if Bob really knows a particular API, it's just _easier_ to only go to him to address problems. Over time everyone might just start calling it "Bob's API", and over even more time everyone absolves themselves of needing to know anything about it - Bob always takes care of it! Bob's API has an unenviable bus factor, and the kicker is that none of this is even Bob's fault, even though his name is on it now!

Knowledge sharing takes a lot of forms, but it's inextricably linked with work sharing. Lectures and lunch-and-learns are great ways for an individual to offload knowledge, but they're not a great way for the others to ingest knowledge. The best way for individuals to offload knowledge are through active-engagement exercises like hackathons or pair work. The _very_ best way to transfer knowledge is the "thrown in the deep end of the pool" approach, but this can also have detrimental effects if the target doesn't first know how to swim.

The best underutilized tool to get knowledge from one person to another is repetition. If our gradeschool teachers had only once told us that the mitochondrea is the powerhouse of the cell, it would not have stuck with us. They didn't tell us once though, they told us this once per year _at least_, and that repetition has reinforced that knowledge in all of us, to the benefit of meme culture a decade ago. If you have particular knowledge about key points regarding specific aspects of the system, you should repeat these points regularly across the various forms of communication you have with your organization. You don't need to start each morning in Slack with the same sentence, or set that sentence as your email signature (that's an example I just came up with but I might actually try that now). Rather, for these bits of knowledge which you can deliver as a soundbite, take the opportunity to do so just whenever it's relevant just with larger groups of people. _Particularly_ do this for those more ephemeral, touchy-feeling aspects of the system - there's a lot more knowledge which we can convey in feelings than we typically consider.

## Documentation

I will be the first to accuse myself of taking the typical software engineer's approach to writing documentation - that is, I tend not to, at least for technical documentation. The more technically focused they are, these wiki pages tend to become stale rather quickly. Whether outlining the broader system architecture, call patterns, the configurability of a component, take your pick! The facts change and it's a coin flip whether the document gets updated. If the change occurs while fixing a P1 it's several coin flips as to whether the documentation gets updated.

Documentation is necessary though, and I don't want to convey that I write _no_ documentation. On the contrary, I just don't write documentation regarding those things. Rather, I find the best documents are those about static facts which don't need updating as time goes on. My prime example is decisions - what was decided and why was it? Historical snapshots which hold long-lived knowledge about a system. Long after I'm gone and Vendor A is replaced by Vendor B, it's still going to be true that we picked Vendor A at one point, and for some reasons. Some of those reasons might persist through to the Vendor B days, and this knowledge is as valuable as gold.

For an administrator dealing with a malfunctioning system, stale technical documentation is not just unhelpful but entirely detrimental. However, being able to scour through "we did X because Y" gives the kind of knowledge helpful in a debugging context. Such documentation, properly done, should list the facts of the system at the time the decision was considered, the thinking that went into the decision incorporating those facts, and the specific decision or action that was actually taken. In future, I can always compare the facts then to the facts now, and I can always consider whether the reasoning would still be sound given the context.

I've touched on this idea before in my writing, and perhaps it deserves its own treatment, but it will suffice here to just offer this advice. Document with great frequency, and treat any point where your writing is recorded (PR descriptions and comments, meeting minutes, kanban card comments) as opportunities for documentation for posterity. Take every documentation opportunity to state the relevant facts at the time, what the thinking behind a decision was, and never forget to write out what the specific decision was.

## It's All Culture

So we've done all of the above. Our services are appropriately decoupled, each with great observability and a short turnaround time to deploy to prod. We've spread the knowledge around the team and our decisions are all documented in an easy-to-search manner. Yes, I can hear you laughing at that last one, but just suppose we've done that - what's it going to look like when it really hits the fan?

Maybe you'll be able to effectively triage and address the issue, maybe not. This hammers home the people element - how deeply have you integrated this all into your culture? There's always a relationship between our proess and culture, as well as our technology and culture; it's always that both are dictated more by culture than anything. I've seen it happen too many times that we sit down to define a process only for it to fall apart - if you want to change the process you need to change the culture. `process = f(culture)`.

Open documentation and knowledge sharing, avoiding silos and planning for risk, redundancy, and succession need to be the fundamental ways that your firm operates. It's not good enough for a single team either! Your software (probably) doesn't exist because your team formed itself from the aether - on the contrary, a bunch of stuffy business folks got some idea or another and here you are. Your architecture is dictated by their requirements, and their cultural influence is just as important - maybe more important - than your own.

Remember that these stuffy business folks (okay, maybe they're not _all_ stuffy) are administrators too sometimes, and they've probably got a _very_ different understanding of things than your senior performance engineer. Well, they both probably agree that they want the whole thing to be fast. Point being: these considerations all become circular in a way, each pointing to the next.

In a way, this is the most important lesson for developing distributed systems. Maybe it's the most important lesson in developing software. Focus on people first, most things will fall into place if you get that right. In my work I'm reminded almost daily of a great Joe Strummer quote: "Without people you're nothing."