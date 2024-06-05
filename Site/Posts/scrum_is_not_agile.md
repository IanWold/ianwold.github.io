;;;
{
	"title": "Scrum is not Agile",
	"description": "Taking a step back to try to be a bit more rigorous about these process terms we use.",
	"date": "5 June 2024",
	"contents": false,
	"hero": "photo-1480099225005-2513c8947aec",
    "related": [
		{ "title": "Reclaim Your Agile: The One Clever Trick Agile Coaches Don't Want You to Know", "description": "What if I told you there's one trick to being able to reshape your team's development process without your company knowing it? What if I told you that you can achieve actual Agile even though you work in a Scrum firm?", "fileName": "reclaim_your_agile" },
        { "title": "On Task Priority", "description": "Some thoughts on assigning priority to our tasks.", "fileName": "on_task_priority" },
        { "title": "A Scrum Oddyssey", "description": "A journey away from daily scrum meetings, as a cycle of eight Shakespearean sonnets.", "fileName": "a_scrum_odyssey" }
    ]
}
;;;

One of my favorite lectures concerned with software engineering methodology is Allen Holub's [War is Peace, Freedom is Slavery, Ignorance is Strength, Scrum is Agile](https://www.youtube.com/watch?v=F42A3R28WMU), in which I believe he convincingly articulates that Scrum is harmful and that it is not equivalent to Agile. This lecture is particularly useful in drawing a clear distinction between Scrum and Agile - even if you do disagree with Holub's assertion that Scrum is not a proper implementation of Agile, you must agree that Scrum and Agile are _different_ things.

The terms are thrown around interchangeably in our industry, and I think that does a disservice to both. I've heard a lot of criticisms levied against "Agile", but it happens that the criticism is actually against specific practices in the Scrum guide, and while not as ubiquitous I've heard the opposite as well. If you disagree with the rest of my article here, I'd kindly suggest that you at least take this point away and encourage others to be a bit more precise when speaking in this area - neither criticism nor praise are useful if misattributed.

