# Interfaces

It is rare to find a legacy C# codebase where interfaces aren't critically misused, yet interfaces are almost entirely necessary when developing even relatively simple production applications. Interfaces should be used to represent meaningful, coherent abstractions or capabilities relevant to your domain, and only used when there is a clear need for abstraction and/or polymorphism. One benefit gained by interfaces is to allow for multiple class implementations, facilitating class swapping, refactor testing, decorator pattern, or testing fakes. A (maybe better) benefit comes from the ability to assign multiple abstract concepts to a single concrete class, a la the Liskov principle.

Often though we find a one-interface-per-class rule in C# codebases, which eliminates or outright ignores a fair set of benefits. How can any of us claim our C# code is SOLID if we restrict ourselves away from even being able to use the Liskov principle? Worse yet is when we tack arbitrary interfaces on to represent abstract-concrete concepts, such as with the ever-present, ever-empty, and ever-reflected `IModel` interface.
    
## Each Class has One Interface

This sort of code is so common in a C# codebase that it's almost expected of every C# engineer to produce by default:

```csharp
public interface IMailChimpFacadeEmailService { ... }

public class MailChimpFacadeEmailService : IMailChimpFacadeEmailService { ... }
```

And there is almost never an `IEmailService` to be found! Interfaces are a specific tool with a specific purpose. If they're only mirroring the class structure, they're unnecessary. In these cases the code is better without these interfaces. If you do need to swap the class in a future refactor, you can create an interface then; it's dead weight to live in the code in the meantime. If you need to rely on the interface for mocks or fakes for unit tests, there's a good chance you're testing wrong and you should think about that too.

A legitimate use - and one that will support legitimate testing strategies - is to create an interface for the properly abstract `IEmailService` concept, then implement it with the one which calls into the MailChimp facade.

## There's Too Many Interfaces

Even in the above example, I might question the need for an `IEmailService`. Are we really using the interface to properly segregate concepts in our codebase? Are we using them to achieve a beneficial pattern, such as the decorator pattern? 9 times out of 10 the answer will be "no".

In these cases, I fall back on the Liskov principle not just as a way to think about abstractions, but also as a guide for when to create interfaces. Are these interfaces really describing meaningful, coherent, single, and abstract concepts that are relevatnt to my domain? Could multiple classes fulfil this contract, even if the classes are unrelated? These questions are useful heuristics when trying to determine this. More often than not, I think the interfaces can be thrown away. It's okay for multiple classes to not have interfaces.



# Abstractions

Misusing abstractinos itsn't a sin that's endemic to C# codebases specifically, it's across the entire industry. There's an interesting observation to be made that the incidence rate of overused abstractions tends to directly correspond with the number of software engineers involved in the team or company which owns that code - when we have time to kill at work, we tend to _work_. Abstractinos are inherently complex, and we should strive to create simple code. The trouble is that abstractions can increase the flexibility of code, so they're also essential tools. The prevailing wisdom is that abstractions should be used sparingly and when necessary, and nobody (not even me) can define when it's necessary to abstract.

## There's (Sometimes Multiple) Passthrough layers

Architectural abstractions can kill an entire codebase, especially when too many layers are introduced. One common find in C# codebases is the passthrough layer - often the repository layer. Indeed, gone are the days of the Data Access Layer, we now have the Repository Layer, named after the pattern. This layer is often a passthrough to Entity Framework (EF, like most ORMs, already implement repository pattern), to service layers which contact external services, or both. Whether it's a repository layer or not, passthrough layers are, I think universally agreed, incredibly annoying.

These layers add no value, they don't procide modulatiry, and often hinder scalability. Demonstrably, they add to development time. Kill these layers, even if they cause you to have to rethink your architectural approach.

## Abstractions are not Refactored Out

Again, the solace here is that every software engineer - not just those of us writing C# - is guilty of creating needless abstractions. If I'm trying to focus on the C# community though, I might accuse us of being reticent to refactor abstractions. When I open a new C# codebase, it feel expected that even simple, imperative concepts are broken into complex, unclear, and often too-restrictive abstractions. [insert example here]

Overall, I would say that an abstraction should clearly and demonstrably add distinct value or functionality. If you can't explain the value-add in one simple sentence, you've probably missed the mark. Architectural abstractions should specifically enhance modularity, scalability, or separation of concerns in a beneficial and articulable way.

