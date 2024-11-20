;;;
{
	"title": "Three Laws",
	"description": "Some "folk laws" that are commonly known but seldom applied.",
	"date": "20 November 2024",
	"contents": false,
	"hero": "photo-1593115057322-e94b77572f20",
    "topics": ["Processes", "Standards", "Architecture"],
    "related": [
		{ "title": "Eight Maxims", "description": "A few principles for thoughtful software engineering.", "fileName": "eight_maxims" },
		{ "title": "Clean Meetings: A Software Engineer's Guide", "description": "If being in meetings all day isn't bad enough, spending more time thinking about them seems horrible. Here's a simple guide on making sure you're getting the most out of your meetings.", "fileName": "clean_meetings_a_software_engineers_guide" },
		{ "title": "Reclaim Your Agile: The One Clever Trick Agile Coaches Don't Want You to Know", "description": "What if I told you there's one trick to being able to reshape your team's development process without your company knowing it? What if I told you that you can achieve actual Agile even though you work in a Scrum firm?", "fileName": "reclaim_your_agile" }
    ]
}
;;;

There is plenty of conventional "folk"-ish wisdom to be had, and a lot of it is widely known if sometimes unintuitive. This knowledge coalesces in the form of popular "laws," particularly in areas like management and economics. Plenty has been written about the application of these laws to the software industry, to varying degrees of acceptability. The implications they have for us can be surprising, and studying them can give an intuition for the unintuitive: how to organize the best team, deliver the most successful project, or architect the best codebase.

Here's three which I find particularly helpful.

**Parkinson's First Law**

> Work expands so as to fill the time available for its completion

That is, if I go through a pointing session and come out with a two week estimate for a task, it will take two weeks. If I estimate one week, it just might come out to one week. There are indeed efforts which take more or less time, but efforts afforded more time _will_ use that time.

This is one of the inefficiencies caused by estimating, and it becomes especially difficult if a culture of "it's okay to point a little higher just in case" develops. This has the potential to cause a feedback loop, where tasks are estimated with a bit of wiggle room, that room is _used_ (per Parkinson's), a new baseline is thus set and estimates expand again to maintain that "little room." If this sets in it will grind a team to a halt eventually - there won't be time for small tasks and important quality work like refactors and vulnerability patches will be punted in favor of feature development.

Do not estimate! There are other reasons beyond Parkinson's to not estimate, but this is a big one. Forecast instead. In fact, you don't need to measure time taken on cards, you [just need to count cards](https://www.youtube.com/watch?v=QVBlnCTu9Ms) to get forecasting that's more specific than estimation.

**Law of Triviality**

> People in an organization devote a disproportionate amount of time to trivial issues

I think of this like an 80/20 proposition: 80% of our resources will be spent on the trivial issues and 20% on the actual thing we need to focus on. One explanation for why this happens is that the trivial issues are easier for many people to understand, biasing people to discuss these. Another reason is that these issues are easier to _disagree_ about - if I'm wrong about a complicated technical issue that might call my competence into question, whereas being wrong about inconsequential issues has, obviously, no consequence.

Following the 80/20 idea, only commit 20% (the bare necessity) to tackling the issue needed. Should you have 15 folks on the call to go over the issue, or 3? Should that call be 60 minutes or 15? Should your meeting have an open - or no - agenda (allowing any topics to be discussed as they arise), or should you have an agenda with the 1-3 items that need discussing?

Do not expand! Liberally cull resources from the problem-solving space.

**Jevon's Paradox**

> As the efficiency of consuming a resource is increased, the net consumption of that resource increases

This is a paradox as you'd expect the resource consumption to decrease, but instead the new efficiency induces an extra demand on that resource. This is a well-known phenomenon in urban planning: when lanes are added to a highway, it tends to cause the highway to have more jams than it did before.

As software engineers, we tend towards making things more efficient. New processors have more cores, algorithms run faster than old algorithms, cloud platforms can offer more "computes" per dollar, etc. We can fall in a trap thinking this can afford us to write code at higher-than-necessary levels, less efficient business logic, or that we can "just" use more cloud products. In the extreme this has manifested in an outsized adoption of distributed architectures; particularly microservices.

Do not complexity! [Complexity very, very bad](https://grugbrain.dev/)! When we write software or make architectural diagrams or the like, we talk about taking several "passes". There's the "get it to work" pass, the "make it pretty" pass, and so on. Always make sure you have a "make it simple" pass. Heck, do several! How many arrows and boxes can you take out of the architectural diagram? How much code can you get rid of? How many iterations over that list of widgets can you consolidate?