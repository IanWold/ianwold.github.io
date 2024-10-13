;;;
{
	"title": "There's More to Network Security than the Network",
	"description": "Assuming a secure network in a distributed system loses sight of all the ways vulnerabilities can creep into our systems. Just as distributed computing makes our systems 10x more complex, the same effect is felt on security.",
	"date": "30 May 2024",
	"contents": true,
	"hero": "photo-1451187580459-43490279c0fa",
    "topics": ["Distribution", "Security"],
	"series": "Fallacies of Distributed Computing",
	"related": [
        { "title": "Bandwidth is Infinite ... ly Troublesome", "description": "The bandwidth of the world-wide web has increased dramatically, but so has its demand. There's an abolute limit to how much data we can all transmit, and working around that requires dilligence.", "fileName": "bandwidth_is_infinite_ly_troublesome" },
        { "title": "Latency is Zero and the Speed of Light is Getting Faster", "description": "Latency is a constant and unavoidable fact of nature, but we can plan for it, work around it, and respond to it.", "fileName": "latency_is_zero" },
		{ "title": "Roll Your Own End-to-End Encryption in Blazor WASM", "description": "Using the SubtleCrypto API to get simple end-to-end encryption for a collaborative Blazor WASM app.", "fileName": "end_to_end_encryption_witn_blazor_wasm" }
	]
}
;;;

As I'm reviewing and writing overviews of the fallacies of distributed computing, I'm trying to keep observations and suggestions at a 10,000-foot view: I want to describe each problem, related patterns, and convey the mindset behind the fallacy. Each fallacy is quite discrete, has obvious examples, and clear patterns that directly relate to it. The fourth fallacy, however, is a gigantic topic - "The Network is Secure" covers _all_ of network security. I wonder if it's misplaced in the fallacies then, as we could very easily imagine a distinct set of the Fallacies of Network Security.

