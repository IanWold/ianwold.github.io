The Modular Monolith Won't Save You
I must once again insist there are no silver bullets; knowing architectural patterns is no substitute for knowing how to write software.

It seems like it was just the other day that microservices were the cool thing to be doing, but as fads come and go the microservice fad has waned. What are the evangelists to evangelize now? Some have dipped their toes into the "modular monolith," which at times has both pleased and confused me.

I've been pleased that conversation has shifted to the benefits of monolithic codebases, although it would be nicer yet to replace this with conversation about the realistic drawbacks of distributed systems. I would think that monolithic code would be the default, that we wouldn't dream of distributing a system unless it developed to a point where some clear, specific principles would force the question of distributing. Maybe I'm an idealist.

That leads partially to my confusion over the new focus on the modular monolith. "Modular?" Isn't that just how monoliths - nay, any professional code - should be written? Can't it just be the _default_ that we do our best to architect and engineer a _really good_ bit of code, and that code all executed together until it absolutely can't? Oh yes, I'm definitely an idealist.

I fear that the modular monolith might take the same place that microservices had in the 2010s, with some degree of dogma, some degree of specific aesthetic concerns, and some degree of very vague rationale in their favor. Realistically, the microservices vs. modular monolith question is a false dichotomy, and this allows me to discuss the broader role of patterns and principles. Or, maybe I'm swapping out my idealism for skepticism.

# Microservices vs. Modular Monoliths

In one sense these are natural opposites - many of those generally opposed to the proliferation of the microservices pattern holding the new modular monolith pattern up as a reaction. In any other sense these are two of an infinite number of ways to think about the organization of software.

## The Rise and Fall of Microservices

