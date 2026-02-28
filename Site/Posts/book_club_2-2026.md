;;;
{
	"title": "Book Club 2/2026: Constraints",
	"description": "Taking a top-level look at the varied and nerdy topic of 'constraints'",
	"date": "28 February 2026",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Patterns", "Languages", "Standards"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 1/2026: C#, Without Allocation", "description": "Some resources on writing allocation-less (or -free) C#", "fileName": "book_club_1-2026" },
		{ "title": "Book Club 10&11/2025: .NET 10, C# 14", "description": "Another Thanksgiving, another .NET!", "fileName": "book_club_10-11-2025" },
		{ "title": "Book Club 8&9/2025: Async", "description": "Not the keyword, but asynchronous communication. It just seemed that 'Async' as a title was more interesting.", "fileName": "book_club_8-9-2025" }
	]
}
;;;

Constraints, in a very broad sense, have been on my mind this last month. I have spent, as I imagine we all have, a lot of my career wrangling systems suffering from constraint problems. I mean "constraint" in the technical sense; the data or rules of the system have not been appropriately constrained. Interestingly, the age of a system doesn't seem to correlate to systems having poorly-implemented constraints; it seems to be a skill issue mostly.

Constraints are a first-class tool we have for defining our systems. It might not seem the most intuitive at first: if we're told "the system needs to be _able_ to do X, Y, and Z," the first thought will surely be how we craft our systems _for_ X, Y, and Z. However, by saying I want the system to behave _some_ way, I'm necessarily also saying I want it to _not_ behave in contradictory ways: if A, B, or C inhibit X, Y, or Z, then I have to also constrain my system to _not_ A, B, or C. Constraints aren't _enabling_ requirements, instead they are _preventing_ errors and error states.

There's a lot of ways we can consider programming with constraints; [constraint programming](https://en.wikipedia.org/wiki/Constraint_programming) is a topic in its own right. I'm going to list a number of areas of focus I've had this last month, starting with the more esoteric.

## Constraint Logic Programming

Or CLP, this is not something with which most of us are regularly engaged. [Logic programming](https://en.wikipedia.org/wiki/Logic_programming), is its own paradigm of programming, separate to the rest, concerned with crafting programs as repositories of logical facts. A program executes in this context when a question is asked about these facts; programs are essentially interpreted by logic solving algorithms. Take this example lifted from the Wikipedia:

```prolog
is_parent(bob, sue).
is_parent(bob, john).
is_sibling(X, Y) :-
    is_parent(Z, X),
    is_parent(Z, Y).
```

And then a question to run a program:

```prolog
?- is_sibling(sue, A).
A = john
```

This is already a form of programming by constraints; I'm _constraining_ the definitions of parentage and siblingness. However, we can go more constrainey with better constraint-aware algorithms to do the solving. Hence we get [constraint logic programming](https://en.wikipedia.org/wiki/Constraint_logic_programming), which is indeed much more powerful and has practical applications. If you want to learn to program with constraints, this is the holy grail: programs are (I'll wave my hand a bit here) composed entirely of constraints.

Most problems - much more than you might think - can be very well-defined by their constraints; well-defined in the sense that the code I write makes sense for the problem. Remember that to programming constraints is to program _out_ error conditions; writing programs with CLP then is probably the best-class solution for a whole set of problems. Programming a user interface, though, is not necessarily one of those problems; HTTP clients are probably not best served by CLP also. That leaves CLP in a bit of a bind: if I want some other language bookending my whole system, why keep a separate CLP in the middle?

