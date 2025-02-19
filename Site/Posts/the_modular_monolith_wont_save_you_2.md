;;;
{
	"title": "The Modular Monolith Won't Save You",
	"description": "I must once again insist there are no silver bullets; knowing architectural patterns is no substitute for knowing how to write software.",
	"date": "19 February 2024",
	"contents": true,
	"hero": "photo-1613396276557-57ba407006f9",
    "topics": ["Processes", "Distribution", "Architecture", "Industry", "Patterns"],
    "related": [
		{ "title": "The Art of Hype-Driven Development", "description": "The five simple steps to embracing and weilding the hype cycle for drudgery and someone else's profit.", "fileName": "the_art_of_hype_driven_development" },
		{ "title": "Reclaim Your Agile: The One Clever Trick Agile Coaches Don't Want You to Know", "description": "What if I told you there's one trick to being able to reshape your team's development process without your company knowing it? What if I told you that you can achieve actual Agile even though you work in a Scrum firm?", "fileName": "reclaim_your_agile" },
		{ "title": "It's Better to be Consistently Incorrect than Inconsistently Correct", "description": "On consistency in code and what it means for something to be 'incorrect'", "fileName": "its_better_to_be_consistently_incorrect_than_consistently_correct" }
    ]
}
;;;

It seems like it was just the other day that microservices were the cool thing to be doing, but as fads come and go the microservice fad has waned. What are the evangelists to evangelize now? Some have dipped their toes into the "modular monolith," which at times has both pleased and confused me.

I've been pleased that conversation has shifted to the benefits of monolithic codebases, although it would be nicer yet to replace this with conversation about the realistic drawbacks of distributed systems. I would think that monolithic code would be the default, that we wouldn't dream of distributing a system unless it developed to a point where some clear, specific principles would force the question of distributing. Maybe I'm an idealist.

That leads partially to my confusion over the new focus on the modular monolith. "Modular?" Isn't that just how monoliths - nay, any professional code - should be written? Can't it just be the _default_ that we do our best to architect and engineer a _really good_ bit of code, and that code all executed together until it absolutely can't? Oh yes, I'm definitely an idealist.

I fear that the modular monolith might take the same place that microservices had in the 2010s, with some degree of dogma, some degree of specific aesthetic concerns, and some degree of very vague rationale in their favor. Realistically, the microservices vs. modular monolith question is a false dichotomy, and this allows me to discuss the broader role of patterns and principles. Or, maybe I'm swapping out my idealism for skepticism.

# Microservices vs. Modular Monoliths

In one sense these are natural opposites - many of those generally opposed to the proliferation of the microservices pattern holding the new modular monolith pattern up as a reaction. In any other sense these are two of an infinite number of ways to think about the organization of software.

## The Rise and Fall of Microservices

