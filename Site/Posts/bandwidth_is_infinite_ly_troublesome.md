;;;
{
	"title": "Bandwidth is Infinite ... ly Troublesome",
	"description": "The bandwidth of the world-wide web has increased dramatically, but so has its demand. There's an abolute limit to how much data we can all transmit, and working around that requires dilligence.",
	"date": "26 April 2024",
	"contents": true,
	"hero": "photo-1451187580459-43490279c0fa",
    "topics": ["Distribution", "Performance"],
	"series": "Fallacies of Distributed Computing",
	"related": [
        { "title": "Latency is Zero and the Speed of Light is Getting Faster", "description": "Latency is a constant and unavoidable fact of nature, but we can plan for it, work around it, and respond to it.", "fileName": "latency_is_zero" },
		{ "title": "The Network is Unreliable and Reliability is Scary", "description": "Indeed the network is unreliable, and this is especially concerning for modern, distributed system. The catch though is that it never can be 100% reliable, and we can't create systems that perfectly compensate for this.", "fileName": "the_network_is_reliable" },
		{ "title": "Just Use PostgreSQL", "description": "With a vast and growing ecosystem of database systems, data models, patterns, and paradigms, choosing the right one can be a long and complicated process. I prefer a simpler approach: Just use PostgreSQL.", "fileName": "just_use_postgresql" }
	]
}
;;;

