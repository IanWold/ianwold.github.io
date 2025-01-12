;;;
{
	"title": "Testing Logging in ASP.NET Core",
	"description": "Comprehensive integration tests may need to validate that specific logs are output in certain conditions. Luckily, this is very easy in ASP.NET Core.",
	"date": "12 January 2025",
	"contents": false,
	"hero": "photo-1493556273165-cf5bca5cc8e6",
    "topics": ["How-To", "Testing"],
    "related": [
		{ "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "fileName": "deploying_aspdotnet_7_projects_with_railway" },
		{ "title": "Book Club 2/2024: Recovering from TDD and Unit Tests", "description": "TDD and unit tests are overused and often misprescribed. What do we really hope to gain from our tests, and what testing practices support our goals?", "fileName": "book_club-2-2024" },
		{ "title": "Guerrila DevEx Testing", "description": "Developer experience is subjective. Employ the 'hallway test' method to ascertain your code's quality.", "fileName": "guerrila_devex_testing" }
    ]
}
;;;

While I'm not much a fan of unit tests, I'm a big fan of integration tests, particularly for distributed services. I'm in agreement with Grug that [this is the right level for long-lived automated tests](https://grugbrain.dev/#grug-on-testing), and I try to structure my projects in such a way that everything which I need to test can be and is tested this way. Now, this strategy doesn't apply to every project type, but any project that can be seen as a request-response black box - like an ASP.NET Core API - is a proper candidate.

A lot of API integration testing will focus on HTTP requests and the responses to those and call it a day. That's perfectly fine, and surely captures most all the necesssary business logic to test. We would be mistaken though to think these are the only inputs and outputs to an API. If you're running in production, I'm sure your API is able to be configured by a DevOps administrator, and maybe it publishes events to an event queue or schedules emails to be sent. These are all different inputs and outputs that can also be tested. One output of our services that often goes untested are their logs.

Why would we need to test log output? You might have many reasons, but I suspect the primary reason would be if you have a monitoring system reading the logs from the API with alert conditions set up in specific cases. Suppose you have a critical path in your API and you want to get pinged about any errors in this flow. You might configure your monitoring service to send you a text if it ever sees an ERROR-level log with text "critical failure." This log output has just become a business rule, and you may well want to keep a test to ensure that if the API ever enters that condition, the log with the correct level and string will be output, as expected by the monitoring suite.

At least, that's the condition I found myself in recently, needing to ensure certain logging conditions are met for specific failure situations. So, I'll document how I went about setting these up for an integration test suite for an ASP.NET Core API. My whole sample solution and all the code on this article [can be found on my GitHub](https://github.com/IanWold/AspNetCoreTestLogOutput/tree/main).

To set up the tests, we just need a simple ASP application with an endpoint that logs an error:

```csharp
var app = WebApplication.CreateBuilder(args).Build();

app.MapPut("/test", (ILogger<Program> logger) =>
    logger.LogError("Test error!")
);

app.Run();
```

If you start a new project today, you'll find that the ASP templates will set you up with a "classless" `Program.cs` file, but this is a bit of an issue when it comes to testing as we need to make the `Program` class public _or_ internally visible to the test project. To set up the demonstration I just updated [my Program.cs file](https://github.com/IanWold/AspNetCoreTestLogOutput/blob/main/AspNetCoreTestLogOutput.Api/Program.cs) to use an actual Program.Main method.

The `Program` class is used in our test code to set up a client to which we can send HTTP requests. I'll use XUnit to set this test up, but this solution works with any test runner. (Note that the test code requires you to add the Nuget packages [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging) and [Microsoft.AspNetCore.Mvc.Testing](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing)).

```csharp
[Fact]
public async Task TestErrorLogged()
{
    var client = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => {}).CreateClient();
    var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Put, "/test"));
    Assert.True(response.IsSuccessStatusCode);
}
```

If we run this test now we'll see it passing. The `/test` endpoint is logging an error, but note that it is always responding 200 by default. Now, we need to capture that error log and update our test to check for it.

The web host builder, which we configure with the helpfully-named `WithWebHostBuilder`, allows us to alter the configuration and services of the ASP app. You can think of it as ammending the normal setup logic in `Program` so that the tests can inject their own configuration, replace services with fakes, or any other alterations that might be needed to set up the test.

In this case, we're going to want to configure how the ASP app deals with logs. ASP is set up with a default _logger provider_ which it uses to resolve the `ILogger<Program>` dependency on our endpoint. We're going to want to write our own `ILogger` to be injected there, and in order for ASP to use our `ILogger` we'll also need to write our own `ILoggerProvider` and configure our app-under-test to use this logger provider.

These interfaces are quite easy to implement, especially since we only need to store logs in-memory so we can search through them in the test. I'll use a [ConcurrentBag](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentbag-1?view=net-9.0) as the collection type to avoid any possible concurrency issues.

`ILogger` only contains three methods, and we'll really only be interested in the `Log` message, in which we'll simply add the log into the in-memory `ConcurrentBag`:

```csharp
public record LogMessage(LogLevel LogLevel, string Message);

public class TestLogger(ConcurrentBag<LogMessage> logs) : ILogger
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
        logs.Add(new(logLevel, formatter(state, exception)));

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}
```

`ILoggerProvider` only has _one_ method which returns an `ILogger` implementation given some category name. We don't care about the category name, so we can just return our custom `ILogger`:

```csharp
public class TestLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentBag<LogMessage> _logs = [];

    public IReadOnlyCollection<LogMessage> Logs => _logs;

    public ILogger CreateLogger(string categoryName) => new TestLogger(_logs);

    public void Dispose() => GC.SuppressFinalize(this);
}

```

I personally prefer to not expose a `ConcurrentBag` and instead expose it as an `IReadOnlyCollection`, but that's a stylistic preference.

The only thing left is to revisit our test method - we need to configure our ASP app to use this logger provider, and we should add a test for the error log. We can use the `ConfigureLogging` method on the web host builder to ensure our custom logger provider is the only provider, and to make sure logs at all levels are captured. After we send the request, we can then inspect the logs in our custom logger provider to confirm the expected log was logged.

```csharp
[Fact]
public async Task TestErrorLogged()
{
    var loggerProvider = new TestLoggerProvider(); // [tl! focus]
    var client = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder => // [tl! focus:7]
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddProvider(loggerProvider);
                logging.SetMinimumLevel(LogLevel.Trace);
            })
        )
        .CreateClient();

    var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Put, "/test"));

    Assert.True(response.IsSuccessStatusCode);
    Assert.Contains(loggerProvider.Logs, l => l.LogLevel == LogLevel.Error && l.Message.Contains("Test error")); // [tl! focus]
}
```

And that's all! Again, all the code above is in [a working solution on my GitHub](https://github.com/IanWold/AspNetCoreTestLogOutput/tree/main). 