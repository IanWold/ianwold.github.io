;;;
{
	"title": "It's Better to be Consistently Incorrect than Inconsistently Correct",
	"description": "On consistency in code and what it means for something to be 'incorrect'",
	"date": "15 February 2024",
	"contents": true,
	"hero": "photo-1598826739205-d09823c3bc3d",
    "related": [
        { "title": "Eight Maxims", "description": "A few principles for thoughtful software engineering.", "fileName": "eight_maxims" },
        { "title": "Reclaim Your Agile: The One Clever Trick Agile Coaches Don't Want You to Know", "description": "What if I told you there's one trick to being able to reshape your team's development process without your company knowing it? What if I told you that you can achieve actual Agile even though you work in a Scrum firm?", "filename": "reclaim_your_agile" },
		{ "title": "Just Use PostgreSQL", "description": "With a vast and growing ecosystem of database systems, data models, patterns, and paradigms, choosing the right one can be a long and complicated process. I prefer a simpler approach: Just use PostgreSQL.", "fileName": "just_use_postgresql" }
    ]
}
;;;

Anyone who has worked with me for any period of time is probably tired of hearing me say this. "It's better to be consistently incorrect than inconsistently correct." I don't say this to mean that being inconsistent is a good thing; rather, I say this whenever I need to underscore the importance of consistency, almost always with respect to a piece of code. Codebases with established conventions - which consistently adhere to these conventions - are, in my opinion and experience, easier to comprehend and maintain.

If I'm working in an app that uses the pipeline pattern to chain behaviors everywhere but then throws a random decorator pattern at me for one slice, I'm going to be confused. Maybe scared. Probably both. Either way, I'm paying a lot of attention to that area, and usually for naught. Suppose this hypothetical app has a 2-tier architecture which might genuinely be served better by decorators, as opposed to pipelines, for its use case: the engineer who checked in the decorator for the one slice probably had good reasons for doing so; it very well could be the "correct" solution for this app.

But this is where I come in with my annoying catchphrase. The app has been established as using pipelines, and pipelines are the consistent answer here. Throwing decorators in instead breaks this consistency, irrespective of the correctness of that solution. Indeed, in a lot of cases these two patterns accomplish the same thing, and the benefits gained by using one over the other are typically minimal. This is where it's more important to adhere to consistency. When I'm in this codebase, I know that I'm going to be treated to a big helping of pipeline. It's better to be consistently incorrect than consistently correct.

What makes code - especially code _style_ - correct or incorrect? The more fine-grained of _thing_ we want to call "correct" or "incorrect", the faster this distinction becomes a matter of personal preference. Pipeline vs decorator pattern is, in simple codebases, almost always a matter of preference. Maybe your debugging tools favor pipelines, maybe you'll write less code with decorators. If we're talking about a simple enough app, the distinction does not become so important as to rise above the level of consistency - pick one and be consistent. If the weight is so far in the favor of one vs the other _and_ the weight comes down to the opposite one used, then you know that what you need is a refactor, not a fractured codebase.

Maybe instead I should say "it's better to consistently adhere to established principles in a codebase in spite of your subjective feelings towards them than inconsistently applying your subjective feelings across the codebase," but that's really a mouthful. Maybe I should say "I don't care about your feelings, consistent patterns are measurable and predictable," but that comes off as uncaring. I do care about your ideas, I just don't want them making my codebase more difficult to navigate. It's better to be consistently incorrect than inconsistently correct.

This conversation inevitably brings up the debate over the utility of consistency. I think there's strong examples of universal agreement for situations where consistency is highly valued - we tend to hold naming conventions as being important for a codebase, and using a single build system for projects (in the same language) within the same repo tends to be important. Are there examples of (near) universal agreement among software engineers of consistency being unwelcome? I can think of a few examples that aren't universally held - such as individuals who feel stifled in their creativity in a certain codebase, or when it's annoying that a codebase uses a style or pattern which is perceived as "incorrect". Ah!

These are the situations though that I suggest consistency is more valuable; these are to be situations where the value judgement is subjective. What is "incorrect" is subjective, but what is "consistent" is usually measurable. If it's not perceivable, measurable, and obvious then it isn't consistency!

Now here's one situation that's important to consider: when the _consistent_ thing to do is objectively incapable of satisfying the requirement of the piece of code. There's another saying, not my own, which goes nicely along with the one that I'm explaining here: "Things that behave the same should look the same, and things that behave different should look different." I think this is hand-in-hand with my idea about consistency. If one piece of code behaves fundamentally different from another piece of code, it shouldn't look similar. The requirement for consistency does not cross this boundary. If we have a layer in our application where each class persists some data on a single database, those should all probably look and act pretty consistently. If we add a layer of classes that contact external services to perform side-effects in the system, those classes should probably look and act a bit different to the persistence layer!

To take it back to the center though, we all perceive different things as being "correct" or "incorrect" in our code. Objectively incorrect things are bugs and unserved requirements; we fix those. Most of our disagreements are over subjective ideas. We can be tempted to check in code - one class here, one method here - that imposes our subjective ideas of correctness on an unwilling codebase. We should avoid doing this; consistency in style and pattern is an important quality of our codebases.

It's better to be consistently incorrect than inconsistently correct.