In my last [post on latency](https://ian.wold.guru/Posts/lateny_is_zero.html) I cited an interesting definition given by David Boike: `TotalTime = Latency + (Size / Bandwidth)`. This suggests that latency and bandwidth are related, and that's intuitively true. At a very simplified level, one deals with packet count and the other with packet size; these add together into a concept of packet volume, and so issues related to each do compound on each other. In that article, I was able to demonstrate some techniques which, depending on how aggressive you might be able to be with your system's requirements - can very largely mitigate issues related to latency.

Bandwidth is not, I think, so simple. Indeed, the total bandwidth of the world has increased markedly year over year, but then our demand for it has as well. In some cases our demand is outpacing our ability to increase bandwidth. Theoretically we _could_ increase bandwidth enough to meet way more than we could ever demand, but a 1-mile-diameter cable between New York and London sounds slightly infeasible. I'm not a cable engineer so I won't get too deep into that debate, but until they give us that cable we'll need to deal with bandwidth.

This is the third fallacy of distributed computing, that bandwidth is infinite. Of course there's a physical limit to how much data we can send, and there's not really any clever tricks here that can mitigate that terribly well. The first step, maybe the most difficult, would be to not transmit erroneous data. From there, we can compress data, tier it by importance, and maybe shift around where it physically gets moved. Nonetheless, we can't adopt an attitude which ignores it.

# Bandwidth Happens

I'll end up sounding quite like a broken record in this article, but it's for good cause. If you're sending a message over the wire, you're consuming bandwidth. The best strategy to prevent this from becoming an issue is to not send data over the wire. Don't architect a distributed system. That's not entirely satisfactory - some systems do need to be distributed. Some systems _have_ to serve their users over a website.

## A Cost of Doing Business

It's apt in a couple ways to suggest that bandwidth is a cost of doing business. Of course it costs money to consume; distributed systems need to pay for this, and large systems are going to have to pay more. Cloud providers know this, and depending on your case you might face an [eye-watering bill](https://world.hey.com/dhh/why-we-re-leaving-the-cloud-654b47e0). Each unit of bandwidth can be exponentially more expensive at higher levels, and this is a more immediate limitation on our systems than that absolute bandwidth is capped.

Describing it as a cost of doing business is apt also because any distributed system requires _some_ consumption of bandwidth; this is a cost in itself. While we have some control over whether messages in our system incur latency, we have no control over that they use up bandwidth. This potentially makes bandwidth-related problems more severe; there are fewer tools at my disposal to mitigate these problems.

## Induced Demand

[Induced demand](https://en.wikipedia.org/wiki/Induced_demand) is a term used in urban planning, it's a phenomenon whereby increasing the number of lanes in a highway causes traffic problems to worsen as it causes more people to choose the highway to commute, thinking that the added capacity will benefit their travel. I think the same thing happens to us with bandwidth. As our bandwidth capacity has increased, users expect to be able to consume more data, businesses expect to be able to ingest and work with more data, and as engineers we architect and build systems which fulfil these requests. We see they're constantly adding more cables and we see other systems working with much more data, we feel more and more comfortable transporting more of our own data.

Media and streaming has become more ubiquitous and has come to incur a higher cost. Ten years ago I was happy with 720p video from YouTube, now I can stream 4k! I stream 4k video from Netflix and haven't touched a DVD in years, I stream music all day instead of listening through my CD catalog. Videos play instantly on my social media apps now.

IoT has caused a huge proliferation of devices connected to the internet, and if these devices aren't streaming data somewhere they're phoning home with some frequency. Cloud computing has made it cheaper and easier than ever to set up systems that add to the problem, and lately even some small sized companies are interested in what they can do with big data.

How much of the data we're sending over the wire is erroneous? I don't know the exact number, but I know it's not 0% - I've written software that's piped unnecessary data between services! I apologize for my part in this. For what it's worth, I do try - more now than earlier in my career - to keep the _amount_ of data to a minimum. We should all be proud to build a system that is as light on the internet infrastructure - and our private infrastructure - as possible. 

# Mitigating Patterns

Once we've eliminated erroneous data from being transmitted around our system, we can take some steps to reduce the strain we put on the cables. The smaller we can make our messages the better, and we have a couple of patterns and practices to follow here.

If we've reduced our packets as much as we can, then we need to deal with the physicality of our traffic. Monitoring and prioritizing critical traffic or geolocating where the bulk of that traffic happens can give us more fine-grained control over keeping our bandwidth usage in check.

And if _those_ strategies fail, we can always throw our hands up, despair, and yell at the cloud. Haha, get it? The _cloud_!

## Serialization and Compression

The first and most obvious tool to reduce the amount of data sent over the wire is ... reducing the amount of data sent over the wire. Maybe that seems obvious but a lot of folks miss this first step, especially serialization!

JSON has become the ubiquitous way to serialize data, but it has plenty of inefficiencies, other formats use fewer characters to represent the structure of the data and can be parsed much faster. Indeed, I would expect that there's a correlation between how many characters a format requires to represent a structure and the speed of (de)serialization.

For those of us in the .NET world, [MemoryPack](https://github.com/Cysharp/MemoryPack) is an attractive option, being up to 200 times faster than the first-party `System.Text.Json` and coming in at a smaller serialized output than JSON. The catch of course that prevents this from being used in a lot of distributed applications is that the client needs to be able to deserialize it - you'll need to either use .NET or their TypeScript code generator. At least if I use JSON then anyone will be able to deserialize it.

An attractive option then is [protobuf](https://protobuf.dev/), a fast and small format developed by Google. This format is relatively ubiquitous and has demonstrated real-world effectiveness: LinkedIn adopted it and [reduced latency by 60%](https://www.infoq.com/news/2023/07/linkedin-protocol-buffers-restli/)! Though they measured latency, these gains were due to smaller message sizes resulting in a reduced impact on bandwidth.

Compression is the other factor here, and it's a saving grace that there are a fair number of compression algorithms available to us; we need to worry less about language-specific tools when we consider compression. Algorithms like gzip and brotli have many implementations in every language, and MemoryPack even comes with a brotli compressor built in. However, speed is crucial here. Plenty of compression algorithms are fast enough but this is important to consider.

## Claim Check

Simply put, this pattern suggests using a URL to the location of a resource rather than the resource itself as the payload across the wire. You've surely encountered systems where images or other files are stored in a CDN separate to the application's persistence, and URLs are used to represent these resources. This is sort of the same idea.

This is a good pattern to use with asynchronous communication if events contain enough data - you can store the payload in a persistence system and just pass the URL as the event content. Naturally though, you might need to include some subsection of the data with the URL so that consumers can identify the data and choose whether and how to react; you wouldn't be saving much bandwidth if each consumer needed to fetch the data at the URL to check it.

Naturally, this is much more effective for larger payloads, and might be harmful for very small amounts of data. I'd expect though that you're not running into bandwidth issues with small events though. I would also suspect that geography plays a part in deciding at which size of resource this pattern might be a benefit; penalties for long-distance data transfer are not incurred solely due to bandwidth limitations!

As an aside, I will say that I've never actually seen this pattern used except insofar as keeping images on a separate CDN could be considered related. However, I was speaking with a colleague the other day, inquiring about his firm's approach to mitigating bandwidth concerns, and he said something to the effect of "If we have bandwidth issues we just 'S3' it." I suppose this pattern is quite natural then!

## Traffic Management

In a lot of cases, bandwidth becomes an issue when it affects a particular type of resource, especially content that we need to deliver to a user or data that's otherwise in the "hot path". It makes sense that _if_ we're going to have issues with bandwidth then we're going to at least want to be able to keep those issues away from certain paths.

Managing traffic to shape bandwidth concerns can be as simple as setting bandwidth or rate limits on different parts of your network. By throttling non-critical systems you're naturally freeing up resources to be able to be consumed by other paths. This can require some monitoring and fine-tuning, but it's a simple solution that can be effective.

Depending on your use case, you might either want to buffer or drop packets when bandwidth limits are hit. I think the default option would be buffering for obvious reasons - we generally want to preserve data. Dropping packets is generally preferable though in scenarios like streaming audio or video to a client (where buffering can compound on itself) or for non-critical data like logs or telemetry data, if the occasional loss doesn't have a significant impact. IoT comes to mind here - systems which stream large amounts of sensor data will probably benefit more from dropping packets.

Setting up a priority queue is only slightly more involved but a more elegant and controllable solution. Message queues typically support this feature, as do gateways like firewalls and load balancers. From an architecture perspective, it's probably good practice anyway to intentionally identify and appropriately segregate the critical parts of the system anyway.

## Edge Computing

Redesigning your application to run at the edge is probably (read: certainly) overkill if you're _just_ trying to solve bandwidth issues, but it should be clear that edge computing gives you more control over how and where you send messages, which allows you to shape bandwidth use at different points in the network.

By handling local traffic locally, you're not just keeping data away from your central system, you're also potentially shortening the wires over which the bulk of your data is transferred; bandwidth issues are more issuesome at larger distances. This is almost a requirement anymore for streaming applications - I can't imagine that Netflix could service most users streaming 4k data from New York to Hong Kong. The centralized systems typically get the lion's share of bandwidth allocation, but interestingly edge computing challenges this because the nodes of the network become bandwidth-heavy. Again, this is an advantage for systems which have bandwidth concerns that are heavily geographically-influenced.

Edge computing adds a fair bit of complexity and a heap of cost though, and that's why I suggest that it's not a solution exclusively for solving bandwidth concerns. I think it's fair to be skeptical of edge computing except for cases that truly require it. If you are computing _on the edge_ though, you're probably dealing with a fair amount of data and one of the multitude of concerns which should motivate the architecture of your system should be these bandwidth considerations.

# Conclusion

Our key first strategy to minimizing latency concerns is to send fewer requests, and our key first strategy to mitigating bandwidth concerns is to send less data. This seems like we're trapped then, like there's a hard, physical limit to the amount of data we can send at any one time. Indeed, this is the case. It seems like we're naturally pushed towards not distributing our system. Again, this is not just a reasonable conclusion but probably the correct one. Distributed systems are more complex, more error prone, and more costly. By being careful about _what_ we distribute we can manage that complexity when we have to.

In those cases where distribution is required, approaching their design with all of these fallacies in mind will only get us so far too. Large streaming systems still fail with some frequency and can only fall back on managing their users' expectations with a thoughtful error message when there simply isn't enough bandwidth to deliver their video.