When refactoring (which ought be a constant practice), pay particular attention to abstractions. Celebrate when you can remove an abstraction, and always prefer clear, readable, imperative/procedural code over abstractions where the benefit of the latter is tenuous.



# Exceptions
    (Overarching point: What is the purpose of an exception? Use exceptions only for exceptional, error conditions.)

    Employ regular control structures (like if-else, loops) for standard flow control.
    Consider alternative error handling strategies like return codes or validation methods for expected conditions.
    Combine exception handling with proactive measures like input validation, fail-safes, and default values.
    Use logging to track unexpected states or errors.
    Design your application to gracefully handle errors and maintain a consistent state.

## Exceptions are Used for Control Flow
    * Reasons: Exceptions are meant for exceptional, unexpected events. Using them for regular control flow (like in a loop or conditionals) is misleading and can significantly impact performance due to the cost of raising and handling exceptions.
    *Detrimental Effects: Poor performance, reduced code readability, and potential misuse of exception handling mechanisms.

## Handling an Error means Catching an Exception
    Using exceptions for error handling instead of Result pattern or other preventative strategies.
    Often leads to swallowing errors or allowing exceptions to interrupt user experience.
    * Reasons: While exceptions are a key part of error handling, relying solely on them can overlook other robust error-handling techniques like input validation or fail-safe mechanisms.
    * Detrimental Effects: Potential for unhandled exceptions, reduced application stability, and missed opportunities for preventive error handling.



# Testing
    (Overarching point: Testing needs to be approached more thoughtfully, both from a conceptual and technical standpoint)

    Focus unit tests on the expected behavior of the code rather than its internal implementation.
    Write tests that are resilient to changes in the implementation as long as the behavior remains consistent.
    Use mock objects and test doubles to isolate the unit of work and avoid dependencies on external systems or complex setups.
    Regularly conduct integration and end-to-end tests to ensure different parts of the application work together correctly.
    Use a combination of testing types (unit, integration, end-to-end) to cover different aspects and layers of the application.
    Ensure tests mimic real-world scenarios and user interactions as closely as possible.

## Unit Tests are Coupled to Implementation
    overly reliant on mocks; writing hacky unit tests to test untestable code
    * Reasons: Tests should focus on behavior rather than implementation details. Coupling tests to implementation can lead to brittle tests that break with any change in the code, even if the behavior remains correct.
    * Detrimental Effects: Increased maintenance of tests, reduced effectiveness in catching regressions, and potential for false negatives.

## Behavior Testing Isn't used
    * Reasons: Focusing solely on unit tests can miss larger issues that only appear when components interact. Behavior testing ensures that the system works as a whole.
    * Detrimental Effects: Potential for integration bugs, reduced confidence in the system’s overall reliability, and overlooked user experience issues.



# Language Features
    (Overarching point: With so many features in a language/framework, choosing which to use when requires diligence. There's no "one right way" but a lot of undocumented bad ways.)

    Use LINQ judiciously, where it improves readability and efficiency.
    Avoid using LINQ in performance-critical paths if it introduces significant overhead.
    Stay updated on LINQ best practices and performance implications to make informed decisions.
    Keep up-to-date with new C# features and understand their intended use cases.
    Experiment with new features in non-critical parts of your codebase to gain familiarity.
    Adopt new features where they improve code clarity, performance, and maintainability, while being cautious about using them inappropriately.

## Over- or Under-Utilizing LINQ
    either for too simple operations causing performance hits or for too complex operations causing unreadable/undebuggable code (and often performance hits)
    * Reasons: LINQ is powerful but can be overused, leading to less readable, less performant code. Conversely, underusing it can result in more verbose and less efficient code than necessary.
    * Detrimental Effects: Performance issues, decreased readability, and potentially missing out on concise, expressive code patterns.

## Over- or Under-Looking New Features
    * Reasons: Ignoring new features can result in missing out on improvements and more efficient ways of coding. Misusing them can lead to convoluted solutions that don’t align with the feature’s intended use.
    * Detrimental Effects: Reduced code efficiency and quality, potential for using inappropriate solutions, and missing out on language evolution benefits.