Much has been written about this subject, some of which by myself on this blog, so I'll stay brief here. I can't recommend enough [Death by a Thousand Microservices by Renegade Otter](https://renegadeotter.com/2023/09/10/death-by-a-thousand-microservices.html) as a primer. "Microservices" is a term that loosely refers to a distributed system composed of dozens to hundreds of very-small independently-deployed services, each focusing on a narrow and specific domain problem. Ideally each one wholly owns its domain area and contains its own persistence.

The pattern arose at firms like Amazon and, maybe most famously, Netflix, as they attempted to tackle the most difficult distributed problems at a global scale. These firms didn't adopt this architecture for fun, nor did they do so out of an idea that it is _the_ right way to architect software (or even specifically distributed software). Rather, they adoped the architecture because they had to - there was no other way to solve planet-scale problems as theirs any other way.

Being good colleagues, the architects and engineers involved with these firms published their results for the rest of us to admire and maybe learn from. Being bad colleagues, the tech evangelists observing this figured they could gain a lot of money or prominence by "helping" much smaller firms and teams with much smaller problems adopt this architecture. Microservices were presented as the antidote to all the crappy legacy code we had to work with up to that point. Code which, coincidentally, was monolithic. "Monolith" was made to mean "legacy" and "spaghetti", and "microservices" were made to mean the opposite.

Of course, the nicest thing to say about that line of thinking is it's misguided; bad code is bad code, but bad code that is _distributed_ is more expensive. In the disarray of the hype, cloud providers have made a fortune, and each year now there's an increasing amount of crappy legacy _distributed_ code that is so much more expensive to maintain than its previous generation of crappy legacy _monolithic_ code. The architectures are neither the problem nor the solution; it's the code.

That was all as unsustainable as it sounds, and I feel confident now in saying that the microservices bubble has most certainly burst. The problem of course is that distributed systems are already hard - [really hard](https://ian.wold.guru/Series/fallacies_distributed_computing.html) - and they'll bite a team not knowing what it's doing. If pain isn't felt setting up a new, distributed system, it's certain to be felt down the line when some new business requirement [reveals the system has been coded into a corner](https://www.youtube.com/watch?v=y8OnoxKotPQ). The pendulum of excitement has been swinging in a distinctly new direction over the last couple of years.

## The Rise of the Modular Monolith

I'm not very good at history, so I'm not sure when this term came about. Maybe it's been around for 20 years, maybe it came up a few years ago when I first heard it. Whatever the case, it's maybe the most commonly-cited anti-microservices pattern. _Why_ that is, I'm unclear. I wonder if it's not partially a bit of clever marketing - microservices having been somewhat of a reaction to the perceived weakness of monolithic software, it makes sense that advocating for the monolith requires some answer to that perceived weakness.

However the case, "modular monolith" comes about and is now excitedly and widely discussed. Loosely it means a monolithic architecture that has good separation between domain boundaries. But wait, I ask: isn't that just how software is always supposed to be made - with proper separation of concerns? If this is all the "modular monolith" is, then why aren't we just saying "do a monolith but don't suck at coding?" I might prefer a step further even: if monolithic architectures should be the default then maybe we should say "don't suck at coding and don't distribute unless you really, really need to."

But that hasn't got a good ring to it so now we've got the "modular monolith," almost as though before microservices there were no well-written monoliths. And here's exactly the problem I perceive: because it needs some overarching explanation of the right way to structure software, it becomes a term that is open to being coopted into meaning something dogmatic. "Microservice" started as something incredibly different to how it ended up largely due to the widespread adoption of ill-formed ideas as to why software is the way it is.

## Which One is Better?

I'm not going to answer that. Not just because I don't want to, but because I can't. Well, also because I don't want to. Microservices are better for Amazon and Netflix and the like. If you're starting up a brand-new system then a monolithic architecture is probably better. This is a false dichotomy though: these are not the only two options! In fact, you might adopt different definitions of "monolith"; consider whether you think a C# server with a standalone React frontend and a Postgres database is a monolith or a distributed system. Most systems today require _some_ kind of distribution, but it's also the case that _almost_ every single microservices system doesn't need to have a microservices architecture.

I do feel comfortable committing to the position that monolithic architectures are the proper default for new systems development, but even then I need to attach the boring caveat that "default" practices are overridable by well-considered reasoning and understanding of the system's domain. This lands me back at a more fundamental truth though: naïveté is the highest virtue when approaching a new system. Well, naïve in a smart sense, I suppose. Keep it simple; less is more; start with what you need then grow from there; so on and so forth.

If you're with me on that, then you'll probably agree it's a point in favor of the so-called modular monolith. "monolith more simple than distribute" + "more bias towards simple" = "more bias towards monolith than distribute". And look how cleverly I can continue to avoid taking a definite position! That's not due to a commitment towards noncommitment on my part; you simply _can't_ make objective statements about the quality of any pattern, their goodnesses can only be judged in relation to some system, problem, or domain.

There are plenty more foundational principles on which I feel quite comfortable taking objective positions, though. In addition to the simplicity bias, I think it's easy to say that code which is written and formatted neatly and tidily is better. At least, more professional. Code with the right balance of abstraction and modularity is better, and even that is quite a bit more definite than our architectural patterns: less is more but it grows with the scale of the domain, system, and complexity.

These principles all seem to be pointing in favor of our modular monolith pattern, but taking that step from principles to an architectural pattern that may well pan out to be some new buzzword (just like what happened to "microservices") gives me pause. Let me re-ask a question I hinted at earlier in this article: Does the "modular monolith" _just_ refer to "proper, professional coding practices also it's a monolith", or is it instead an actual architectural pattern? If it's the former it's not a pattern; "try to code good" is a universal ideal.

# Patterns Won't Save You

Increasingly I get the sense that the "modular monolith" term is being defined as something of a fad pattern. Some patterns are quite well-defined, even broad ones: if I ask you to implement dependency injection you can get to work and your result is probably not going to spark a lot of debate as to whether your code "is" or "is not" a proper example. Other patterns are poorly defined: I've never seen "single responsibility pattern" mentioned during a code review that _didn't_ spiral into debate.

Fad patterns, particularly the architectural ones, are in a weird state as regards quality of definition. We can make some definite statements about them: microservices should have lots of distributed services, blockchain systems are slow, and so on. At the same time there is a general confusion, or at least lack of consensus: implementation details get fuzzy and lots of ideologues have divergent prescriptions for different problems that arise. They have a general, agreed _thrust_ that might be more aesthetic than technical, along with a large cloud of nebulous hype and half-thought-out implementation details. The microservices debacle shows that even with a concerted educational effort by our colleagues who actually know what they're doing, the nebula still wreaks havoc over plenty of systems.

## The Role of Patterns

We certainly shouldn't understand architectural patterns as being too wishy-washy to be useful. Yes, they don't have definite prescriptions regarding implementation, but architecture sits around the implementation details a lot of the time anyway. These sorts of patterns give us a common language at a 10,000-foot view. For all the folly, if in a conversation I say "microservices" my interlocutor will get a good sense that the following conversation is going to involve topics of distribution, infrastructure, domain separation, and a lot of complaining about patterns!

Patterns are good pedagogical tools too. In spite of the fact that university educations are typically poor preparations for professional software engineers, any serious course needs some way to give a name to different concepts discussed. Students need easy ways to be able to distinguish between broad concepts, and patterns give that sort of naming foundation.

Similarly, patterns are useful for those of us who have been in the industry for a bit of time. Seeing as things change so frequently as they do, I don't only need some way to be able to latch on to new concepts, but also a way to apply those new concepts in my considerations at work. Patterns define a space where we can explore exercises in abstract thinking about problems, which is crucial to being able to integrate new thinking into our work.

These (and maybe a couple others I've missed) constitute the proper utility of architectural patterns. None of them have anything to do with the implementation of systems, as none of them are capable of describing an implemented system. They're abstract and exist for the benefit of our thinking and communication, not for the benefit of our fingers-on-keyboards work. These patterns are informed by real-world problems, and in turn real-world solutions are informed by them, but "informed" doesn't do any heavy lifting for us.

## The Real World

The real world is a world that has a lot of variables in it. When we're considering how to implement a real system we've got (off the top of my head) business requirements, quality attributes, specific infrastructures or cloud providers, performance targets, security of both the system and the environment it'll be working in, the hopes and dreams of managers, particular egos within a team, competitions with other deliverables for the firm, technical complexity, and the degree of understanding of the domain or solution. Surely, there's dozens more.

These variables all present requirements your solution may need to fill, and don't roll your eyes at "egos within a team!" I'm quite serious about that one - keeping up development at a sustainable pace means keeping everyone happy. We're engaged from 8 to 5 in doing implementation, so details about those implementations affect our contentedness. Egos are an often-overlooked variable. At the same time though don't necessarily take it as being on par with "business requirements" - _that_ is a huge hole itself filled with many variables.

The broader point is that the patterns account for a small set of requirement variables and propose a solution to satisfy _those_. Variables which aren't accounted for will alter the implementation of the pattern or maybe make it irrelevant. Maybe your business requirements _do_ contain the set of factors that are addressed by some pattern, but also include an extra one which makes the pattern unsuitable.

There's no formula to apply here, and there's (as best I can tell) no way to teach how to navigate discovering a proper implementation for a set of requirements. There's only experience and diligence, and constant learning. Our patterns only help us with that third point. If you take from patterns the principles of problem solving, then you can solve any problem with the same _principles_. However, if you only satisfy requirements by applying predefined patterns, you've missed the forest for the trees.

## Avoid the Trap

With this understanding, let me ask: how many of the ill effects of microservices implementations are ultimately attributable to a misunderstanding of the roles of patterns? How frequently has it been that some demonstration of microservices at a pattern level has found its way into our codebases? The problem might not be the pattern, it might be _us_. Both the "us" that becomes lazy when considering implementing a real system, and also the "us" that becomes lazy when teaching or mentoring. These are two sides of the same coin: the misunderstanding of the role of patterns gives way to a confusion of the pattern for an implementation prescription. This is the "pattern trap."

If the recent buzz around the modular monolith resembles the same follies as that around the microservice, are we falling into the same trap again? If I wanted to be cynical I might throw my arms up in despair that our industry is eternally captive to hype. There's plenty of indication that a similar thing is happening - popular teachers are lazily making prescriptions from a loose understanding of a pattern and we're all not immune from a bout of laziness while inflicting those prescriptions upon our own code.

I'll go back to the most hilarious aspect of this all (at least, hilarious to me): I still don't think that "modular monolith" means anything! Take a gander over to [Wikipedia on 'modular programming'](https://en.wikipedia.org/wiki/Modular_programming) - "modular" isn't some ultra-sophisticated way of handling domain separation; it's just using classes. Or packages, or "namespaces", or whatever your language supports. You're already doing it! Monoliths _are_ "modular." What gives?

This has potentially alarming implications. A survey of articles about the "modular monolith" offer a wide array of interpretations about what work "modular" is doing in that name, many of which incompatible with each other. One writer might take it to mean using [DDD](https://en.wikipedia.org/wiki/Domain-driven_design) and another might take it to mean writing each module in a way that it might easily be separated out into a distributed service. These are wildly different.

One of the main problems with "microservices" was that the term came to mean less and less over time. It started as a very specific pattern describing architecture and practice, and over the years evangelists watered down and confused the meaning to the point that the popular understanding of the term became something like "vaguely distributed system and also the word 'event' should probably be used." The end result was to create an environment for a hefty set of poorly-considered and many times contradictory prescriptions to take hold. These became implemented on enough codebases that the popular zeitgeist now has a bad taste for the word "monolith."

And to think - "modular monolith" has _started off_ not meaning anything! The pattern trap is a real trap, and trends tend to set these traps. There's plenty of new trends constantly about, and the modular monolith is one of them.

# Principles over Patterns

None of this is to say that any pattern should be avoided, and that includes writing on both microservices and modular monoliths. The point is how we understand them, and how those come to inform our implementations. The role these patterns have is not one of prescribing solutions; they're intellectual exercises. Studying patterns will, hopefully, help one develop ways to consider problem solving in the face of different sorts of requirements. They're a _component_ of developing principles to guide our approach to writing software. Principles which, if you're doing things right, will change over time.

The real requirements of our systems are sometimes contradictory even, and this frequently makes us have to choose least-worst options. Often, this even results in a contradiction of principles we hold. As a result, we engineers are frequently engaged in a process of reconciling these contradictions, and it's that process of working through contradictions that causes us to emerge with solutions to our problems, implementations of our systems, and hopefully new principles we can keep going forward.

The systems we actually implement are synthesized from the requirements and contradictions of the multitude of real variables surrounding them. The _actual_ design of our systems will never follow 1-to-1 how microservice or modular monolith or any other pattern might describe. To implement a system we need to have the skills to be able to construct it based on the real requirements it has, not based on the ideal or ephemeral assumptions of the patterns.