I'm not a security expert, I avoided the security class in university, and I typically find myself relying on colleagues for the more involved security considerations. That's unfortunate because security is so important and overlooked. One study [cited by a University of Wisconsin paper](https://pages.cs.wisc.edu/~zuyu/files/fallacies.pdf) suggests that 52% of networks are only defended at the perimeter. The link to that study no longer exists and I have a lot of questions about methodology and specific definitions, but I think we can all intuit that there's some truth here.

I'm not a great authority, then, to recommend specific practices for mitigating security issues; indeed, I'll allow someone else to write a series on the "Fallacies of Network Security". However, I can speak about the areas where security concerns come up, and why those areas are a problem. Just as well then, otherwise this article could be far too long! I hope I can convey the idea that even for those of us developing alongside a strong, security-minded colleague, we can adopt a holistic view of security issues and keep them in mind in the course of our work. I don't think I'm going to point out any new, groundbreaking information here, but it can be helpful to have these top-level ideas collated in one place.

# Keep the Services Secure

The very baseline of any security is at the level of the services, and not the network itself as one might assume. This is the fallacy - the assumption that the whole network is secure leads us to develop more vulnerable services. We might develop services that are relaxed in their authorization or authentication, don't implement proper (or any) access control, or that don't properly validate user input.

There's a lot to be said about authorization and authentication. _That_ you have auth in place is the first step, but there's a lot of considerations when it comes to the specifics. How you handle keys and passwords is important, and if you're relying on third party auth then the ways you go about securing your communication with them, and how you handle the keys you receive from them, matter. This is an important area of focus, for you and your team to thoroughly consider how these are all handled.

Access control plays into this - _which_ users can access _what_ data? Ensure you're appropriately applying the principle of least privilege - users should only have as much access as they've been authed to have. This is important at the level of each individual service, being skeptical of the security of the whole network is the idea here. Even guarding against internal denial of service attacks by implementing rate limiting might not be a bad idea, depending on your situation.

Even when I can be sure I'm correctly dealing with requests from a valid user, I still need to handle their inputs appropriately. This doesn't just end at sanitizing SQL inputs to guard against injection attacks - is your user able to input anything - say, regex - that can get around other data protections? Maybe your service's auth code is so complicated that it's easy for incorrect auth attributes to be associated with a request as it's processing and you accidentally give users higher privileges based on their own inputs. You might roll your eyes at that suggestion but I guarantee you it's less funny to encounter in the wild!

To boot, you can get issues with poor configurations - the use of default passwords is one that I encounter with some regularity. Most systems should have guidelines on security practices as regards their configuration, and this shouldn't be overlooked. This also bleeds from the service itself to how the servie itself is configured when running; yes, those running your service should be following your practices, but you should set them up for success by creating a service which is difficult or impossible to misconfigure.

# Keep the Data Secure

The next obvious security concern is data transmission. I hope you're using HTTPS for everything! HTTPS makes it easy to start securing this area but it's not the end. Particularly if you're handling sensitive data like credit card numbers, you're going to need extra protections in place.

If you do have sensitive data, you probably want that data encrypted as it goes over the wire. Using proper, not-outdated encryption algorithms is important - there's still a fair amount of SHA-1 out there. Keeping up-to-date here is important too - algorithms and practices fall out of date like libraries, frameworks, or other practices.

Data integrity can also be a concern depending on your circumstance. It's good practice generally to use a hash as a checksum for your data, not just for the security aspect but to ensure your packets are transmitting correctly. That's quite circumstantially dependent and up to you and your team to choose whether it's needed in your case, but if you're transmitting sensitive data encrypted anyway you probably want this guard as well.

Access controls certainly help keep the data secure as well - if we appropriately guard who can receive which information then we've won half the battle. The regular suggestions for securing a network - firewall, maybe a VPN - help here too, but remember that if you're developing services within a network you can't rely on these being present or even helping you if they are. Security can't be assumed.

Data security has an internal aspect, too - sensitive data should be masked if it's stored internally in logs or audits. Properly enforcing how sensitive information is handled on your team is quite important not just because you don't want a rogue employee misusing it with malicious intent, but you want everybody protected if any of your internal systems are compromised. This is a vigilance mindset which is important to develop and maintain whenever we're working on our systems - you don't want to be the one whose laptop was breached and exposed PII!

# Guard Against Third Parties

In [my article on latency](https://ian.wold.guru/Posts/lateny_is_zero.html) I pointed out that third parties can introduce latency onto us, and there's a similar relationship in a security sense - third parties can introduce vulnerabilities on us. This can happen whenever we have a connection with a third party, the obvious one being whichever third party services ours call into. A less obvious occasion for third-party-induced vulnerabilities that still effects everyone is when the building or delivery of our software is compromised - this process almost always involves external systems.

I think the first, most obvious step to guarding against vulnerabilities from a third party system is to make sure the system and the company owning it was appropriately vetted before relying on them. Something like Azure seems to be a pretty safe bet that they're going to do, and continue to do, what they can on their end to keep vulnerabilities away from you. Other vendors might be more or less trustworthy, but the point then is that you probably shouldn't replace MailChimp with BobsSuperGreatEmailService after a thorough review.

If you _do_ need to consume BobsSuperGreatEmailService though, hiding it behind an isolation layer would be a smart move. That can be said for any system security-wise. Does every external system need an isolation layer? Maybe, that's up to you and your team to determine. There's pros and cons here, and the point is that security should be one of the considerations when working through this.

The supply chain vulnerabilities are the more interesting ones here. We all rely on third parties - sometimes a _lot_ of them - to build and deploy our systems. Every codebase I know brings in several libraries through a package manager, just like those libraries rely on others, and so on down the line. You can be dependent on dozens or hundreds of libraries, and each of them is a security concern. Including a library should be the result of an audit, and these need to be continuously reviewed to catch vulnerabilities if they occur.

This might seem like a far-off case, but my friends in the .NET world will recall the recent incidence where [the maintainer of the most popular C# mocking library put malware in a release](https://www.youtube.com/watch?v=A06nNjBKV7I), harvesting emails from the machines of any developers who built their code with the latest version of his library. The most charitable reason to give why he did this is profound ignorance, and it shows that security vulnerabilities have nearly infinite vectors to exploit. I commend Nick Chapsas for having shared that video as soon as he did; I was able to raise that alarm at my firm that morning and over the next two weeks one of my colleagues painstakingly removed Moq from our main codebase. Indeed, incidents like this are largely reactive.

Some of the most impactful and famous attacks have been supply chain attacks. The [SolarWinds attack](https://www.cisecurity.org/solarwinds) was a compromise of their delivery system, and the [CCleaner attack](https://www.wired.com/story/inside-the-unnerving-supply-chain-attack-that-corrupted-ccleaner/#:~:text=The%20software%20updates%20users%20were%20downloading%20from%20CCleaner,distributed%20software%20is%20actually%20infected%20by%20malicious%20code.) saw legitimate versions of the product compromised with malicious code. While these third party vectors are some of the most obscure and difficult to guard against, these compromises can result in serious damage.

# Guard the Whole Network Against Attacks

This seems like the easiest and first thing to do really - keep malicious actors outside my network of distributed components. As I keep mentioning though, this is the source of the fallacy! Indeed, we _do_ have to do what we can to secure the network, however it's vital to ensure the individual components within the network, their connections outside the network, and the data transmitting across the network are completely secure. If those components are engineered to be appropriately secure, then the security of the network as a whole is an extra layer ensuring a lower number of vulnerabilities, rather than being the last line of defense.

On the flip side, you might observe that since all of the constituent components of the network are secure, it seems like the network itself doesn't need to be a focus of our vigilance. Au contraire! Not only do you lose the multiple layers of security which I described above, but there are some advantages you get from the network security layer.

Firewalls, VPNs, and intrusion detection systems can be configured for the entire network, giving a blanket level of security for all of its systems - not that they're relying on this - important! These intrusion detection systems are particularly interesting and can't really operate as well within the context of an individual service.

Denial-of-service attacks, particularly of the distributed sort, are famous and capable of taking down entire systems. Individual services should be guarded against denial of service attacks, but the network as a whole should be as well. Rate limiting, traffic filtering, and a good load balancer can be employed at the network level to alleviate this concern. I think cloud providers have products specifically for these sorts of attacks as well, but I'm not sure how effective they are.

The topology of the network can help its security too. Segmenting the network into isolated areas can help contain breaches, which combined with firewalls and intrusion detection systems can give you a lot of control and insight into the state of the system. Ensuring that no actor in the network are trusted - even internal ones - helps to maintain the focus on security throughout the system, forcing proper auth checks at each stage.

# React to Attacks and Vulnerabilities

As I mentioned before, the number of vectors for attack are too many to be able to guard against all of them. Unless you have a particularly honed security mind (I do not) then you're unlikely to be able to imagine a lot of them. I don't want to discount the importance of building secure systems, but in a lot of ways the most important action to ensuring a secure system is to actively monitor and respond to attacks and detected vulnerabilities. Adopting a security mindset isn't just about being cognizant of the technical aspects of security, but realizing our limitations and building practices and processes to shore up our shortcomings.

Routinely scanning and auditing for vulnerabilities is the first step, and when we know that we don't know what we don't know (how many times can I say "know" in a sentence), this practice can be the best teacher for us. If your code is hosted in GitHub, it [has excellent security scanning products](https://github.com/security) which can help to stay on top of known vulnerabilities, particularly with respect to your code's dependencies. If you're fortunate enough to have a security-minded person on your team or a security team at your firm, they can potentially help you set up a process of auditing attack vectors in your services and system.

You do need to prioritize fixing these, however! It's no good finding vulnerabilities only to shrug and say "probably not gonna get found". Just because a system hasn't been exploited yet doesn't make it secure, and part of our professional duty is to deliver software as secure as we're able to. Known vulnerabilities must be prioritized.

Once we're in prod, we still need to observe our systems as they run. We can't prevent all security issues before we deploy, so being able to catch them when they happen is crucial. Security Information and Event Monitoring (SIEM) systems are purpose-built for this kind of monitoring - if you're securing a distributed system I'd make the assumption that you're able to devote resources for a product like this.

Again here, when alerts are raised they need to be prioritized and responded to - it's no good knowing the vulnerability exists (an if detected here, probably exploited) only to not fix it. It would be good to have a conversation about what the plan should be when a security breach is detected - this likely involves a lot of parts of the organization outside just engineering. I've seen a lot of firms - much bigger than you might expect - that didn't have these clearly laid out. Being explicit about what should be done for security incidents makes it easy for anyone to be able to react to the incidents when they occur, which strengthens the overall response from the team or firm.
