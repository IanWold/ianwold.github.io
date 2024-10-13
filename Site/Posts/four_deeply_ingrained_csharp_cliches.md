;;;
{
	"title": "Four Deeply-Ingrained C# Cliches",
	"description": "There's a lot to love about C# and .NET, and there are some things that I don't love as much. Then there are four bad habbits that are so deeply ingrained they've become cliches within our codebases.",
	"date": "4 February 2024",
	"contents": false,
	"hero": "photo-1619468654139-877048c5a71f",
    "topics": ["Dotnet", "Standards"],
    "related": [
		{ "title": "Eight Maxims", "description": "A few principles for thoughtful software engineering.", "fileName": "eight_maxims" },
		{ "title": "My (Continuing) Descent Into Madness", "description": "It started simply enough, when I asked myself if I should try an IDE other than Visual Studio. Mere months later, I'm now using a tiling window manager. This is the story of my (continuing) descent into madness.", "fileName": "my_continuing_descent_into_madness" },
		{ "title": "Reclaim Your Agile: The One Clever Trick Agile Coaches Don't Want You to Know", "description": "What if I told you there's one trick to being able to reshape your team's development process without your company knowing it? What if I told you that you can achieve actual Agile even though you work in a Scrum firm?", "fileName": "reclaim_your_agile" }
    ]
}
;;;

There's a lot to love about C# and .NET, and there are some things that I don't love as much. Then there are four bad habbits that are so deeply ingrained they've become cliches within our codebases. Despite being obviously bad practices, their ubiquity seems to force them into our codebases in spite of our knowing better. When fighting the battle against [complexity spirit demon](https://grugbrain.dev/) it's easy to tire and allow these to slip through, perhaps justified by their disarming commonality.

It's not as though these subjects haven't been written about before, but that they are still practiced in spite of our best efforts to craft well-written code. I think it's useful to call these four out, and I want to propose alternative solutions with which I have found success in the past. Take my solutions with a grain of salt if you will, but do have a think about a better alternative yourself. These cliches have plagued too many codebases, creating spaghettiable (is that a word?) code and exposing too many bug vectors.

# Interfaces

Interfaces are critically overused _and_ misused. When I open a C# codebase I can guarantee I'll be treated to a whole host of interfaces that have exactly one implementation. Maybe they'll also be mocked in 200 different unit tests. I know I'll be treated to files that have classes so simple they haven't changed in five years, yet there's an interface right at the top for this class exposing the single public method. I know I'll find at least one (usually many) empty interfaces, and I'm always on the edge of my seat to find how much runtime reflection there is referencing the empty `IModel`.

What's going wrong here? I think we've forgotten the utility that interfaces have for us, yet interfaces are almost entirely necessary when developing even relatively simple production applications. Interfaces should be used to represent meaningful, coherent abstractions or capabilities relevant to your domain, and only used when there is a clear need for abstraction and/or polymorphism. One benefit gained by interfaces is to allow for multiple class implementations, facilitating class swapping, refactor testing, decorator pattern, or testing fakes. A (maybe better) benefit comes from the ability to assign multiple abstract concepts to a single concrete class, a la the Liskov principle.

Often though we find a one-interface-per-class rule in C# codebases, which eliminates or outright ignores a fair set of benefits. How can any of us claim our C# code is SOLID if we restrict ourselves away from even being able to use the Liskov principle? Sure, we don't need to adhere to Liskov religiously, but it's a good heuristic to help judge when to use an interface. If they're only mirroring the class structure, they're unnecessary. In these cases the code is better without these interfaces. If you do need to swap the class in a future refactor, you can create an interface then; it's dead weight to live in the code in the meantime. If you need to rely on the interface for mocks or fakes for unit tests, there's a good chance you're testing wrong and you should think about that too.

As an example, consider the following example which we have each encountered several thousand times:

```csharp
public interface IMailChimpFacadeEmailService { ... }

public class MailChimpFacadeEmailService : IMailChimpFacadeEmailService { ... }
```

And there is almost never an `IEmailService` to be found! In this case we're usually better just deleting `IMailChimpFacadeEmailService`, but in the cases where the interface abstraction is necessary then replacing `IMailChimpFacadeEmailService` with `IEmailService` is entirely appropriate.

