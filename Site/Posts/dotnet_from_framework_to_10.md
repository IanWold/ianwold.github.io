;;;
{
	"title": ".NET: From Framework to 10",
	"description": "Looking back at all of the new features introduced in C# and how they impact the way we develop .NET systems today.",
	"date": "21 November 2025",
	"contents": false,
	"hero": "photo-1565027476253-ce1e3113c96b",
    "topics": ["Dotnet", "Learning", "Patterns"],
    "related": [
		{ "title": "The Modular Monolith Won't Save You", "description": "I must once again insist there are no silver bullets; knowing architectural patterns is no substitute for knowing how to write software.", "fileName": "the_modular_monolith_wont_save_you" },
		{ "title": "The Art of Hype-Driven Development", "description": "The five simple steps to embracing and weilding the hype cycle for drudgery and someone else's profit.", "fileName": "the_art_of_hype_driven_development" },
		{ "title": "Using Interfaces", "description": "I'm on a quest to make it happen less", "fileName": "using_interfaces" }
    ]
}
;;;


It's been more than six years since the last major version of .NET Framework (4.8) was launched along with C# 7.3, and the .NET world has come a huge way in that time. Last week Microsoft released .NET 10 and C# 14, continuing a trend of releases that significantly rework how software is written on the .NET platform. While Microsoft, and most brand-new .NET development along with them, have been making these strides, there's still plenty of older applications still on .NET Framework; as a result, there are plenty of engineers focused on maintaining these codebases that feel increasingly disconnected with modern .NET practices.

Each year now brings a new release of .NET (that is ".NET" and not ".NET Framework" or ".NET Core") along with a new release of C#, and each of these releases is a significant lift to the libraries, language, tooling, best practices, and standards. C# has become much more accommodating (and in some cases requiring) functional programming styles, while .NET has introduced sweeping new features across all of its workloads. This huge amount of change is reflected in the ways engineers think about, architect, and productionalize applications in .NET.

I can't give a perfectly thorough explanation of _all_ the ways that .NET is different now than it was in 2019, but I do want to type up _some_ thoughts on what's going on. Each year it becomes more and more difficult for .NET Framework engineers to make the hop over to the latest .NET with the same level of productivity and familiarity. It's not just the new .NET APIs or the new C# keywords but the styles and paradigms of the development and the "why"s motivating the new changes. 10 seems like a good version number for a catch-up, so I want some writing here that can serve as a jumping-off point for .NET Framework engineers wanting to get up-to-speed with the latest toys.

It is difficult to, for any individual update, explain the precise way that the new features have resulted in a different style of development. This is particularly so since many of these changes have been made in order to accommodate development practices that had already emerged; the language and framework evolved to meet the requests of engineers that have been influenced from other sources. Thus, by giving an explanation of modern .NET practices by starting _from_ these updates, I'm missing out on the other force that is moving .NET development along, and that will need to be a future post from me. Nonetheless, I think that the majority of .NET engineers are not on this vanguard but are instead wanting to adopt these new features into their existing workflow, and this is where I see the utility of tying all these into an explanation of "modern .NET."

As a fair forewarning though, I'm largely going off of memory and some incomplete historic documentation from Microsoft to figure everything out historically so there might be some inaccuracies around the edges here. However, everything I say you can do in C# and .NET now is accurate, maybe the history is a bit wonky. Furthermore, I'm also not giong to say anything about F#. I enjoy using F# but I use C# professionally, so I want to focus on how changes to .NET and C# have affected how we do development with those two. Finally, I will not be explaining any improvements to EntityFramework because a polished dung is still a dung.

## .NET History: Framwork, Standard, and Core

I think a (very small) bit of history can help understand most of the reasons that have motivated the lion's share of changes in .NET and help disambiguate terms. .NET Framework came about ages ago to be the standard library for the (then) new C# language, and expanded and evolved quite rapidly alongside the language to support all the different kinds of applications Microsoft wanted to support. ASP was added as a web framework, WinForms (then WPF then a mess of other things) supported desktop GUIs, WCF supported SOAP, Silverlight allowed engineers to frustrate everyone who wanted to watch the 2008 Olympics, and so on.