Holub's thesis is stronger than just "Scrum and Agile are different things", though, and it's the stronger point which I find convincing and which I want to defend here. That point is that Scrum is not an implementation of Agile. Holub means this as an indictment of Scrum, where he sees the Agile princiles as being, I think it's not unfair to say, the best set of principles we have right now to guide development methodology. I've [written about opinions on this](https://ian.wold.guru/Posts/book_club_12-2023.html) before, so I'm not going to rehash _opinion_ on the matter here. Rather, I think I can defend a dispassionate form of this statement, that based on definitions Scrum can be said to not be an implementation of Agile, even though it claims to be.

Agile is very broadly defined, as well it should be. Its purpose nowadays (I do not know the original intent) is to be a category of processes - there are processes which _are_ Agile and those which _are not_, and helpfully there's going to end up being a murky middle ground with a fair amount of debate. Agile is bounded by a [manifesto](https://agilemanifesto.org/) and [twelve principles](https://agilemanifesto.org/principles.html), making it obvious that any implementation of Agile needs to satisfy these properties. Is it necessary though for an implementation to _allow_ for these properties to be included with the specific methodology practiced, or is it necessary for an implementation to _support_ or even _require_ these properties? Take for example the following principle:

> Build projects around motivated individuals. Give them the environment and support they need, and trust them to get the job done.

Certainly, if I invent a process which claims to implement Agile but in its definition precludes the ability for engineers to trust each other (say, one of its principles is "Trust nobody but yourself"), I can say that the process does not actually implement Agile. But is it necessary for an implementing system to specify that individuals within the system _must_ trust each other, or is the necessary component simply that the implementing system _allows_ for trust? Rather than splitting hairs, this is an important distinction which will affect how we interpret things, maybe drammatically. What do we do with the following Agile principle then, given this open question:

> Continuous attention to technical excellence and good design enhances agility.

This doesn't directly recommend a practice or category of practice unless we consider "enhancing agility" to be a good quality, but that doesn't necessarily emerge from the rest of the writing on the matter. As an aside, if you or a coworker are engaged in a definitional project as the authors of the Agile Manifesto were two decades ago, I can't recommend enough that you consult a philosopher, they have 2500 years of experience in this sort of work.

Vaguely defined things like Agile can be broken down into _strong_ and _weak_ forms. A _strong Agile_ will be an implementation which makes clear and explicit accommodations for each of the Agile principles - trust in work would be encouraged or required, "enhancing agility" would be said to be good, and so on. A _weak Agile_ is one which might or might not explicitly require anything from the principles, but does not preclude or discourage any of them. This is a good distinction, but not terribly useful to us as it doesn't really give us a lot of teeth to be able to determine that something _is_ or _is not_ Agile, but rather a scale of Agility.

We do actually see Agile and non-Agile things though, so we need some way of being able to draw a line in the sand. I'll propose that there are a smaller set of well-defined qualities which can be used to properly delineate Agile, and I think this can be derived from the manifesto and those principles which prescribe practice. Thus, I'll suggest that a system is not Agile unless it explicitly requires the following:

* Software must be delivered in continuously iterations, its proper function for the user must be continuously monitored, and the improvement of the software for the requirements of the user must be the highest priority at all times;
* The stakeholders in the software all must be continuously engaged with by the engineers of a software, the engineers must be the sole owners of the software, and this relationship and working pace must be able to be carried on ad infinitum;
* The engineers must be the sole owners of their own specific processes, they must continuously evaluate and adapt these, and they must accommodate change in any scope at any point in time.

These are the hard requirements which form the "core" of Agile; without specifically and explicitly adhering to these, a system is said to not be an implementation of Agile. While I've used three bullets for brevity, note that these are 9 points grouped into three.

Defining Scrum is, helpfully, much easier, [It has a very specific guide](https://scrumguides.org/docs/scrumguide/v2020/2020-Scrum-Guide-US.pdf) to reference. There are specific roles, practices, and structure defined. I'll use that document as a reference for the specific definition of Scrum.

The question then is whether this document satisfies each of the above points - I'll go one-by-one.

Scrum certainly requires the delivery of software in continuous iterations, a `sprint` is defined:

> They are fixed length events of one month or less to create consistency.

Check. Is its function being continuously monitored? `Scrum artifacts` certianly are:

> The Scrum artifacts and the progress toward agreed goals must be inspected frequently and diligently to detect potentially undesirable variances or problems.

Though the document leaves it open whether these artifacts includes the software itself, I think that it's proper to grant a charitable reading to say so. As to whether properly-functioning software is elevated to the highest priority, it seems that a team's goals are rather the highest priority:

> The Scrum Team commits to achieving its goals and to supporting each other. Their primary focus is on the work of the Sprint to make the best possible progress toward these goals.

We would certainly hope that teams would, themselves, prioritize the functioning software, but this is not explicitly stated. As for the engineers being continuously engaged with the stakeholders, the Scrum guide falls short as well:

> The Product Owner is one person, not a committee. The Product Owner may represent the needs of many stakeholders in the Product Backlog. Those wanting to change the Product Backlog can do so by trying to convince the Product Owner.

Being fair, this isolation can be beneficial in discrete applications, but to specify it as a part of the process goes against our second requirement for Agile. This Product Owner is also charged with developing the Product Goal, which is not explicitly defined. This is a problem as it ought at least be defined to exclude the possibility that the Product Owner might supplant the engineers as the sole owners of the software. Certainly nothing in the Scrum guide, I don't think, might be construed to suggest a working relationship or pace which is not infinitely maintainable.

The Scrum guide fails again in keeping the engineers the owner of their own process, the most glaring way is the inclusion of the Scrum Master role - this is not a simple inclusion and wrestles process control, at least in part, away from the engineers. The Scrum guide is quite successful, by contrast, in mandating Review and Retrospective meetings, both of which are opportunities to evaluate and adapt the process. Accommodating change in any scope at any point in time is a bit more tricky though, take the following:

> During the Sprint: [...] No changes are made that would endanger the Sprint Goal;

Surely there's some way to change course though? Well, there is this:

> A Sprint could be cancelled if the Sprint Goal becomes obsolete. Only the Product Owner has the authority to cancel the Sprint.

I don't take that inclusion to be accommodating of change though, these prescriptions might allow for some change but they place impediments in its way and refocus the attention away from change being in support of the software to change being antagonistic to the sprint goals.

Is Scrum Agile? No, in the sense that they are just different things, but also no in the sense that Scrum is not an implementation of Agile. It's not just that there are required Agile prescriptions missing from Scrum, were that the case then I might be able to say that Scrum is an implementatino of a subset of Agile. I can't say that though because Scrum contradicts some Agile requirements. This all isn't to say anything negative or disparaging about Scrum, but rather to dispassionately say that it is not an implementation of Agile. Those committed to the whole of Scrum are, I think, committed to the position that not _all_ of Agile is desirable. So as to not throw the baby out with the bathwater, that might be alternatively phrased that _only some portion of Agile is desirable_. In the same way, those who are committed to the whole of Agile are committed to the position that not _all_ of Scrum is desirable. Perhaps portions of it or ideas from it are.
