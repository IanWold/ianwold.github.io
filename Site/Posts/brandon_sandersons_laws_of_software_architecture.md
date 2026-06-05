;;;
{
	"title": "Brandon Sanderson's Laws of Software Architecture",
	"description": "You're either clicking this because you know who Brandon Sanderson is and you're surprised he has laws of software architecture, or you know what software architecture is and you're curious what prescriptions this Sanderson guy has for it. Either way my clickbait title will succeed in getting you to read it hahaha",
	"date": "5 June 2026",
	"contents": false,
	"hero": "photo-1593115057322-e94b77572f20",
    "topics": ["Architecture"],
    "related": [
		{ "title": "How do you Enforce Architectural Decisions?", "description": "Depending on what you mean by decisions, enforcing, and architecture, this question can go a lot of different ways. Most are probably unhelpful.", "fileName": "how_do_you_enforce_architectural_decisions" },
		{ "title": "The Modular Monolith Won't Save You", "description": "I must once again insist there are no silver bullets; knowing architectural patterns is no substitute for knowing how to write software.", "fileName": "the_modular_monolith_wont_save_you" },
		{ "title": "Three Laws", "description": "Some 'folk laws' that are commonly known but seldom applied.", "fileName": "three_laws" }
    ]
}
;;;

Just hear me out on this one: software architecture is like magic in fantasy novels. Not perfectly, and maybe they only bear a passing resemblance, but so too do ERD diagrams and building blueprints. I will not explicitly propose that we retitle architects as wizards, but I also won't stop you from reading that into my post here.

Magic systems are created by authors of fantasy novels to provide a foundation for the narrative and parameterize how magic solves problems within the text. Corollarily, software architectures are created by engineers of applications or systems to provide a foundation for the solution and parameterize how architecture solves problems within the code.

I get that any two disparate concepts can be molded into being analagous on a whim; the point of drawing an analogy isn't to assert _that_ the analogy exists, but to use the analogy as a tool to learn about its subject. How can this particular analogy teach us about software architecture? Apart from that software engineers are probably disproportionately well-versed in the fantasy genre, there has been quite a bit written about magic systems from the perspective of _developing_ the magic system and the system's _impact_ on narrative; carrying these aspects across the analogy reveals that these speak to relatively less-well-commented-upon aspects of software architecture.

The author Brandon Sanderson might well be the most-cited understander of magic systems, to the point that even the most cursory research into narrative magic will immediately uncover his _Laws of Magic_: three laws he developed to ground the development of these magic systems. I propose that these can easily be analogously understood to apply to software architectures, and that this exercise reveals some considerations we should probably keep more at the fore in our architectures. Sanderson's Laws, which I will recontextualize below, are ([from Wikipedia](https://en.wikipedia.org/wiki/Brandon_Sanderson#Sanderson's_Laws_of_Magic)):

1. An author's ability to solve conflict with magic is directly proportional to how well the reader understands said magic.
2. Weaknesses, limits and costs are more interesting than powers.
3. The author should expand on what is already a part of the magic system before something entirely new is added, as this may otherwise entirely change how the magic system fits into the fictional world.

## 1. Our ability to solve problems with architecture is directly proportional to how well we understand said architecture

_"Well duh,"_ I hear you thinking, _"of course we need to tell our colleagues what the established architecture is!"_

Certainly, the architecture should be documented. More fundamentally though, the architecture should be _documentable_. If you need to take two weeks to slop together a 100-slide powerpoint to teach me a system's architecture, I won't understand it. Similarly, if you find a way to encode an extremely complex architecture into a short description, I still won't understand it. If your architecture can be explained in [Basic English](https://en.wikipedia.org/wiki/Basic_English) in a well-contained blog post then I can understand it.

This seems like an obvious point to make, but I find more often than not that obvious points _need_ to be made: if I don't understand your architecture, I can't use it to resolve my problems. I can't use it, I can't conform to it, I can't do anything with it.

## 2. Constraints are more helpful than prescriptions

"Use algorithm X"; "ServiceY sould be used when..."; "Implement the Z pattern if..." and so on are prescriptions. Prescriptions are necessary, to be sure. They're not guiding though, even if they seem like it. They don't establish any boundaries for the architecture, they're never universally applicable, and interestingly they don't really convey the _intent_ of the architecture.

Constraints, on the other hand - "Don't use `System.Reflection` in runtime code"; "Serializable models must be immutable" - are more powerful, applicable, and helpful. These give the real boundaries wherein the solution must fall. If I tell you that we can't use reflection at runtime, you know my _intent_ is performance.

## 3. Elaborate on existing architecture before adding new rules

Or, I could translate this one as:

* When writing code, just conform to the existing understanding of the architecture instead of trying to conjure new patterns, or
* Don't expand the arch docs until you really need to, or
* Be restrained in describing the architecture

No system needs an architecture with a thousand bullet points (remember understandability), and no set of future problems can be comprehensively predicted by any architectural prescription (remember constraint > prescription).

When you have some properties of your architecture, understand them, and let that be your architecture. You'll _know_ when it becomes too limited to be a useful problem solving tool (meaning it needs expansion), and you'll _know_ when it becomes a liability preventing appropriate solutions (meaning it needs altering or reduction). We don't need to anticipatorially add anything to the architectural canon, nor do we need to be too eager to edit it.