As software engineers we understand very well that if requirements expand and change too rapidly and haphazardly, the sooner a rewrite of the system is going to be needed, but we usually try to stave off a rewrite until _something_ happens to make it necessary. What happened to .NET a bit over ten years ago is it needed to run on Linux among a host of other factors (I'm boiling it down significantly). It came to be that a rewrite and some breaking changes needed to be made.

So, Microsoft embarked on creating an alternative standard library for C#, which they called .NET Core, and released it in 2016. They also created a separate deal called ".NET Standard" to act as a standard between .NET Framework and .NET Core, guaranteeing certain similar APIs in both standard libraries to ease transitioning from one to another, or writing code that could be guaranteed to work in both libraries.

.NET Core progressed through a few versions before Microsoft quickly decided that it would be best to only develop updates for the new .NET Core going forward. In 2019 they released the last major version of .NET Framework, but also to demark the change they decided to be rid of the "Core" name as well. The last version of .NET Core, 3.1, was developed into the first version of ".NET", which they began at version 5 as the lowest major version number not common to either of its predecessors. Each year since .NET 5 was released, Microsoft has released a new major version of .NET along with a new major version of the C# language, and this period has seen the most significant updates to the framework and language.

## .NET Core: 1.0 to 3.1

The various versions of .NET Core essentially ended up serving as the ramp-up period for the new .NET development, and did not very significantly diverge from .NET Framemwork but for having full support for Linux and MacOS.

The first super duper thing introduced with it was the [dotnet CLI tool](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet) exposing actions like creating a new app, building it, restoring packages, and the like. This acts in a similar way to the compilers of most programming languages, facilitating doing C# development in a similar manner to any other language. Go has commands like `go run .`, and now .NET has `dotnet run` as well.

.NET Core 3.0 is the first interesting .NET version to me. It introduced the ability to publish an app as a single file; instead of having to ship a bunch of DLLs and support files, the C# app gets contained in one `.exe` or whatever format it is for the target OS you select.

.NET Core 3.0 shipped along with C# 8, which had a huge number of updates to the language. The most significant feature was the nullable reference types. Null was, previously, very tricky with reference types, since they always had the option to be null, requiring huge amount of null checks and lots of time lost to reference-type variables unexpectedly becoming null in prod. The solution was to allow the nullable `?` operator to be added for reference types, and a compiler directive to tell C# that it should throw warnings or errors if null checks weren't explicitly added around these.


```csharp
string? myString = ...;
PrintLength(myString); // Error: myString can be null

public void PrintLength(string s) =>
    Console.WriteLine(s.Length());
```

In the csproj, the `Nullable` property needs to be set, and now is by default:

```xml
<Nullable>enable</Nullable>
```

I'm going to dump a bunch of little things here:

One of the cooler things in C# 8 is default interface members:

```csharp
public interface IThing
{
    string GetName() => "Unnamed";
}

public class Thing : IThing {}

Console.WriteLine((new Thing()).GetName()); // Prints "Unnamed"
```

As well as "static abstract" members:

```csharp
public interface IThing
{
    static abstract string Name { get; }
}

public class Thing : IThing
{
    public static string Name => "I'm Thing"; // Required to be present to compile
}

public void PrintName<T>() where T : IThing =>
    Console.WriteLine(T.Name);
```

Switch expressions were also introduced, allowing you to treat a `switch` as a value:

```csharp
var myThing = myIntThing switch
{
    1 => "one",
    2 => "two",
    _ => "larger than two"
};
```

This supports very functional-looking development doing pattern matching on the shape of arguments in a method, though this example is admittedly very contrived:

```csharp
public bool IsEmpty<T>(IEnumerable<T> list) => list switch
{
    var l when l.Count > 0 => true,
    _ => false
}
```

Local functions are pretty significant and just support keeping appropriate scope for methods:

```csharp
public void DoThing()
{
    string GetString() =>
        "hi";

    Console.WriteLine(GetString());
}
```

I make huge use of null-coalescing assignment for lazy loading properties:

```csharp
public class Thing
{
    private MyLoadableThing? _prop;
    public MyLoadableThing Prop => _prop ??= new MyLoadableThing();
}
```

A huge quality of life improvement came with `..` as a range operator like many other languages have (this example taken from MS's docs):

```csharp
int[] numbers = [0, 10, 20, 30, 40, 50];
int start = 1;
int amountToTake = 3;
int[] subset = numbers[start..(start + amountToTake)];
```

One of the most interesting additions to the .NET world with .NET Core 3.0 was Blazor, which takes a bit of explaining for the uninitiated. For some time, there's been an amount of effort to create an assembly language for the web - Web Assembly or WASM - understood by all browsers, that can be as ubiquitously implemented as JavaScript but that is, well, assembly, instead of JS. This has resulted in a number of languages being able to target WASM as a platform separate to Windows, Linux, and so on.

Microsoft figured that with .NET Core intending to support multiple platforms that WASM should be no different, but of course _the_ use case for WASM is web apps, so Microsoft developed Blazor as both the .NET runtime for WASM as well as the framework for developing a web UI in C# for WASM.

I won't dig too much into Blazor here, that can be a whole separate thing. However, I'll point you to an open source app I have: [FreePlanningPoker.io](https://ian.wold.guru/Posts/free_planning_poker.html), which is a simple Blazor + SignalR app that you can dive into for a taste of Blazor!

## .NET 5 and C# 9

Being the first version to "consolidate" the standard library post-Framework, there wasn't too much added to the APIs here. Most significantly is probably that ASP introduced the option to self-host instead of needing to rely on IIS, making it much easier to deploy containerized.

C# 9 was released in tandem, and just like the previous release had a huge number of things included. I think the most significantly new feature in C# in the last five years was introducted in this version: records. A record is a type of class in C# that is primarily designed for immutable data; that is, a class where no properties change. These are used extensively in functional languages and the compiler can make a lot of optimizations for immutable data when you use them.

```csharp
public record Person(string Name, int Age);
```

This is _kind of_ equivalent to:

```csharp
public class Person
{
    public string Name { get; private set; }

    public int Age { get; private set; }

    public Person(string name, int Age) { /* assign props */ }
}
```

Except the Name and Age properties can never be modified (either from inside or outside the record). Further, records have deep value equality, so:

```csharp
var first = new Person("John", 30);
var second = new Person("John", 30);
Console.WriteLine(first == second); // Outputs true
```

Functional styles with immutable records will typically assign new values when record data needs to change, and C# supports this pattern now with the `with` keyword:

```csharp
Person Birthday(Person person) =>
    person with { Age = person.Age + 1 };
```

The other really awesome change is a quality of life improvement that Program.cs does _not_ need a class, Main method, or even a namespace now. The following will compile as a valid C# program:

```csharp
Console.WriteLine("Hello, World!");
```

## .NET 6 and C# 10

.NET 6 started seeing some new things introduced in the API level. Most notably though is MAUI, "modern application UI", which was created to replace Xamarin and WPF as the cross-platform way to develop UIs in C#. This is a huge disappointment: it was buggy and incomplete on its release and it has not improved very much since.

In the ASP level, .NET 6 introduced "minimal APIs," which were developed to be able to have as easy a proof-of-concept story as other languages. For example, in Python, setting up a hello world endpoint is trivially simple:

```python
from flask import Flask

app = Flask(__name__)

@app.route("/")
def hello_world():
    return "<p>Hello, World!</p>"
```

In ASP this had previously required creating a controller class, and while that's not _difficult_, it's not super speedy feels like the future, so now we've got this as an option alongside controllers:

```
app.MapGet("/", () => "Hello, World");
```

Which is actually astonishing for rapid development. Kudos here. Other great thing: lots of Open Telemetry support. I just use the regular OTEL added with the templates nowadays so I'm kind of dumb as to what specifically was done here, but it was done.

On the C# side, the super cool deal everyone was talking about was file-scoped namespaces. Now, instead of:

```csharp
namespace MyApp
{
    class MyClass
    {
        // ...
    }
}
```

Do this:

```csharp
namespace MyApp;

class MyClass
{
    // ...
}
```

I refuse to read code if it is >= version 10 and does not do this. Another cool namespace thing you can do:


```csharp
global using WhateverNamespace; // now no other file needs "using WhateverNamespace;"
```

And then the functional support is extended through pattern matching with "property patterns", this from MS's docs:

```csharp
if (e is MethodCallExpression { Method.Name: "MethodName" })
```

This can be a bit unintuitive at first, but it fits right in line with how a lot of languages with better pattern matching support do. Pattern matching is hugely important for functional patterns, and can be kind of fun:

```csharp
public abstract record Shape;

public record Circle(double Radius) : Shape;
public record Rectangle(double Width, double Height) : Shape;

public static double Area(Shape shape) => shape switch
{
    Circle { Radius: var r } => Math.PI * r * r,
    Rectangle { Width: var w, Height: var h } => w * h,
    _ => throw new ArgumentException("Unknown shape") // required for exhaustiveness in the switch expression
};
```

## .NET 7 and C# 11

I remember .NET 7 being very cool, but looking back at the historical documentation I can't really remember this happening. If I'm remembering correctly, .NET 7 came out a little over a year after I started at Crate & Barrel, and I had upgraded some brand new microservices from .NET 5 to .NET 7 right then because I could. What a deal!

The really cool thing here was you can now choose to have .NET generate a Dockerfile when you publish your app, instead of just compiling it. This makes it so you don't need to hand-write the Dockerfile, which is a really nifty feature and has supported being able to debug directly in Docker from Visual Studio. Maybe Code also. I don't use it very much but no doubt it's nifty.

This was also the year that [Stephen Toub's annual performance improvement report](https://devblogs.microsoft.com/dotnet/performance_improvements_in_net_7/) became the talk of the industry. He'd been writing these for some time, but this one was really overwhelming at the time. To say these articles are "huge" would be an understatement as large as these articles are; they're incredibly thorough, meticulous, and technical, but a "TLDR" on the .NET 7 article sums up the cause of its impact in this particular release:

> TL;DR: .NET 7 is fast. Really fast. A thousand performance-impacting PRs went into runtime and core libraries this release, never mind all the improvements in ASP.NET Core and Windows Forms and Entity Framework and beyond. It’s the fastest .NET ever. If your manager asks you why your project should upgrade to .NET 7, you can say “in addition to all the new functionality in the release, .NET 7 is super fast.”

.NET 7's focus on performance continued past this release into each of the subsequent releases, and each year since I've observed improvements from each version bump we do. .NET gets a lot of flack for being bloaty; .NET 7 is Microsoft righting that course.

Apart from supporting dark mode for developer exception pages, ASP had a lot of small quality of life features, but it's tough to parse out a single one that's changed how development happens. By this point too, development on WinForms and WPF had largely stalled, while MAUI and Blazor made a couple steps forward but not enough for me to really enjoy them. MAUI remained half-baked and Blazor remained difficult to debug.

C# 11 brought a bunch of things that have really influenced how I do development.

The feature they called "generic math" had a huge amount of discussion on the C# language design repository, and launched to much acclaim. Essentially they had all the numeric types in C# implement `INumber<self>`, so `Integer` implements `INumber<Integer>` for example. This allows math to be done indiscriminately of whether the numeric type is `double` or `int` or whatever. From MS's docs:

```csharp
public static TResult Sum<T, TResult>(IEnumerable<T> values)
    where T : INumber<T>
    where TResult : INumber<TResult>
{
    TResult result = TResult.Zero;

    foreach (var value in values)
    {
        result += TResult.Create(value);
    }

    return result;
}
```

Very cool! I never use it, but indeed there are many applications for it.

What I do use all the time, particularly for SQL scripts, is raw string literals:

```csharp
var myString = """
    Hello,
    World
    """;
```

And you can specify how many curlies are needed for interpolation by adding `$`s:

```csharp
var hi = "Hello";
var myString = $$$$$"""
    {{{{{hi}}}}},
    World
    """;
```

File types are also something I make a lot of use of. Instead of private nested classes, `file` will limit the scope of a type to a file:

```csharp
// No longer do:
public class MyClass
{
    private record MyRecord(../);

    // Use MyRecord
}

// Now do:
file record MyRecord(...);

public class MyClass
{
    // Use MyRecord
}
```

Saves nesting, makes it easier to navigate files.

Continuing the support for pattern matching (and functional patterns by extension), this version introduced list pattern matching. You can do some wild things:

```csharp
x switch
{
    [1, 2, 3] => //matches if x is int[] { 1, 2, 3 }
    [1, 2, 3, ..] => // matches if x is int[] { 1, 2, 3, 4, etc }
    [_, _, ..] => // matches if x has at least 2 items (_ is a throwaway identifier, the values are not kept)
    [.., 2] => // matches if x ends with 2
    [ >1, ..] => // matches if the first element of x is greater than 1
};
```

## .NET 8 and C# 12

[This is the first .NET release I did a blog about!](https://ian.wold.guru/Posts/book_club_11-2023.html)

I remember this release being particularly good for ahead-of-time (AOT) compilation. .NET Core and its successor have focused on providing support for AOT compilation, which differs from the typical .NET compilation strategy of just-in-time (JIT) by compiling the entire application into native machine code, instead of compiling into IL and relying on a pre-installed .NET runtime on the target machine.

Compiling AOT obviously has more limitations as .NET is primarily designed for JIT, but it is an important feature to support the portability and, in some cases, performance requirements of many applications. After .NET 8 I became quite comfortable using AOT.

I also remember that the `System.Text.Json` library became very good in this release, as it was after .NET 8 that I replaced a lot of code that had used Newtonsoft with the new one.

This release also had a significant upgrade for the standard dependency injection framework with keyed services, letting you "key" a service implementation with a string:

```csharp
builder.Services.AddKeyedScoped<IService, ServiceOne>("one");
builder.Services.AddKeyedScoped<IService, ServiceTwo>("two");

app.MapGet("/", ([FromKeyedService("two")] IService service) => ...);
```

I don't say that keyed services are significant because I use them, but because I now have to be cognizant of them whenever I implement some DI thing!

In the web development world, there were a lot of small things that, looking back on them now, I do not use. There are a couple neat things though.

SignalR, for one, got a new `WithStatefulReconnect` feature that a client connection can specify to automatically reconnect with the same client id when it's disconnected. This, however, relies on the same SignalR server staying alive (as it remembers the client id in its memory). This makes it more difficult to deploy SignalR servers unless you have access to some fancy deployment management tooling to keep servers with outstanding connections alive after new versions are deployed. I stopped using it after a bit.

Blazor got a bit of a major rejigger on this release, allowing us to blend server- and client- side Blazor. You can now have a server-rendered Blazor application that has some _components_ that execute in WASM on the client, or you can even wriet a SPA that initially is server-rendered but then seamlessly transitions itself into a full WASM client once the client is fully downloaded, eliminating long startup times. They also introduced "Blazor Hybrid," allowing an electron-like experience with Blazor and MAUI, but MAUI is still bad.

Blazor applications are _still_ 10MB by default, so very large, and the whole server-to-client transition thing is a bit of a workaround, but it's not uncommon for SPA libraries to support this and Blazor makes it quite easy.

C# 12 introduced two huge quality of life features: primary constructors and collection expressions.

When records were introduced in C# 9, they had "primary constructors":

```csharp
public record Person(string Name, int Age);
```

These arguments are then compiled into public, read-only properties in the record. C# 12 made primary constructors available for classes, but they behave differently:

```csharp
public class PersonService(PersonRepository repository, ILogger<PersonService> logger)
{

}
```

Notice the argument identifiers are lower-case: for classes, primary constructor arguments compile into private, read-only fields. In fact, the above is exactly equivalent to the following in previous versions:

```csharp
public class PersonService
{
    private readonly PersonRepository repository;
    private readonly ILogger<PersonService> logger;

    public PersonService(PersonRepository repository, ILogger<PersonService> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }
}
```

The primary constructor, then, significantly reduces the amount of things we need to write, particularly for services with many dependencies. One thing to note also is the primary constructor gets rid of the need to preface private fields with an underscore (`_`). In previous versions, most C# engineers would have preferred to write the following:

```csharp
public class PersonService
{
    private readonly PersonRepository _repository;
    private readonly ILogger<PersonService> _logger;

    public PersonService(PersonRepository repository, ILogger<PersonService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public Person? GetPerson(int id)
    {
        var personResult = _repository.GetPersonById(id);

        if (personResult is not PersonDao p)
        {
            _logger.LogInfo("Unable to find person {Id}", id);
            return null;
        }

        return new(p.Name, p.Age);
    }
}
```

However, it's now typically preferred to eschew the underscore entirely:

```csharp
public class PersonService(PersonRepository repository, ILogger<PersonService> logger)
{
    public Person? GetPerson(int id)
    {
        var personResult = repository.GetPersonById(id);

        if (personResult is not PersonDao p)
        {
            logger.LogInfo("Unable to find person {Id}", id);
            return null;
        }

        return new(p.Name, p.Age);
    }
}
```

Collection expressions are the other great feature in this release. What this feature does is it makes the following a literal value in C#:

```csharp
[1, 2, 3]
```

So, I can do the following:

```csharp
public int[] GetInts() =>
    [1, 2, 3];
```

This is huge! Previously I'd need to do:

```csharp
public int[] GetInts() =>
    new int[] {1, 2, 3};
```

With the spread operator this saves a lot of code:

```csharp
public static T[] Concat<T>(this T[] first, T[] second) => [..first, ..second];
```

## .NET 9 and C# 13

Having done such a significant amount of development to this point, .NET 9 slowed down a bit and, like .NET 7, had a huge focus on performance. Not that .NET 8 didn't have major performance improvements, but .NET 9 really did. When [I wrote about this release](https://ian.wold.guru/Posts/dotnet_9_csharp_13.html) I mostly complained about Microsoft pushing AI and Aspire, and I continue to hold this opinion.

There were some fun goodies though. I enjoy my LINQ and we got new `CountBy` and `AggregateBy` methods, which do what they sound like they do.

In this version the compiler started recommending the following when doing any kind of regex matching (example from MS's docs):

```csharp
[GeneratedRegex(@"\b\w{5}\b")]
private static partial Regex FiveCharWordProperty { get; }
```

This uses source generators for regex, which does seem to be a whole lot faster in parsing regex. This is good news all around, I know plenty of applications with a little bit of regex here and there (mostly in a validation layer) that benefit from this.

A big improvement to async work came with `await foreach` and `Task.WhenEach` (example also from MS's docs):

```csharp
await foreach (Task<string> t in Task.WhenEach(first, second, third))
{
    Console.WriteLine(t.Result);
}
```

The other great async improvement is the new `System.Threading.Lock` object, which provides a better interface and language integration for locking assets in async operations (instead of locking on an `object`, use the `Lock` API).

On the web side, ASP got `app.MapStaticAssets()`, which is a great builtin feature for shipping static assets, like a React SPA, with compression and etags built in.

Blazor got constructor injection for Razor components! Yes, before this you had to use property injection. No joke!

SignalR received a number of improvements. Polymorphic type support was big for me when it came out, but it also received a lot of updates to the telemetry it omits. This was really key for a lot of applications; having a complicated websockets app with insufficient telemetry was probably not great for a lot of folks.

Finally, ASP also introduced a `HybridCache` type that is capable of being both in-memory and distributed, handling a lot of the complexity of having those two tiers of cache. Very handy! I constantly insist that caches are difficult and hazardous; having a well-constructed first-party implementation with an appropriately restrictive API allows a wider adoption of safer caches.

On the C# side, there's a bunch of small things. With how impactful previous features were, it is good they slowed down a bit.

My favorite is being able to use any collection type for `params`:

```csharp
public void DoThing(params IEnumerable<string> args) => ...
```

The language also introduced `^` as an operator you can use when indexing arrays, meaning "from the end":

```csharp
var myArray = new int[5];
myArray[^1] = 12;
```

`myArray` then has `12` at the 1th position from the end. Interesting!

## .NET 10 and C# 14

As of my writing this, .NET 10 and C# 14 have _just_ been released a few days ago. Exciting!

This release is similar to the last in that the development has settled into a more manageable pace; it keeps the focus on broad performance improvements, incremental nice-to-haves, and rounding out C#'s support for modern development patterns.

One _very_ nice addition is `WebSocketStream`, which essentially lets you treat a web sockets connection like a stream. I think I may be using this a lot!

My favorite nice-to-have is `dotnet run program.cs`, which completes the scripting capabilities in C#. With this feature, I do not need a SLN or CSPROJ in order to run a C# script. If I have a file `sayhi.cs`:

```csharp
Console.WriteLine("Hello, World!");
```

With nothing else in the directory, I can `dotnet run sayhi.cs` and it runs. Super cool! This isn't just great for local scripting, I use this to generate my site!

* My site has a single [build.cs](https://github.com/IanWold/ianwold.github.io/blob/master/build.cs)
* Which I then run to generate this static site [in a GitHub action](https://github.com/IanWold/ianwold.github.io/blob/master/.github/workflows/build.yml#L47)

On the web development side, Blazor got a huge number of quality-of-life improvements that make developing with it - even a full WASM app - actually doable. I have a few Blazor projects and since upgrading to the .NET 10 previews it's been actually very nice. I won't go into any particular features but to say that if you've not enjoyed Blazor in the past, this one might make you more comfortable with it.

Another thing I'm eager to dig into is an overhaul of JSON Patch. PATCH is still a difficult thing to deal with, but there's a lot of applications for it and a new, robust, standards-compliant implementation looks good.

C# 14 has two features I'm very excited for that have been a long time in the making, maybe more than 10 years each.

First is the `field` keyword. For too long, we've had to manually create backing fields for properties:

```csharp
public class MyClass
{
    private LazyService? _service;
    public LazyService Service =>
        _service ??= new(...);
}
```

This is unfortunate because auto properties compile to include their own scoped field anyway, but the programmer has never had access to this. Having to do the above exposes the backing field internally in the class. Should instance methods use `Service` or `_service`? The answer is definitely `Service` but that gets ignored sometimes. Now, we can use `field` instead to ensure the field is properly scoped to the property:

```csharp
public class MyClass
{
    public LazyService Service =>
        field ??= new(...);
}
```

The next super-cool feature is "extension members." Not really sure where they got that name but that's what we're going with. For a very long time now, the C# language design team has dreamed of what they call "extension everything," extending the langauge to support that any kind of extensions could be made for types. Many languages, not just functional languages, support a much more robust extension system than C#. In fact, C#'s extensions are kind of hacky, essentially just giving a way to turn a static method call into a `.`-looking call. Not _really_ extensions.

This version of C# lets you do the following wizardry, in a step towards "extension everything":

```csharp
public record Person(string FirstName, string LastName, int Age);

public static class Extensions
{
    extension (Person)
    {
        public static string Species => "Homo Sapiens";
    }
}
```

What can I do with that?

```csharp
Console.WriteLine(Person.Species); // Outputs "Homo Sapiens"
```

Whoa!! Super cool! It's an _actual_ extension! But wait, there's more - if I name that type then I can put extensions into the type, similar to how I do with the `this` keyword today:

```csharp
public static class Extensions
{
    extension (Person person)
    {
        public string FullName => $"{person.FirstName} {Person.LastName}";
    }
}
```

But ... notice it's a property and not a method! Wooooo!!! You can still do methods too.

## Bringing Everything Together

So, given all of these, what do modern development practices look like today?

I've mentioned "functional" styles a fair amount. There's three features in particular that support functional-style development: records, pattern matching, and switch expressions. Switch expressions might seem an odd inclusion on this list, but let me demonstrate that it's necessary to include these to really be able to harness functional patterns in C#. Most functional languages rely on function definitions using pattern matching, like this Haskell Fibonacci function:

```haskell
fib :: Integer -> Integer
fib 0 = 0
fib 1 = 1
fib n = fib (n - 1) + fib (n - 2)
```

The C# equivalent now looks _extremely similar_:

```csharp
public static int Fib(int n) => n switch {
    0 => 0,
    1 => 1,
    _ => Fib(n - 1) + Fib(n - 2)
};
```

And it's not just an aesthetic similarity at that. Functional patterns will assume functions can be written in this form, and the switch expression is the _thing_ that allows C# to be able to adopt the patterns 1-to-1.

A more fun comparison could be made with list patterns, here the classic list sum function:

```haskell
sum :: [Int] -> Int
sum [] = 0
sum (x:xs) = x + sum xs
```

Is also equivalent in C#:

```csharp
public static int Sum(IEnumerable<int> l) => l switch {
    [] => 0,
    [var x, .. var xs] => x + Sum(xs),
};
```

To bring records into the fold, these are equivalent to data types that typically exist in functional languages; immutable types are the norm there, and their patterns require these kinds of structures. In particular, functional languages have _algebraic data types_ (ADTs), which are discrete (closed) unions of other types. Take this example:

```haskell
data Expression
    = Const Int
    | Add Expr Expr
```

What is this saying? There is a type `Expression` that can be _either_ a `Const` (which will itself contain a single integer), or an `Add` (which will contain two `Expression` objects). This is modeled like so today with records in C#:

```csharp
public abstract record Expression;
public record Const(int Value) : Expression;
public record Add(Expression Left, Expression Right) : Expression;
```

In this case, C# uses inheritance to signify that an `Expression` can be a `Const` or an `Add`, and this is a typical arrangement when mapping functional ADTs into C#. It is not, however, as precise a translation as the previous examples, because the Haskell structure is saying something kind of different. Where in my C# code I could create a record in some other part of the code and inherit `Expression`, I cannot do this in the Haskell example. Technically, we would say that the C# `Expression` is not `closed`. This presents a difficulty now in how we handle these in functions. Take this Haskell:

```haskell
evaluate :: Expression -> Int
evaluate (Const n) = n
evaluate (Add a b) = evaluate a + evaluate b
```

Since `Expression` is closed in Haskell we will have no problems detected by the compiler, but C# needs an extra line:

```csharp
public static int Evaluate(Expression e) => e switch {
    Const c => c.Value,
    Add(var l, var r) => Evaluate(l) + Evaluate(r),
    _ => throw new ArgumentException("Unknown expression")
};
```

This last line is required to complete the switch, since C# currently has no mechanism by which it can close the `Expression` type while allowing the inheritance by `Const` and `Add`. Nonetheless, this is the current proper translation of the functional functions, and is commonly seen in C# today. In fact, this is a great example of what I mentioned in the introduction to this article, that the langauge and framework also evolve to meet the demands of engineers who are using C# in new ways owing to external influence! In this case, the C# langauge team has been pursuing including ADTs fully into the language, a feature which they call "discriminated unions." The language design team's latest writings on the feature seem to indicate we should see initial support for these in the next version.

I've also written a bit about some functional patterns that have become particularly popular in the .NET world:

* [Functional Patterns in C#](https://ian.wold.guru/Posts/book_club_10-2023.html)
* [Results, Railways, and Decisions](https://ian.wold.guru/Posts/book_club_1-2025.html)
* [Roll Your Own C# Results](https://ian.wold.guru/Posts/roll_your_own_csharp_results.html)
