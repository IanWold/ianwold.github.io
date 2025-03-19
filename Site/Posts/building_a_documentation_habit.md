;;;
{
	"title": "Building a Documentation Habit",
	"description": "Documentation is great when it's great and terrible when it's not, but a habit to document, review, and improve is a good habit for a team.",
	"date": "19 March 2025",
	"contents": false,
	"hero": "photo-1457369804613-52c61a468e7d",
    "topics": ["How-To", "Processes", "Learning"],
    "related": [
		{ "title": "Don't Retro the Same Twice", "description": "Different retrospective formats are mostly the same thing in different flavors - don't argue about them; try all the flavors (at least once)", "fileName": "dont_retro_the_same_twice" },
		{ "title": "Develop Effective Coding Standards", "description": "Bad coding standards are worse than no standards, and even good standards are sometimes unnecessary. What's the utility in coding standards, and what makes a good one?", "fileName": "develop_effective_coding_standards" },
		{ "title": "Intuiting Jevon's Paradox", "description": "On the unintuitive pattern of resource consumption and how it relates to software engineering.", "fileName": "intuiting_jevons_paradox" }
    ]
}
;;;

Two weeks ago I started working with a new team at my company, and the first (real) meeting I attended with the team was a manager's attempt to start increasing the documentation the team kept. This being the manager's goal (and, being clear, I'm talking about a good manager with a good goal), this is my goal to some extent. How do we take a longstanding team and build a habit of writing documentation?

Sometimes it's not really _that_ necessary to keep up large amounts of documentation. Like code, documentation needs to be kept up-to-date, but what's more dangerous than code is that documentation can lie to us if it becomes too stale; the code always tells me exactly how it's running. The most important documentation, then, is to give a history of _why_ certain decisions were made, to explain _why_ the code looks the way it does at any point. The best time to document _why_ being when the decision happens, a longstanding team just starting to compile documentation is never going to recreate this.

I'm not claiming to be some documentation guru, but we should all understand it as one of the important outputs of our professional work, so it makes sense to instill a habit for writing documenation on any team. Building any habit on a team takes two things:

1. Constantly doing; and
2. Constantly reminding

Don't think you can call a meeting, say "let's all document now," and expect any change. With the team having decided it wants to build a habit, someone gets appointed the habit czar and keeps an eagle eye on opportunities to exercise it. Constant reminders by the habit czar, constant doing by the whole team. That is a bit too neat and tidy though, isn't it? If the habit is too burdensome it won't be adopted, if nobody likes the czar the team will probably start doing the opposite. Habits are people processes, and it takes some compassion and empathy on the team to be able to achieve.

Back to documentation, there's a couple things to insist on:

## Foundational Points

The ultimate goal is simplicity at all levels so adoption becomes effortless. Any decisions along the way should reinforce these two:

**Reduce the burden to document**

Don't worry about _where_ or _how_ documentation is happening. The team should pick one repository for documentation to go, and that's enough decisions to start. Nobody should feel restricted about where particular bits of documentation are put, how they're formatted, or the like. If you're starting from zero, then _any_ documentation is a step up.

**Constantly refactor**

Revisit documentation you've made, visit (and revisit) documents made by colleagues. Move files around, create or delete folders, move chunks from one file to another; any opportunity to add necessary context, remove cruft, and keep the whole structure tidy is good. Remember the [boyscout rule](https://deviq.com/principles/boy-scout-rule): always leave the documentation campsite cleaner than you found it.

## Practical Points

**A good search feature is necessary**

Because building the habit is our primary focus, organization takes a bit of a backseat. Yes, we refactor to improve the organization of the documentation, but that ends up moving the cheese a bit. Those are good things because they are the tools to reduce the barrier to adopting the habit, and that is the goal! Do not lose focus!

But how do we navigate the documentation? Shouldn't we impose a directory hierarchy at least to make it simpler? We could say that dev docs go in this folder, and architectural docs go in this other folder... NO! [Grug reach for club](https://grugbrain.dev/)! You're trying to [optimize prematurely](https://stackify.com/premature-optimization-evil/) and that will hurt everyone.

Instead, rely on a great search tool. I really hate Atlassian products, but they have produced one good thing: the search bar in Confluence. I've heard from colleagues that the search in Notion is also quite good. Rely on that for navigation! A singular, comprehensible organization pattern will emerge over time with colleagues engaging with each other over refactoring, let that develop naturally. Even once such a pattern is established, you're never going to know perfectly where everything is. Search is friend!

**Document at all the levels**

Documentation that only focuses on technical details, only on 10000-foot details, or only on why details is incomplete. Maybe you need or want incomplete documentation; that's perfectly fine! If there are techincal details that have been around a while - say, tribal knowledge like "don't alter this sproc otherwise Bob gets really angry" - that's good knowledge to have. Equally, basic architectural diagrams of what systems depend on which other systems is really great to have. There's so many points of good-to-have knowledge that it's impossible to enumerate here; I would say I'm barely scraping the iceberg but this is more like looking at the iceberg!

Remember that if the documentation is bunk it can be deleted later; because you're constinuously refactoring any documentation _will_ be discussed by the team and can be moved/removed. If you're unsure about the utility, put a note at the top: "Ian is unsure this is long-term useful but put it here because it looked really strange as he was looking at some issue."

Each team having different requirements in different environments, you won't know what bits of information are really the most valueable long-term but by practice. Onboarding new engineers requires lower-level documentation, while being able to communicate the cause of an issue to a VP requires higher-level documentation. Experienced practitioners will surely have developed a sixth sense about what kinds of documentation are more valuable in different sorts of contexts; this guide is for the inexperienced.

**Get outside feedback**

Which bits are useful, which aren't? Which bits aren't going to be able to be maintained, which bits are concrete? What document structures make the information clearest, which are overloaded or burdensome or the like? Any questions about the quality of the documentation or the quality of the experience of _reading_ the documentation can be best helped by a fresh pair of eyes.

I wrote a while about [using guerilla testing for DevEx](https://ian.wold.guru/Posts/guerrila_devex_testing.html), and you can do the same here for ... DocEx? Surely we can call it something better than that...

Documentation for its own sake isn't terribly useful, documentation should serve a function. When we throw up documentation that _might_ be useful for such and such reason, we'll need some way to later understand what is _actually_ useful; often interactions with the world outside the team are the best determiner of that. Did this documentation help resolve an issue raised by another team? Did Bob object to being called "angry" in the other doc? (And just to note, best practice is to not blast other colleagues in docs...)