Much has been written about this subject, some of which by myself on this blog, so I'll stay brief here. I can't recommend enough [Death by a Thousand Microservices by Renegade Otter](https://renegadeotter.com/2023/09/10/death-by-a-thousand-microservices.html) as a primer. "Microservices" is a term that loosely refers to a distributed system composed of dozens to hundreds of very-small independently-deployed services, each focusing on a narrow and specific domain problem. Ideally each one wholly owns its domain area and contains its own persistence.

The pattern arose at firms like Amazon and, maybe most famously, Netflix, as they attempted to tackle the most difficult distributed problems at a global scale. These firms didn't adopt this architecture for fun, nor did they do so out of an idea that it is _the_ right way to architect software (or even specifically distributed software). Rather, they adoped the architecture because they had to - there was no other way to solve planet-scale problems as theirs any other way.

Being good colleagues, the architects and engineers involved with these firms published their results for the rest of us to admire and maybe learn from. Being bad colleagues, the tech evangelists obsreving this figured they could gain a lot of money or prominence by "helping" much smaller firms and teams with much smaller problems adopt this architecture. Microservices were presented as the antidote to all the crappy legacy code we had to work with up to that point. Code which, coincidentally, was monolithic. "Monolith" was made to mean "legacy" and "spaghetti", and "microservices" were made to mean the polar opposite.

Of course, the nicest thing to say about that line of thinking is it's misguided; bad code is bad code, but bad code that is _distributed_ is more expensive. In the disarray of the hype, cloud providers have made a fortune, and each year now there's an increasing amount of crappy legacy code that is so much more expensive to maintain than its previous generation of crappy legacy monolithic code.

That was all as unsustainable as it sounds, and I feel confident now in saying that the microservices bubble has most certainly burst. The problem of course is that distributed systems are already hard - [really hard](https://ian.wold.guru/Series/fallacies_distributed_computing.html) - and they'll bite a team not knowing what it's doing. If pain isn't felt setting up a new, distributed system, it's certain to be felt down the line when some new business requirement [reveals the system has been coded into a corner](https://www.youtube.com/watch?v=y8OnoxKotPQ). The pendulum of excitement has been swinging in a distinctly new direction over the last couple of years.

## The Rise of the Modular Monolith

I'm not very good at history, so I'm not sure when this term came about. Maybe it's been around for 20 years, maybe it came up a few years ago when I first heard it. Whatever the case, it's maybe the most commonly-cited anti-microservices pattern. _Why_ that is, I'm unclear. I wonder if it's not partially a bit of clever marketing - microservices having been somewhat of a reaction to the perceived weakness of monolithic software, it makes sense that advocating for the monolith requires some answer to that perceived weakness.

However the case, "modular monolith" comes about and is now excitedly and widely discussed. Loosely it means a monolithic architecture that has good separation between domain boundaries. But wait, I ask: isn't that just how software is always supposed to be made - with proper separation of concerns? If this is all the "modular monolith" is, then why aren't we just saying "do a monolith but don't suck at coding?" I might prefer a step further even: if monolithic architectures should be the default then maybe we should say "don't suck at coding and don't distribute unless you really, really need to."

But that hasn't got a good ring to it so now we've got the "modular monolith," almost as though before microservices there were no well-written monoliths. And here's exactly the problem I perceive: because it needs some overarching explanation of the right way to structure software, it becomes a term that is open to being coopted into meaning something dogmatic. "Microservice" started as something incredibly different to how it ended up largely due to the widespread adoption of ill-formed ideas as to why software is the way it is.

## Which One is Better?

I'm not going to answer that. Not just because I don't want to, but because I can't. Well, also because I don't want to. Microservices are better for Amazon and Netflix and the like. If you're starting up a brand new system then a monolithic architecture is probably better. This is a false dichotomy though: these are not the only two options! In fact, you might adopt different definitions of "monolith"; consider whether you think a C# server with a standalone React frontend and a Postgres database is a monolith or a distributed system. Most systems today require _some_ kind of distribution, but it's also the case that _almost_ every single microservices system doesn't need to have a microservices architecture.

I do feel comfortable committing to the position that monolithic architectures are the proper default for new systems development, but even then I need to attach the boring caveat that "default" practices are overridable by well-considered reasoning and understanding of the system's domain. This lands me back at a more fundamental truth though: naïveté is the highest virtue when approaching a new system. Well, naïve in a smart sense, I suppose. Keep it simple; less is more; start with what you need then grow from there; so on and so forth.

If you're with me on that, then you'll probably agree it's a point in favor of the so-called modular monolith. "monolith more simple than distribute" + "more bias towards simple" = "more bias towards monolith than distribute". And look how cleverly I can continue to avoid taking a definite position! That's not due to a commitment towards noncommitment on my part; you simply _can't_ make objective statements about the quality of any pattern, their goodnesses can only be judged in relation to some system, problem, or domain.

There are plenty more foundational principles on which I feel quite comfortable taking objective positions, though. In addition to the simplicity bias, I think it's easy to say that code which is written and formatted neatly and tidily is better. At least, more professional. Code with the right balance of abstraction and modularity is better, and even that is quite a bit more definite than our architectural patterns: less is more but it grows with the scale of the domain, system, and complexity.

These principles all seem to be pointing in favor of our modular monolith pattern, but taking that step from principles to an architectural pattern that may well pan out to be some new buzzword (just like what happened to "microservices") gives me pause. Let me re-ask a question I hinted at earlier in this article: Does the "modular monolith" _just_ refer to "proper, professional coding practices also it's a monolith", or is it instead an actual architectural pattern? If it's the former it's not a pattern; "try to code good" is a universal ideal.

# Patterns Won't Save You

Increasingly I get the sense that the "modular monolith" term is being defined as something of a fad pattern. Some patterns are quite well-defined, even broad ones: if I ask you to implement dependency injection you can get to work and your result is probably not going to spark a lot of debate as to whether your code "is" or "is not" a proper example. Other patterns are poorly defined: I've never seen "single responsibility pattern" mentioned during a code review that _didn't_ spiral into debate.

Fad patterns, particularly the architectural oens, are in a weird state as regards quality of definition. We can make some definite statements about them: microservices should have lots of distributed services, blockchain systems are slow (_joking_ ... kind of), and so on. At the same time there is a general confusion, or at least lack of consensus: implementation details get fuzzy and lots of idealogues have divergent prescriptions for different problems that arise. They have a general, agreed _thrust_ that might be more aesthetic than technical, along with a large cloud of nebulous hype and half-thought-out implementation details. The microservices debacle shows that even with a concerted educational effort by our colleagues who actually know what they're doing, the nebula still wreaks havoc over plenty of systems.

## The Role of Patterns
    What are architectural patterns? Why are they used?
    They are good for:
        - Common language, point of reference
        - 10,000 foot overview
        - Abstract thinking exercises
    They are not good for specific solutions - this is the "pattern trap"

    Patterns are the artifacts of abstract problem solving. The problems are informed by real-world problems, but they are abstracted over many real-world problems.

## The Real World
    Patterns never perfectly apply to a situation
    Sometimes very small aspects of a situation can make a pattern that otherwise looks like a good fit completely unsuitable
    Real problems are complex, necessarily. They can only be solved imperfectly, you can only analyze tradeoffs for a least-worst solution
    If you solve by applying predefined patterns, you've missed the forest for the trees.
    If you take from patterns the principles of problem solving, then you can solve any problem with the same _principles_.

## Avoid the Trap
    The pattern trap is the real trap, and trends set the trap. The new trend is the modular monolith.
    - Avoidign the trap: break down modular monolith knowledge into bits, and explore the _principles_ behind each piece.

# Principles over Patterns
    - The most important thing for an engineer to develop is a strong set of fundamental principles.
    - Critical thinking: "Does this pattern fit the problem?", "What are the trade-offs?", "How can I adapt this to my specific needs?"
    - A strong sense of skepticism does one well. Default to "no"