Empty interfaces should be deleted outright. But how will we reflect on our models without an empty `IModel` interface? Well, we probably shouldn't reflect on them, especially not after startup time. A lot of legitimate uses can be replaced by source generators here. These empty interfaces really grind my gears; they pollute my code and make me sad.

# Abstractions

Overusing abstractions isn't a sin that's endemic to C# codebases specifically; it's across the entire industry. There's an interesting observation to be made that the incidence rate of overused abstractions tends to directly correspond with the number of software engineers involved in the team or company which owns that code - when we have time to kill at work, we tend to _work_. Abstractions are inherently complex, and we should strive to create simple code. The trouble is that abstractions can increase the flexibility of code, so they're also essential tools. The prevailing wisdom is that abstractions should be used sparingly and when necessary, and nobody (not even me) can define when it's necessary to abstract.

One common find in C# codebases is the passthrough layer - often the repository layer. Indeed, gone are the days of the Data Access Layer, we now have the Repository Layer, named after the pattern. This layer is often a passthrough to Entity Framework (EF, like most ORMs, already implements the repository pattern), to service layers which contact external services, or both. Whether it's a repository layer or not, passthrough layers are, I think universally agreed, incredibly annoying.

These layers add no value, they don't provide modularity, and often hinder scalability. Demonstrably, they add to development time. Kill these layers, even if they cause you to have to rethink your architectural approach.

Overall, I would say that an abstraction should clearly and demonstrably add distinct value or functionality. If you can't explain the value-add in one simple sentence, you've probably missed the mark. Architectural abstractions should specifically enhance modularity, scalability, or separation of concerns in a beneficial and articulable way. When refactoring (which ought be a constant practice), pay particular attention to abstractions. Celebrate when you can remove an abstraction, and always prefer clear, readable, imperative or procedural code over abstractions where the benefit of the latter is tenuous.

# Exceptions

There's a lot that's been written about the ills of exceptions, and I think the negativity associated with them is maybe a bit exaggerated. One concept which I think is overlooked is the [two-pronged error model](https://joeduffyblog.com/2016/02/07/the-error-model/); that there are some errors which need to be handled immediately, and then others which require an interruption to the control flow. The fact of the matter is that exceptions satisfy the use cases for the latter - the truly _exceptional_ scenarios. We get into trouble however when we start relying on exceptions for all of our error handling.

The blame for this one definitely falls first on the C# language - it really only natively supports exceptions and there's still not a great way to create and consume a robust result or option monad (discriminated unions when). That said, one of the variants of the result pattern are better for the former sort of error. If you can encapsulate the result of your operations - particularly your data access and business operations - in such a way that the resulting value can't be accessed without a success check, then you'll be guarding the consuming code against abusing your method - you'll be making your contract more explicit.

Here's a simple result object for C# that uses the try pattern to do this:

```csharp
public class Result<T>
{
    private bool isSuccess;
    private T? value;

    private Result(bool isSuccess, T? value)
    {
        _isSuccess = isSuccess
        _value = value;
    }

    public bool IsSuccess([NotNullWhen(true)]out T? result)
    {
        result = value;
        return isSuccess;
    }

    public static Result<T> Success(T value) => new(true, value);

    public static Result<T> Failure() => new(false, default);
}
```

I can't consume the value unless I make a check now:


```csharp
var itemResult = GetItem();
if (!itemResult.IsSuccess(out var item))
{
    // handle error case
    return;
}

item.DoSomething();
```

The other bad aspect of exceptions comes when we start using them for situations that aren't even errors. It takes very little ingenuity - disappointingly little, even - to adapt exceptions as Malbolge's new `goto`. It isn't uncommon to see this in codebases, especially in library or utility logic, where some non-happy-path, yet non-error, scenario will throw, expecting to be caught _somewhere_. Sometimes this is code that merely lacks the most basic consideration (say, an email validator that throws if the string is empty instead of returning the validation error).

Other times this is entirely intentional, given very careful consideration in the entirely wrong direction. A great example is the `IDataReader` from `Microsoft.SqlClient` - if I want to access a column that doesn't exist (i.e. `dataReader["some_column"]`) it throws! Maybe that would made sense if they implemented a `TryGetValue` or `ContainsColumn`, but they did neither. The problem is so onerous because the contract for this object is that I am _supposed_ to use try/catch here to poll for maybe-extant columns.

# Unit Tests

I'll admit up front here that I'm a unit test hater. Always have been, and aggressively moreso every single time that I have to rewrite 20 unit tests because I just did a minor, non-behavior-altering refactor. Unit tests, as they're currently practiced, are in their ideal form a way to isolate a single method (or "unit") of code to ensure its contract is respected. There's a lot wrong with this.

First of all, our unit tests (almost) never live up to this. And it's not a question here of letting perfect be the enemy of good enough, they're so completely far away from this supposed ideal that I don't think it's fair to say they're even _trying_ to aspire to it. We find unit tests heavily reliant on mocks (or fakes if we're lucky) where methods that don't even make sense to test in isolation are stood up in an almost Frankensteinian manner to prod it with ill-figured test scenarios.