Aha! Here is exactly the great practicality of logic programming: because the programs are _just_ a particular application of proof-solving algorithms, these languages can be expressed perfectly naturally as libraries within other languages. [MiniKanren](https://minikanren.org/) is a lightweight DSL library with sibling implementations in most languages; C# has [uKanren.NET](https://github.com/naasking/uKanren.NET). There's plenty other libraries in most languages, and frankly it's trivially simple to implement a proof solver. [Decider](https://github.com/lifebeyondfife/Decider) is an actively maintained non-Kanren system. This gives you an engine in whichever language you fancy to express deserving logic using CLP. It's too bad that this is so often overlooked; really a huge amount of logic can be more elegantly expressed with these tools.

**Reading**

* [A practical introduction to Constraint Programming - Lorenzo Tabacchini](https://medium.com/vptech/a-practical-introduction-to-constraint-programming-2037c91833ba)
* [Learn Prolog Now!](https://lpn.swi-prolog.org/lpnpage.php?pageid=online)
* [An Introduction to Datalog - Fabien Alberi](https://blogit.michelin.io/an-introduction-to-datalog/) (curiously, hosted by Michelin)
* [Kanren Light - Marco Maggesi, Massimo Nocentini](https://minikanren.org/workshop/2020/minikanren-2020-paper5.pdf)
* [HackerNews: A year ago I asked about real-world use cases for Prolog](https://news.ycombinator.com/item?id=9248107)
* [Why Learn Prolog in 2021? - David Strohmaier](https://dstrohmaier.com/why-learn-prolog-in-2021/)

**Watching**

* [I See What You Mean - Peter Alvaro](https://www.youtube.com/watch?v=R2Aa4PivG0g)
* [Three Things I Wish I Knew When I Started Designing Languages - Peter Alvaro](https://www.youtube.com/watch?v=oa0qq75i9oc)
* [Production Prolog - Michael Hendricks](https://www.youtube.com/watch?v=G_eYTctGZw8)

## Dependent Typing

This is some crazy voodoo magic. Most programmers (especially if you're reading my blog) are familiar with type theory - maybe not at the theoretical level, but you intuit that this won't compile:

```csharp
string variable = 12
```

`string` is a type, `variable` cannot have values that aren't `string`s, and `12` is not a `string`. This is incredibly simple: a `string` is a `string` is a `string`. A more complicated type might be an array:

```csharp
var ints = new int[2];
ints = [1, 2, 3];
```

C# allows the above, but it is a curious bit of code. Why would I specify a length of 2 and then overwrite the array with a length of 3? C# allows this because the type of `ints` is just `int[]`, but it seems like the author might have intended something else. Alas, C# does not allow its types to rely on literal values. This is what _dependent typing_ is: allowing types themselves to have values. In a dependently-typed language, the above would not compile, as `ints` would be able to have type `int[2]`.

This can be an extremely powerful way of programming, which puts constraints directly in the type system. In the array example, I _constrain_ my array to only ever have length 2. Indeed, dependently-typed languages allow the programmer to apply any constraints that might be needed. So frequently we write programs expecting data to have certain shapes or properties or values; dependent typing allows us to write programs that make misrepresentations impossible, eliminating a whole class of errors.

This knowledge has less practical application. To start, dependent typing is a fundamentally functional concept; the type checking algorithms that can validate these sorts of systems rely on functional constructs. Being a more esoteric branch in the functional tradition, then, the syntaxes that implementors are drawn to are, could I say, difficult to read. [Agda](https://en.wikipedia.org/wiki/Agda_(programming_language)) will let you use any unicode character in a variable name. From the Wikipedia:

```agda
data _≤_ : ℕ → ℕ → Set where
  z≤n : {n : ℕ} → zero ≤ n
  s≤s : {n m : ℕ} → n ≤ m → suc n ≤ suc m
```

Studying this form is still hugely beneficial, as it teaches a mindset of loading assumptions into the _type system_ rather than runtime. This allows problems to be caught at compile time, and is the main tool for those looking to [make illegal states unrepresentable](https://fsharpforfunandprofit.com/posts/designing-with-types-making-illegal-states-unrepresentable/).

**Reading**

* [Dependent Types, Explained - Marty Stumpf](https://functional.works-hub.com/learn/dependent-types-explained-2e233)
* [The Future of Programming is Dependent Types — Marin Benčević](https://medium.com/background-thread/the-future-of-programming-is-dependent-types-programming-word-of-the-day-fcd5f2634878)
* [The Idris Language](https://www.idris-lang.org/index.html)
* [dris, a General Purpose Dependently Typed Programming Language: Design and Implementation - Edwin Brady](https://www.type-driven.org.uk/edwinb/papers/impldtp.pdf)
* [Dynamic Typing with Dependent Types - Xinming Ou, Gang Tan, Yitzhak Mandelbaum, and David Walker](https://www.cs.princeton.edu/~dpw/papers/DTDT-tr.pdf)

**Watching**

* [Idris: Practical Dependent Types with Practical Examples - Brian McKenna](https://www.youtube.com/watch?v=4i7KrG1Afbk)
* [A Little Taste of Dependent Types - David Christiansen](https://www.youtube.com/watch?v=VxINoKFm-S4)
* [Dependent Types: Through The Looking Glass - Owein Reese](https://www.youtube.com/watch?v=jgFAkmnBHwU)
* [Propositions as Types - Philip Wadler](https://www.youtube.com/watch?v=IOiZatlZtGU)
* [Making Dependent Types Practical - Chris Casinghino](https://www.youtube.com/watch?v=_2jrmgO_Gq0)

## Refinement Typing

While fewer folks yet are looking at refinement typing, I think there is vastly more potential practical benefit here than in dependent typing. Actually, the two dovetail. I'm incredibly excited about this area. I would love to see [refinement typing](https://en.wikipedia.org/wiki/Refinement_type) become the next big thing; most languages could probably implement this with some ease.

While dependent types allow types to depend on _values_, refinement typing allows types to depend on some pure constraint about them - not a value that could be some program variable, but some predicate function. Though, I might try to explain it another way: most languages anymore have pattern matching syntax. C# has a robust one:

```csharp
if (value is >= 0 and <= 100)
{
    //...
}
```

Although this is a large wave of the hand, I think of refinement typing as attaching some pattern matching statement onto a base type; the resulting refined type is guaranteed that this pattern will always match the value of the variable with that type. The above example matches some `int` with a pattern, perhaps we could imagine a C# with refinement types (I am _not_ proposing a syntax, mind you):

```csharp
int{>= 0 and <= 100} value = 12;
```

So long as we're not allowing runtime variables into this pattern statement, this is not a difficult task for the type checker. The power here, though, is immense: think of how many methods your code has with lots of guard clauses at the front? Example:

```csharp
public User CreateUser(
    string username,
    int age,
    string email,
    decimal accountBalance
)
{
    if (username is null or { Length: 0 })
        throw new ArgumentException("Username must be provided.", nameof(username));

    if (age is < 18 or > 120)
        throw new ArgumentOutOfRangeException(nameof(age), "Age must be between 18 and 120.");

    if (email is null or { Length: <= 3 })
        throw new ArgumentException("A valid email address must be provided.", nameof(email));

    if (accountBalance is < 0m)
        throw new ArgumentOutOfRangeException(nameof(accountBalance), "Account balance cannot be negative.");

    // Implementation
}
```

If I could move these constraints into the types, I can eliminate the guards, catch errors at compile time, and ensure that callers are guarding data appropriately:

```csharp
public User CreateUser(
    string{not null and {Length: > 0}} username,
    int{> 18 and < 120} age,
    string{not null and {Length: > 3}} email,
    decimal{> 0} accountBalance
)
{
    // Implementation
}
```

_That's_ a pretty neat deal to me.

**Reading**

Note these are all academic; there isn't a great into article I've found. Future post idea?

* [Principles and Applications of Refinement Types - Andrew D. Gordon, Cédric Fournet](https://www.microsoft.com/en-us/research/wp-content/uploads/2016/02/MSR-TR-2009-147-SP1.pdf)
* [Refinement Types - Ranjit Jhala, Niki Vazou](https://arxiv.org/pdf/2010.07763)
* [Programming with Refinement Types An Introduction to LiquidHaskell - Ranjit Jhala, Eric Seidel, Niki Vazou](https://ucsd-progsys.github.io/liquidhaskell-tutorial/)
* [Focusing on Refinement Typing - Dimitrios J. Economou, Neel Krishnaswami, Jana Dunfield](https://dl.acm.org/doi/epdf/10.1145/3610408)

**Watching**

* [An Introduction to Refinement Types - Ranjit Jhala](https://www.youtube.com/watch?v=OEdXcn1rx6k)
* [Pursuing Practical Refinement Types - Michael Perucca](https://www.youtube.com/watch?v=oYTGXNrMEho)
* [Refinement Kinds: Type-Safe Programming with Practical Type-Level Computation  - Luís Caires, Bernardo Toninho](https://www.youtube.com/watch?v=pnktXb-BY54)
* [Program Synthesis from Refinement Types - Nadia Polikarpova](https://www.youtube.com/watch?v=KZwpQIpqbf4)
* [Liquid Haskell: Verification with Refinement Types - Niki Vazou](https://www.youtube.com/watch?v=Ye0luMc4vGU)
