;;;
{
	"title": "Using Interfaces",
	"description": "I'm on a quest to make it happen less",
	"date": "8 June 2024",
	"contents": false,
	"hero": "photo-1634136145105-03a3c2455513",
    "related": [
		{ "title": "Develop Effective Coding Standards", "description": "Bad coding standards are worse than no standards, and even good standards are sometimes unnecessary. What's the utility in coding standards, and what makes a good one?", "fileName": "develop_effective_coding_standards" },
        { "title": "It's Better to be Consistently Incorrect than Inconsistently Correct", "description": "On consistency in code and what it means for something to be 'incorrect'", "fileName": "its_better_to_be_consistently_incorrect_than_consistently_correct" },
        { "title": "Just Use PostgreSQL", "description": "With a vast and growing ecosystem of database systems, data models, patterns, and paradigms, choosing the right one can be a long and complicated process. I prefer a simpler approach: Just use PostgreSQL.", "fileName": "just_use_postgresql" }
    ]
}
;;;

I've previously [written about this problem in the C# world](https://ian.wold.guru/Posts/four_deeply_ingrained_csharp_cliches.html#interfaces) that interfaces are overused. Some codebases seem to have the idea that a DI container is incapable of handling a service class if it doesn't have an interface. Other codebases define inexplicable, empty `IModel` interfaces which just serve as a handle for reflection or something. Other codebases yet have the even stranger idea that every single class should have its own interface, and then if polymorphism is required, there are interfaces on top of those!

Last I checked, which was admittedly a while ago, this is a problem in the Java world as well. To limited extents the problem exists in other interface-supporting languages like Go or TypeScript, though there are many more opportunities than just the interfaces to footgun yourself with TypeScript types. I hope that we might be able to disabuse our codebases of these faulty notions about interfaces. I'm not going to propose some sort of SOLID but for interface use, I just finished [writing about the problems of that methodology](https://ian.wold.guru/Posts/book_club_5-2024.html).

Rather, I'm going to motivate some instances when interfaces should be used, the suggestion being that if you are not encountered with one of the following then you should not use an interface. Interfaces are abstractions, and abstractions should be avoided until the codebase reveals their necessity. I'm very skeptical of so-called principles which suggest abstractions should be implemented in all cases, or indeed to be implemented before they are revealed through a first iteration of the code.

# Polymorphism

Obviously, if you need multiple implementations of the same service, you should use an interface. `PayPalPaymentService` and `GooglePayPaymentService` are probably required to share an `IPaymentService` interface.

Polymorphism is particularly useful when I need to migrate from one implementation of a service to another implementation, but I can't do it all at once. Suppose my software had always processed payments through PayPal, but our firm decided to not renew a contract and now we need to switch entirely to Google Pay. Bit of a contrived scenario, but you get the gist.

If I had only ever had one implementation of `PaymentService`, it would not need an interface, and indeed I'm suggesting that this would have been correct. Much of the time we put interfaces on services with single implementations thinking about this potential future case that we're going to need to alter implementation. Being clear, I've encountered this situtation of needing to swap implementation a lot. However, I advocate for not implementing the interface before I start doing this swap because it's easy to add the interface at that point in time. I don't know what the future brings, I don't know _which_ services are going to need implementations swapped out, so I don't want to pollute my codebase with interfaces until it becomes necessary.

Any IDE, even Vim, is going to be able to support you on this. If I have the single concrete implementation `PaymentService`, I can do a global rename of `PaymentService` to `IPaymentService`, create `IPaymentService` as a separate interface which gets a compiler error, then rename the payment service class to `PayPalPaymentService` implementing the new interface. All of my code now references the interface, and I'll only need to update the DI registration and pull public methods up into the interface. This takes 5 minutes, or no more than 30 if you have a particularly tortured codebase.

# Layer Boundaries

Almost every legitimate use of an interface is going to be because there are multiple concrete implementations of that service, but there are legitimate cases when I need to hide the implementation from components across a system. I contend that this could not possibly be anything other than a very large system which requires lots of wrangling.

The best example would be the _modular monolith_ architecture. In this architecture, the various modules (domains, areas, or the like) of the application are not allowed to reference each other. Instead, there is a shared library which provides the abstractions (interfaces) of each of the modules which they in turn reference. A central host or shell module will provide the entrypoint for the application and use DI or a similar pattern to marshal the correct implementations from each module to the others depending on their abstractions.

This pattern facilitates the individual modules to be distributed from the core monolithic application as becomes necessary, and it's a first step in implementing the [strangler pattern](https://en.wikipedia.org/wiki/Strangler_fig_pattern). It's an important pattern in making sense of very large codebases, and although it introduces a number of problems itself which do require care to resolve, I don't think that its reliance on interfaces is one of them. It's certainly overkill to use this pattern on a small or a new codebase, which leads me to suggest that implementing interfaces for services in anticipation of a future implementation of the modular monolith is improper.

The [Hexagonal architecture](https://en.wikipedia.org/wiki/Hexagonal_architecture_(software)) (AKA "onion" or "ports and adapters" architecture) is another example of using interfaces to support layer boundaries, where there is a domain boundary which all other layers reference, which contains the abstractions necessary to perform domain operations. Again here we want to be careful that this architecture only be used for codebases of an appropriate size and type, but it's inoffensive to use interfaces to support this sort of architectural style.

# All Other Scenarios

Don't use interfaces. [YAGNI](https://en.wikipedia.org/wiki/You_aren%27t_gonna_need_it). Ideally, when we see an interface in our code - when we consume one - some flashing light should be going off somewhere in our brain: this is more dangerous than consuming an implementation! I'm not suggesting anything dramatic, but consuming interfaces should be handled more diligently and with an absolute expectation of a change in underlying behavior. The casual use of interfaces causes us to forget this in a sort of boy-cried-wolf sort of way: because interfaces are ubiquitous and their implementations _don't_ actually change that much in practice, we stop caring. An interface is an abstraction, and abstractions are to be treated with respect and fear.

One objection I can anticipate is that I did not account for using interfaces to facilitate mocking in unit tests. You might be inclined towards permitting interfaces for this case, but I am not. Indeed, [I am decidedly anti-unit-test](https://ian.wold.guru/Posts/book_club-2-2024.html) but I recognize a difference in opinion exists here. I prefer a combination of integration testing and property testing, along with relegating business logic to an area of the code that doesn't have dependencies - ports and adapters is good for this!