On top of that, it's rare to find a test that even tests the properties of these methods appropriately. Often times, these tests don't even seem to know what the properties of the methods are they need to test for (a stark contrast to the much more focused discipline of [property testing](https://matthewtolman.com/article/what-is-property-testing)).

More troubling than either of those is that isolating methods is usually not the best way to test an application. The majority of methods in a business application do not need isolated testing. Rather, the system as a whole needs testing. This ideal of isolating methods to ensure that they adhere to the contracts they establish loses the forest for the trees; contracts can and ought be enforced in the code itself.

This all culminates in the extremely common deception that our codebases are robust because we have 90% code coverage with thousands of tests that are severely abusing mocks and checking arbitrary input and output values, or call patterns worse yet! This leads to our codebases being so fragile that even small changes require several unnecessary changes across dozens of useless unit tests. And it's entirely missing the point to suggest that this is because unit tests aren't being used correctly. The definition I provided earlier is inherently flawed and leads towards code being developed this way. Mocks or fakes are necessary to set up most classes for testing, and this practice leads to huge swathes of tests which are coupled to the implementations they test.

You'll be interested to learn then that this definition of a unit test that is so ubiquitous across our industry is entirely wrong. It's a great misunderstanding of the original intent of the "unit test". I'll link [this brilliant talk by Ian Cooper](https://www.youtube.com/watch?v=EZ05e7EMOLM) as an in-depth explanation, but it suffices to say that the original intent of the "unit" test was that a "unit" represented some portion of the system which was behaviorally isolated. Perhaps we should be easy on ourselves for having mistaken that to mean "method" but after enduring how many unit test refactors, my charitability is thin.

Proper behavior tests are sorely underused, often not at all. It's quite typical that codebases will have maybe 95% unit testing and 5% proper behavior testing. This is backwards - when we're developing LOB or product software, the behavior is the most important, not the implementation. The tests _need_ to be divorced from the implementation in order to properly isolate and test behavior.

# Conclusion

tl;dr:

* Interfaces are overused and have become almost like header files, mindlessley mirroring the class structures in our codebases. Consider the Liskov principle when creating interfaces, and best yet only use interfaces when they're helping you fulfil a pattern or a meaningful abstraction.
* On the topic of abstractions, they tend to be used too frequently for cases which don't require abstraction. Sometimes it's difficult to see these abstractions enter the codebase over several PRs, so refactor your code frequently and aim to reduce abstractions every time. Prefer writing straightforward imperative or procedural code when possible.
* Exceptions are often used for any kind of error handling and sometimes as extra special `goto`s. Only use exceptions for exceptional conditions that require you to break control flow, and consider using some form of the result pattern for error handling.
* Unit tests are the work of the devil; they tend to lead to tests coupled to the implementations they test. Most applications can (and should) be entirely tested with behavioral (integration, e2e) tests instead of unit tests. When you do need to isolate discrete methods or classes for testing, consider whether patterns like property testing would better test the behavior of the method.

These cliches lead to code of a lower quality. Sometimes they just make it a bit more tedious to add a feature or change a bit of business logic. Other times, often over time, they create mangled codebases where making progress is like swimming through molasses. Though these are common practices in C# codebases, they don't need to be. I've suggested several alternatives that I've found to be very well worth considering, but you might have your own alternatives instead. What's most important is that we can stop letting these four cliches into our code, and start doing _something_ more robust.
