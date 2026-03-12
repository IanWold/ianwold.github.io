;;;
{
	"title": "Create Typed SignalR Clients with ... TypedSignalR.Client",
	"description": "Much better to avoid magic strings",
	"date": "12 March 2026",
	"contents": false,
	"hero": "photo-1686061593213-98dad7c599b9",
    "topics": ["Learning", "Dotnet"],
    "related": [
		{ "title": ".NET: From Framework to 10", "description": "Looking back at all of the new features introduced in C# and how they impact the way we develop .NET systems today.", "fileName": "dotnet_from_framework_to_10" },
		{ "title": "Postgres: Use Views to Refactor to Soft Delete", "description": "Refactors are tough, database refactors are scary. Being a bit clever can save us a lot of pain!", "fileName": "postgres_use_views_to_refactor_to_soft_delete" },
		{ "title": "Guerrila DevEx Testing", "description": "Developer experience is subjective. Employ the 'hallway test' method to ascertain your code's quality.", "fileName": "guerrila_devex_testing" }
    ]
}
;;;

SignalR is, I think, a good bidirectional networking library. I like its interface; it offers fully-typed server implementations without onerous reflection overhead - other libraries like [MediatR]() are no-gos for me due to this. However, the typing experience in SignalR doesn't extend to its C# clients. Microsoft's own documentation recommends magic strings, disappointingly.

Curiously, Microsoft _does_ have [a Nuget package](https://kristoffer-strube.dk/post/typed-signalr-clients-making-type-safe-real-time-communication-in-dotnet/) to support source-generated typed clients, and I have made good use of it. Disappointingly again though they do not reference this library in their documentation. More disappointingly yet, this is surely as they have left this in a pre-release version for years. Most disappointingly, when the release of .NET 9 [brought updates to SignalR](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-9.0?view=aspnetcore-10.0#signalr), this library was left behind, now lacking support for some SignalR uses.

Nonetheless, I have continued using Microsoft's solution; I refuse to use magic strings. A couple months ago I ran into [a need to support this feature on clients](https://github.com/IanWold/PostgreSignalR), leaving me worried that this would spiral into a side project of writing my own source generator, spinning off new side projects to support yet-unfinished projects. Lucky me though, someone else has beaten me to it.

[TypedSignalR.Client](https://github.com/nenoNaninu/TypedSignalR.Client), aptly named, is a production-ready and up-to-date source generator for typed SignalR clients. I've switched myself over to this library, and I would recommend it. Here's how client implementations work -

---

As a refresher, servers can be implemented in a typed fashion using interfaces to define the clients:

```csharp
public interface IClient
{
    Task OnSomeEventAsync();
}

public class MyServer : IHub<IClient>
{
    public async Task DoSomething()
    {
        await Clients.Group(someId).OnSomeEventAsync();
    }
}
```

This can be called from clients using magic strings:

```csharp
public class MyClient
{
    private readonly HubConnection _connection;

    public MySignalRClient(string hubUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _connection.On("OnSomeEventAsync", OnSomeEventAsync);
    }

    public async Task DoSomethingAsync() =>
        await _connection.InvokeAsync("DoSomethingAsync");

    public async Task OnSomeEventAsync() { ... }
}
```

And that's all yucky. To use the new `TypedSignalR.Client`, we'll first need a server interface:

```
public interface IServer
{
    Task DoSomethingAsync();
}
```

The server class should implement this now. We'll also want to update the client to implement IClient, but while we're at it we'll also implement a new `IHubConnectionObserver` interface that comes from our new Nuget package, letting us observe SignalR events.

```csharp
public class MyClient : IClient, IHubConnectionObserver // [tl! ++ **]
{
    private readonly HubConnection _connection;

    public MySignalRClient(string hubUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _connection.On("OnSomeEventAsync", OnSomeEventAsync);
    }

    public async Task OnClosed(Exception? exception) { ... } // [tl! ++ **]
    public async Task OnReconnected(string? connectionId) { ... } // [tl! ++ **]
    public async Task OnReconnecting(Exception? exception) { ... } // [tl! ++ **]

    public async Task DoSomethingAsync() =>
        await _connection.InvokeAsync("DoSomethingAsync");

    public async Task OnSomeEventAsync() { ... }
}
```

In order to call the typed server, we'll use the new `_connection.CreateHubProxy` extension:

```
public class MyClient : IClient, IHubConnectionObserver
{
    private readonly HubConnection _connection;
    private readonly IServer _serverProxy; // [tl! ++ **]

    public MySignalRClient(string hubUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _serverProxy = connection.CreateHubProxy<IHub>(); // [tl! ++ **]

        _connection.On("OnSomeEventAsync", OnSomeEventAsync);
    }

    public async Task OnClosed(Exception? exception) { ... }
    public async Task OnReconnected(string? connectionId) { ... }
    public async Task OnReconnecting(Exception? exception) { ... }

    public async Task DoSomethingAsync() =>
        await _serverProxy.DoSomethingAsync(); // [tl! ++ **]

    public async Task OnSomeEventAsync() { ... }
}
```

Then to be rid of the magic string in the event listener we'll use the `Register` extension.

```csharp
public class MyClient : IClient, IHubConnectionObserver
{
    private readonly HubConnection _connection;
    private readonly IServer _serverProxy;
    private readonly IDisposable _subscription; // [tl! ++ **]

    public MySignalRClient(string hubUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _serverProxy = connection.CreateHubProxy<IHub>();
        _connection.On("OnSomeEventAsync", OnSomeEventAsync); // [tl! -- **]
        _subscription = connection.Register<IReceiver>(this); // [tl! ++ **]
    }

    public async Task OnClosed(Exception? exception) { ... }
    public async Task OnReconnected(string? connectionId) { ... }
    public async Task OnReconnecting(Exception? exception) { ... }

    public async Task DoSomethingAsync() =>
        await _serverProxy.DoSomethingAsync();

    public async Task OnSomeEventAsync() { ... }
}
```

To wrap it up, it's good practice to implement IDisposable. Further, the library author recommends using a custom cancellation token source when creating the proxy:

```csharp
public class MyClient : IClient, IHubConnectionObserver, IDisposable // [tl! ++ **]
{
    private readonly HubConnection _connection;
    private readonly IServer _serverProxy;
    private readonly IDisposable _subscription;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public MySignalRClient(string hubUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _serverProxy = connection.CreateHubProxy<IHub>(); // [tl! -- **]
        _serverProxy = connection.CreateHubProxy<IHub>(_cancellationTokenSource.Token); // [tl! ++ **]
        _subscription = connection.Register<IReceiver>(this);
    }

    public async Task OnClosed(Exception? exception) { ... }
    public async Task OnReconnected(string? connectionId) { ... }
    public async Task OnReconnecting(Exception? exception) { ... }

    public async Task DoSomethingAsync() =>
        await _serverProxy.DoSomethingAsync();

    public async Task OnSomeEventAsync() { ... }

    public void Dispose() // [tl! ++ **]
    { // [tl! ++ **]
        _subscription.Dispose(); // [tl! ++ **]
        // ... // [tl! ++ **]
    } // [tl! ++ **]
}
```

Bit of extra work but not enough to keep the juice from being worth the squeeze!
