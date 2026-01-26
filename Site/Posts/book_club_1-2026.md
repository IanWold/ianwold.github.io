;;;
{
	"title": "Book Club 1/26: C#, Without Allocation",
	"description": "Some resources on writing allocation-less (or -free) C#",
	"date": "25 January 2026",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["How-To", "Dotnet", "Performance", "Patterns"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 10&11/2025: .NET 10, C# 14", "description": "Another Thanksgiving, another .NET!", "fileName": "book_club_10-11-2025" },
		{ "title": "Book Club 6&7/2025: Async", "description": "Not the keyword, but asynchronous communication. It just seemed that 'Async' as a title was more interesting.", "fileName": "book_club_8-9-2025" },
		{ "title": "Book Club 6&7/2025: OOP", "description": "Just some blabbering about OOP, paradigms, and being non-dogmatic", "fileName": "book_club_6-7-2025" }
	]
}
;;;

I'm not _very_ good at coding for performance. I can do some rudimentary profiling, and if you give me a couple metrics to optimize for I can do a pretty good job getting those numbers down. When I sit down to write a piece of software though, performance is maybe my second or third concern, after ensuring the software works properly, it's well-formed (for readability and extensibility), it's well-documented, I can deploy it easily, and the like. Intentionally, I write software both in a language/runtime and in the sorts of domains for which this approach works.

I don't think that every engineer on a team needs to be extremely performance-focused. We have colleagues who are fantastic at performance, and I hold their contributions to a high esteem. At the same time though, it's no good allowing anyone to think it's alright for an engineer to neglect developing a decent knowledge of engineering for performance, and it should especially be discouraged for anyone to not consider performance altogether. The worst offense would be to develop a habit of slopping performance concerns on our performance colleague; often code written without an eye towards performance is code that _can't_ be made more performant.

I know plenty of folks will disagree with my enumerating "performance" among the sorts of _skills_ that software engineers can have - for the above reason no less - but the fact is that there are many different sorts of considerations that need to be held in the head of a software engineer, and it's natural that we will each be good at some considerations but not all of them. Again, this is not to give anyone a reason to neglect performance. Instead, this presents an opportunity to ask: _how frequently do you review your own code through a performance lens?_

My answer is "not as frequently as I'd like," which I think is a good opportunity to practice more performant development. One aspect of writing performant code, particularly in a garbage-collected language like C#, is to consider how the runtime allocates data. Reducing allocations in languages like C# or Java isn't just about making sure less memory is used - more memory use _also_ increases garbage collector time, which can result in its own set of difficult-to-debug issues if left unchecked. Using the wrong kind of object (`class`/`struct`) depending on your use case can cause copying where there doesn't need to be any.

Fortunately, we don't need to go looking through IL to be able to learn some fundamentals that can significantly help us here! I've collected some resources on writing allocation-free C#. Notice: there's a lot of repeated information between the sources isn't there?

Before listing these links though, I want to point everyone to one of my favorite C# engineers, though I have never spoken with them. [Yoshifumi Kawai (@neuecc)](https://github.com/neuecc) and their company [Cysharp](https://github.com/Cysharp) have published a wealth of open-source projects that provide a master class in practical, performant C#. From a [zero-allocation LINQ implementation](https://github.com/Cysharp/ZLinq) to maybe the fastest [binary (de)serializer](https://github.com/Cysharp/MemoryPack) for .NET, I am a huge fan.

**Reading**

* [Reduce memory allocations using new C# features - MSDN](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/performance/)
* [Making .NET code less allocatey - Allocations and the Garbage Collector - Maarten Balliauw](https://blog.maartenballiauw.be/posts/2016-10-19-making-net-code-less-allocatey-garbage-collector/)
* [Optimising .NET code: Hunting for allocations - Jonathan George](https://endjin.com/blog/2023/09/optimising-dotnet-code-2-hunting-for-allocations)
* [Zero-Allocation Patterns in Modern .NET Web APIs - Sukhpinder Singh](https://medium.com/c-sharp-programming/zero-allocation-patterns-in-modern-net-web-apis-8c06c18743b4)
* [Zero allocation code in C# and Unity - Sebastiano Mandalà](https://www.sebaslab.com/zero-allocation-code-in-unity/)
* [Writing zero-allocation code with C# - Sergey Nazarov](https://github.com/nazarovsa/csharp-zero-allocation/blob/main/Presentation.pdf)

**Writing**

* [Writing Allocation Free Code in C# - Matt Ellis](https://www.youtube.com/watch?v=nK54s84xRRs)
* [Writing C# without allocating ANY memory - Nick Chapsas](https://www.youtube.com/watch?v=B2yOjLyEZk0)
* [Pushing C# to the limit - Joe Albahari](https://www.youtube.com/watch?v=mLX1sYVf-Xg)
* [Exploring NET's Memory Management — A Trip Down Memory Lane - Maarten Balliauw](https://www.youtube.com/watch?v=9FEfy9y0fFQ)

_And a preview of more performance things..._

* [High-performance code design patterns in C# - Konrad Kokosa](https://www.youtube.com/watch?v=3r6gbZFRDHs)