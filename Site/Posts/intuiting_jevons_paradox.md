;;;
{
	"title": "Intuiting Jevon's Paradox",
	"description": "On the unintuitive pattern of resource consumption and how it relates to software engineering.",
	"date": "13 December 2024",
	"contents": false,
	"hero": "photo-1635241161466-541f065683ba",
    "topics": ["Learning", "Standards", "Architecture", "Industry"],
    "related": [
		{ "title": "Book Club 10/2024: Fallacies of Distributed Computing", "description": "Putting a pin in the series I wrote this year on the topic of these Fallacies", "fileName": "book_club_10-2024" },
		{ "title": "Learn the Old Languages", "description": "New languages are hip, old languages are erudite. Don't neglect these languages as you round out your skills.", "fileName": "learn_the_old_languages" },
		{ "title": "Eight Maxims", "description": "A few principles for thoughtful software engineering.", "fileName": "eight_maxims" }
    ]
}
;;;

I [wrote in a recent post](https://ian.wold.guru/Posts/three_laws.html) about Jevon's Paradox, and I received some feedback on that; more than I usually do. As always, I'm grateful when you all [contact me](https://ian.wold.guru/connect.html) though continue to encourage use of the [excellent comment feature](https://ian.wold.guru/Posts/giscus_is_awesome.html) or, if you want to be one of the cool kids, [webmentions](https://ian.wold.guru/Posts/ive_indiewebbed_my_site.html), both of which I recently included on this blog! Anywho, I want to expand on this topic.

Jevon's Paradox is one of those very unintuitive but easily observable facts of life. It was originally descried by economists (so I'm lead to believe) but has applications across the board, economics being the strage confluence of so many different social studies as it is. There are many different staements of the paradox, but I like this one:

> As the efficiency of consuming a resource is increased, the net consumption of that resource increases.

That's a bit of a compact definition, so it's worth giving a more plain-language explanation. It's not a paradox in the logical sense (i.e. [Russell's Paradox](https://en.wikipedia.org/wiki/Russell%27s_paradox)), but in that its implication subverts our expectations. By way of example, I can explain it using the well-known concept of [induced demand](https://en.wikipedia.org/wiki/Induced_demand) in urban planning. Suppose we live in a city with a highway going through it and we've been tasked by the mayor with finding a solution to that it always seems to be in a traffic jam. The most straightforward and intuitive response would be to add more lanes to the highway, right?

In fact, if you clicked that link describing induced demand, you'd know that adding more lanes is not the right answer, as it will actually cause an even greater demand on the highway than there was before. What's more, this is not a linear demand: if the current demand for the highway is x cars per lane per hour, after adding more lanes it's actually xy cars per lane per hour, where y is some constant greater than one such that the resulting demand is even more intensive after adding more lanes. This is the "paradox" bit - it's entirely unintuitive that increasing the units of highway resource (lanes in this case) should result in an even higher demand per resource unit. We expect demand to remain the same, and that increasing the efficiency of use will resolve issues in congestion caused by that use. Because of Jevon's Paradox, however, not only have we failed to resolve the congestion issue but we've even made it worse.

What of these implications then? There are a lot of conclusions to draw. First, and I think most importantly, we can say that when there are congestion issues with the consumption of a resource, making that consumption easier will exacerbate the problem and should not be considered as a solution. What solutions there are to the problem are probably context-specific, but if it involves increasing consumption efficiency then that's not one.

The next conclusion we can draw is that the inverse of Jevon's is probably true:

> As the efficiency of consuming a resource is decreased, the net consumption of that resource decreases.

If I remove lanes from the highway, then of course there won't be as many cars on the highway. To extend this example, I might ask if removing lanes has resolved any issues though? There is some level of need to consume road resources, so unless I can find the root cause of _that_ need I'm probably just shifting the problem around by changing the highways. This is another important conclusion from Jevon's, albeit more indirect than the others: address problems at their root. It seems that [the five whys](https://en.wikipedia.org/wiki/Five_whys) end up in a lot of places!

Jevon's Paradox is so called because it's unintuitive, and indeed without a proper intuition for its implications you might find yourself amid the problem it describes. Certainly we're not all urban planners dictating the adding or removing of highway lanes, but indeed it has plenty of applications for us software engineers. Any software is necessarily a resource user - there's computational, storage, and networking resources which are easy for us to understand. The ease of consuming each has only increased over time, but is our software any better? In a lot of ways software has become much less efficient, bloated, worse for the users, and slower. Jonathan Blow [has documented this quite well](https://www.youtube.com/watch?v=FeAMiBKi_EM) I think. Are we Jevon's-ing ourselves and our software?

Maybe. I'm certain a collective failure to intuit Jevon's is not the only problem with our industy, and I'm doubtful that it might even be a major factor. However, I find it quite compelling to think in terms of resource consumption, and Jevon's is one of the prime gotchas for the ways in which software could be said to be a resource consumer.

How to intuit it? The answer is common to many problems in software engineering: I think you need to think about it really hard. This is also my answer when colleagues ask how I know my code works. Being infallible as I am, this of course proves to be correct every time and I have never had an instance of being wrong on this count! To be serious though, this is one to keep in the back of the mind. When approaching architectural questions, ask whether you're solving a resource problem in the wrong way, or if you're creating a potential one. Consider whether Jevon's applies, and if you're diagnosing an existing resource problem consider whether it _had_ applied.