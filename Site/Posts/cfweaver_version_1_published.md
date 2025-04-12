;;;
{
	"title": "CFWeaver Version 1.0.0 Published",
	"description": "I've published version 1 of my test scenario generation tool CFWeaver",
	"date": "12 April 2024",
	"contents": false,
	"hero": "photo-1714245145426-c8565109aa34",
    "topics": ["Projects"],
    "related": [
		{ "title": "Thing I Made: CFWeaver", "description": "I made a simple CLI tool to generate comprehensive test scenarios from control flow models in simple markdown", "fileName": "thing_i_made_cfweaver" },
		{ "title": "Guerrila DevEx Testing", "description": "Developer experience is subjective. Employ the 'hallway test' method to ascertain your code's quality.", "fileName": "guerrila_devex_testing" },
		{ "title": "Book Club 2/2024: Recovering from TDD and Unit Tests", "description": "TDD and unit tests are overused and often misprescribed. What do we really hope to gain from our tests, and what testing practices support our goals?", "fileName": "book_club-2-2024" }
    ]
}
;;;

I've just published verion 1.0.0 of [CFWeaver, my test generation project]! This tool has helped me on a few complicated projects at work, sorting out how systems behave. In fact, while I developed it to help generate integration test scenarios, I've used it more to help me map the functionality of some legacy bits of code.

You can read my previous writing [about CFWeaver](https://ian.wold.guru/Posts/thing_i_made_cfweaver.html) or more generally [about this way of thinking](https://ian.wold.guru/Posts/book_club_1-2025.html) that the tool supports. The latest version is [available for download on GitHub](https://github.com/IanWold/CFWeaver/releases/tag/v1.0.0) now!

## Variables

In this release I added the ability to define "variables" along with each step to make it easier to track certain properties of the system through to the table of results. I had previously decided on a syntax for conditions which had the condition specified after a `?`. This turned out to be conveniently similar to the URL query string syntax, so adding variable definitions was as simple as allowing `&` after this to append variables to the result.

```markdown
* Step1: Result ? some condition & myVar = value
```

## Multi-Line Results

This was a big one since it was getting tedious to append multiple results on the same line with `|`. Take:

```markdown
* AuthenticateOnExternalSystem: Success ? DidError = no | Timeout = 500 ? auth server timed out & VerifyLog = auth server timeout & DidError = yes | NotAuthenticated = 403 ? not authenticated | OtherError ? non-timeout error from auth server & VerifyLog = auth server unknown error & DidError = yes
```

That's not great to read. Much better is to have separate lines, and I had supposed early on that I might achieve that with nested lists. However, I really don't like whitespace dependence, even for a simple modeling language that's coopted markdown. Instead, I opted to use `-` as list bullets to distinguish a result list lien from a step line, which isn't the _greatest_ either since it requires specific knowledge, but it strikes me as the least-bad option.

```markdown
* AuthenticateOnExternalSystem
    - Success ? DidError = no
    - Timeout = 500 ? auth server timed out & VerifyLog = auth server timeout & DidError = yes
    - NotAuthenticated = 403 ? not authenticated
    - OtherError ? non-timeout error from auth server & VerifyLog = auth server unknown error & DidError = yes
```

## Tests

The last thing I've done in preparation for this release is I've added an integration testing apparatus. I don't have _everything_ tested since I'm still considering the best way to test HTML and markdown output, but I do have all the errror conditions tested! This was an interesting process that I haven't done for a command line app yet.

Crucially, I had to set up the application to inject the console and file I/O functionality; you can [see that in this commit](https://github.com/IanWold/CFWeaver/commit/80addb8ffb3822d804e14de5e2e7afd031154700). This allows the tests to inject fakes to capture these operations and allows me to run integration tests without needing to build the application and run it in Docker or some other squirrely setup for testing.