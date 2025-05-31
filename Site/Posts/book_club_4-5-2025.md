;;;
{
	"title": "Book Club 4&5/2025: Incidents and Resiliency",
	"description": "Thinking more about responding to and preventing incidents",
	"date": "31 May 2025",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Processes", "Architecture", "Standards", "DevEx"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 2&3/2025: Slack: Communication, Organization, Teams", "description": "On various aspects of teamwork, communication, and the like", "fileName": "book_club_2-3-2025" },
		{ "title": "Book Club 1/25: Results, Railways, and Decisions", "description": "A story on a successful result and insights gained from some more heady research this month.", "fileName": "book_club_1-2025" },
		{ "title": "Book Club 12/2024: Team Metrics and KPIs", "description": "Allow neither ignorange nor complacency to abuse metrics or KPIs; these are important tools for the competent professional.", "fileName": "book_club_12-2024" }
	]
}
;;;

In March I moved to a different role at my current firm, and the thing that's changed the most for me is incidents. In the past quarter I have had to respond to more incidents than I had in the two and a half years prior. Comes with the territory sometimes, I suppose, but that puts the topic into my mind!

I'm very happy to have been subscribed to Lorin Hochstein's blog [Surfing Complexity](https://surfingcomplexity.blog/), which has given me a lot of links to share with colleagues. Lorin's approach to rigidity in complex systems is to center the interactions of the components of the system as the potential, and sometimes actualized, causes of failure. This is opposed to the traditional model of there being a single "cause" or "actor" within the system from which the failure arose. Stated plainly this seems like an obvious thing to think, but turning that thinking into something _operational_ is a different thing altogether.

Following up on an incident, how do we update our system to exclude failure-causing interactions? How do we do this in a way that avoids thinking about "root causes"? Most fundamentally, how do we even identify all the interactions in the first place? I figure that the complexity of a system will directly correlate with the complexity of incident-causing interactions. I also figure that complexity of any sort directly correlates with lack of understanding.

So, as the complexity of the system increases, the complexities of the causes of failure increase, the ignorance of the workings of the system increases, and the inabilities to comprehend the causes of failures increase. In short, the probability that the holistic understanding of the causes of an incident cannot be had increases with time. The greater the possibility that I can't understand the full picture as to why an incident occurred, the less useful it is for me to engage in root cause analysis. In such a system, the root cause analysis will at best not fix the issue, but at worst will introduce new issues.

What's to be done then? Resiliency, of course! Also easier said than done. The implication is that failure can never be ameliorated, but can only be managed. A system that better manages errors is more resilient and will function properly a greater percentage of the time. That might be an unfulfilling answer, but I'll ask you whether that maps better onto your experiences with complex systems?

When we follow up from incidents, we can identify individual failures in components of our system, and we can probably identify several points of interaction failure. The question to answer is how we can update the components to succeed in the face of the errors they encountered, not how we can fix the problem. Assume you can't fix the problem, because you can't comprehend the problem. You can only control the limited view you have over the system, and you can only make that slice of the system behave _better_ in its error-laden environment.

I was convinced to write about this topic for the book club this month by this recent post from Lorin : [Not causal chains, but interactions and adaptations](https://surfingcomplexity.blog/2025/05/19/not-causal-chains-but-interactions-and-adaptations/).

Links from folks other than Lorin, in alphabetical order of their names:

* [The “Bug-O” Notation - Dan Abramov](https://overreacted.io/the-bug-o-notation/)
* [Writing Resilient Components - Dan Abramov](https://overreacted.io/writing-resilient-components/)
* [People that make computers go crazy - Gojko Adzic](https://gojko.net/2017/12/08/people-making-computers-crazy.html)
* [Redefining Software Quality - Gojko Adzic](https://gojko.net/2012/05/08/redefining-software-quality/)
* [The most efficient way to solve problems: not having them - Lucas F. Costa](https://lucasfcosta.com/2020/09/05/not-having-problems.html)
* [Consistency vs. Availability - Julia Evans](https://jvns.ca/blog/2016/10/21/consistency-vs-availability/)
* [System operations over seven centuries - Martin Kleppman](https://martin.kleppmann.com/2013/08/12/system-operations-over-seven-centuries.html)
* [Hermann Bondi: Arrogance of certainty - Martin Kleppman](https://martin.kleppmann.com/2008/06/10/hermann-bondi-arrogance-of-certainty.html)
* [The complexity of user experience - Martin Kleppman](https://martin.kleppmann.com/2012/10/08/complexity-of-user-experience.html)
