;;;
{
	"title": "Roll Your Own C# Results",
	"description": "C# doesn't have discriminated unions yet, but that shouldn't stop us from adopting the result pattern to strengthen and simplify our code. It's not complicated at all to create result objects that give us all the expressiveness and safety we want!",
	"date": "3 May 2024",
	"contents": false,
	"hero": "photo-1562158147-f8d6fbcd76f8",
    "related": [
		{ "title": "Roll Your Own End-to-End Encryption in Blazor WASM", "description": "Using the SubtleCrypto API to get simple end-to-end encryption for a collaborative Blazor WASM app.", "fileName": "end_to_end_encryption_witn_blazor_wasm" },
		{ "title": "Book Club 10/2023: Functional Patterns in C#", "description": "This month I've focused on functional domain modeling and related patterns. We're just a few weeks away from the release of the next version of C#, and like each previous version it'll introduce even more functional features.", "fileName": "book_club_10-2023" },
		{ "title": "Write Your Own RDBMS Versioned Migration Boilerplate", "description": "Versioned migrations are an essential tool for systems using an RDBMS, and it's no work at all to start your applications the right way with this pattern.", "fileName": "write_your_own_rdbms_versioned_migration_boilerplate" }
    ]
}
;;;

The result pattern is one of my favorite functional patterns to use in the C# world, and I don't think I'm alone on this. If you try to Google "result pattern" to try to find a Wikipedia article or something to link to as a definition of the pattern, you'll find a bunch of articles and repositories explaining the pattern in C#. Maybe that's just my search results, but this pattern is such a fundamental concept in functional programming that I imagine most folks regularly using functional languages might scoff at the suggestion that this should be elevated to the level of being a proper design pattern.

I'd like to consider it a proper pattern though. Typically and traditionally our C# programs handle errors by throwing and catching exceptions. Exceptions are abused in this case though - they're intended for truly _exceptional_ events, occurrances which I don't expect to happen and which absolutely necessitate the interruption of my control flow. If your API's validation layer found a validation error in a request body, that's not exceptional because I expect clients to send invalid requests time-to-time. If my application wasn't able to parse a JSON response from an external resource, that's not exceptional because I know that data outside my application is never 100% trustworthy.

Nonetheless, we have all tended at one point to treat them as exceptional and throw exceptions in these cases. This can lead to bad code in a number of ways - I might forget to catch an exception at some point, I might not consider all of the error states I need to support for an operation, or my application might be made slow by excessively throwing exceptions. Instead, it's proper to only throw exceptions when the application has entered a state which we either consider impossible or from which I know it is impossible to recover. In the other 99% of cases, I want to return a result object representing the success of the operation. This is the result pattern.

# Existing Implementations in C#

There's a number of libraries you can find on GitHub to support this in C#. Two well-liked ones are [ardalis' Result library](https://github.com/ardalis/Result) and [altmann's FluentResult library](https://github.com/altmann/FluentResults). You'll notice that these libraries support a method returning one of a set of options - Success, Failure, Invalid, etc. The Computer Science term for this is a [tagged union](https://en.wikipedia.org/wiki/Tagged_union), also called a discriminated union or an algebraic data type. These types are foundational to functional programming and the basis of the result pattern, but not yet supported in C#. [The OneOf library](https://github.com/mcintyre321/OneOf) is an attempt to provide a generic form of these in C#, with which you can implement a result pattern.

_(Note that the C# team is actively working to include discriminated unions in the langauge!)_

Those and others are all fine libraries, and while each of them can solve specific scenarios quite well none of them are a one-size-fits-all approach. If you just need "success" and "failure", then you probably just need one of these libraries or you can write your own class which has an `IsSuccess` flag. However, in a lot of cases we have more than one kind of result we might need to return. A validation layer might have multiple levels of validation (valid, valid but for an old version, invalid), or a persistence layer might want to distinguish between multiple states (found all the objects you queried, the query succeeded but yielded no results, the query itself was invalid, the parameters were invalid, the database crashed, etc). 

Among the libraries which support more than 2 result states, `OneOf` is the best option here, but you'll find that you need to use `T0`, `T1`, `T2`, and so on in your code; this is due to the generic nature of the library. To get around that you'll need to implement your own classes on top of it, which may be what you need but I'd stop here and ask: why do we not roll our own? No, it's not complicated, in fact it's very easy and probably involves no more code than if you were to extend `OneOf` anyway. Then of course you get the benefit that your solution is tailored to your problem and your team can extend and maintain it as your application evolves.

There's a lot of ways to get to where we want to be, and one might be more preferable for your case. I'm going to demonstrate two: first a "quick and dirty" implementation using one class with a result flag, and second a (more preferable, in my opinion) solution using multiple classes and implicit casting. For both examples I'll implement a database query result which for the purposes of this article can either be Found, Empty, ClientError, or ServerError. If it is Found, it will contain the object which we queried; if it is one of the two errors, it will contain a string description of the error.

# First Approach: One Class, Result Flag

The simplest way to get off the ground with a result object is to implement one class with an enum flag signifying what type of result it is:

```csharp
public enum DatabaseQueryResultType
{
    Found,
    Empty,
    ClientError,
    ServerError
}

public class DatabaseQueryResult
{
    public DatabaseQueryResultType Type { get; set; }
}
```

This `DatabaseQueryResult` will also need to hold the value of the result for the `Found` case, as well as the error description for either of the error cases. These need to be nullable as we can't know what kind of result this is until runtime. The class also needs to be generic as we need to support different types of values:

```csharp
public class DatabaseQueryResult<T>
{
    public DatabaseQueryResultType Type { get; set; }

    public T? Value { get; set; }

    public string? ErrorDescription { get; set; }
}
```

This can be used as-is now:

```csharp
public DatabaseQueryResult<IEnumerable<Item>> GetAllItemsFromDatabase(...)
{
    try
    {
        var result = ContactDatabaseSomehow(...);

        if (result is IEnumerable<Item> items)
        {
            return new DatabaseQueryResult<IEnumerable<Item>>
            {
                Type = DatabaseQueryResultType.Found,
                Value = items
            };
        }
    }
    catch (/*exception indicating client error*/)
    {
        return new DatabaseQueryResult<IEnumerable<Item>>
        {
            Type = DatabaseQueryResultType.ClientError,
            ErrorDescription = exception.Message
        };
    }
    catch (/*exception indicating server error*/)
    {
        return new DatabaseQueryResult<IEnumerable<Item>>
        {
            Type = DatabaseQueryResultType.ServerError,
            ErrorDescription = exception.Message
        };
    }

    return new DatabaseQueryResult<IEnumerable<Item>>
    {
        Type = DatabaseQueryResultType.Empty
    };
}

public void ShowItems()
{
    var result = GetAllItemsFromDatabase(...);
    switch (result.Type)
    {
        case DatabaseQueryResultType.Found:
            foreach (var item in result.Value!)
            {
                Console.WriteLine(item.ToString());
            }
            break;
        case DatabaseQueryResultType.Empty:
            Console.WriteLine("No items found!");
            break;
        case DatabaseQueryResultType.ClientError:
        case DatabaseQueryResultType.ServerError:
            Console.WriteLine($"Error! {result.ErrorDescription}");
            break;
        default:
            throw new Exception("This is an actual exceptional state since the code _should_ never get here.");
    }
}
```

This works but oh goodness the verbosity! The most glaring verbosity issue, at least as I perceive things, is all the code we spend instantiating a new `DatabaseQueryResult` each time. Imagine if every single method in the persistence layer was littered with this code - not only is it taking up a lot of space but we're relying on the implementor to perfectly instantiate the `DatabaseQueryResult` each time - they need to know to _always_ set the `Value` if it's found, to _always_ set the `ErrorDescription` for an error, and so on. We can implement a couple of static instantiator methods to guard this and reduce the size of the implementing code:

```csharp
public class DatabaseQueryResult<T>
{
    private DatabaseQueryResult() { }

    public DatabaseQueryResultType Type { get; private set; }

    public T? Value { get; private set; }

    public string? ErrorDescription { get; private set; }

    public static DatabaseQueryResult<T> Found(T Value) => new()
    {
        Type = DatabaseQueryResultType.Found
        Value = value
    };

    public static DatabaseQueryResult<T> Empty() => new()
    {
        Type = DatabaseQueryResultType.Empty
    };

    public static DatabaseQueryResult<T> ClientError(string description) => new()
    {
        Type = DatabaseQueryResultType.ClientError,
        ErrorDescription = description
    };

    public static DatabaseQueryResult<T> ServerError(string description) => new()
    {
        Type = DatabaseQueryResultType.ServerError,
        ErrorDescription = description
    };
}

public DatabaseQueryResult<IEnumerable<Item>> GetAllItemsFromDatabase(...)
{
    try
    {
        var result = ContactDatabaseSomehow(...);

        if (result is IEnumerable<Item> items)
        {
            return DatabaseQueryResult<IEnumerable<Item>>.Found(items);
        }
    }
    catch (/*exception indicating client error*/)
    {
        return DatabaseQueryResult<IEnumerable<Item>>.ClientError(exception.Message);
    }
    catch (/*exception indicating server error*/)
    {
        return DatabaseQueryResult<IEnumerable<Item>>.ServerError(exception.Message);
    }

    return DatabaseQueryResult<IEnumerable<Item>>.Empty();
}
```

That simplifies and safety-ifies (that's not a word) our implementing code a lot. There's still a similar problem in the code which consumes the result object, but to a lesser degree. The switch statement is a bit cumbersome, and it's annoying to have to implement the default case always. Here's the first instance where I can rely on the nature of our specific use case to get some custom value that I might have a bit more difficulty with a library. Although I have 4 result states, for most purposes I only really care about three - did it succeed, did it fail, or is it empty? I can implement two methods - one to check for success and one to check for failure - then use an easier-to-read if/else if/else check to get the behavior I want:

```csharp
public class DatabaseQueryResult<T>
{
    private DatabaseQueryResult() { }

    public DatabaseQueryResultType Type { get; private set; }

    public T? Value { get; private set; }

    public string? ErrorDescription { get; private set; }

    public bool IsFound() =>
        Type == DatabaseQueryResultType.Found;

    public bool IsError() =>
        Type == DatabaseQueryResultType.ClientError
        || Type == DatabaseQueryResultType.ServerError;

    public static DatabaseQueryResult<T> Found(T Value) => new()
    {
        Type = DatabaseQueryResultType.Found
        Value = value
    };

    public static DatabaseQueryResult<T> Empty() => new()
    {
        Type = DatabaseQueryResultType.Empty
    };

    public static DatabaseQueryResult<T> ClientError(string description) => new()
    {
        Type = DatabaseQueryResultType.ClientError,
        ErrorDescription = description
    };

    public static DatabaseQueryResult<T> ServerError(string description) => new()
    {
        Type = DatabaseQueryResultType.ServerError,
        ErrorDescription = description
    };
}

public void ShowItems()
{
    var result = GetAllItemsFromDatabase(...);

    if (result.IsFound())
    {
        foreach (var item in result.Value!)
        {
            Console.WriteLine(item.ToString());
        }
    }
    else if (result.IsError())
    {
        Console.WriteLine($"Error! {result.ErrorDescription!}");
    }
    else
    {
        Console.WriteLine("No items found!");
    }
}
```

This takes up much less space and, at least to me, reads much clearer with a better understanding of the intention of the code. Ive seen a lot of implementations stop here, but there's one more nagging bit - null checks! I _know_ that when the result is `Found` that `Value` is going to be non-null, and the same for `ErrorDescription` when the type is one of the error types. It's annoying for the consumer to have to spam `!` everywhere when it knows that these values are not null, and it's bad practice anyway to not encapsulate this behavior.

In this case I can implement the `IsFound` and `IsError` methods to return the non-null values with a sort of modified try pattern, which would also allow me to hide the internal state of the result object from the outside world. This is much better design, and it pains me when this simple step isn't taken. I'll just provide a simple implementation of these two methods, yours might involve more checks depending on your case:

```csharp
public class DatabaseQueryResult<T>
{
    private DatabaseQueryResult() { }

    private DatabaseQueryResultType Type { get; set; }

    private T? Value { get; set; }

    private string? ErrorDescription { get; set; }

    public bool IsFound([NotNullWhen(true)] out T? value)
    {
        value = Value;
        return Type == DatabaseQueryResultType.Found;
    }

    public bool IsError([NotNullWhen(true)] out string? description)
    {
        description = ErrorDescription;
        return
            Type == DatabaseQueryResultType.ClientError
            || Type == DatabaseQueryResultType.ServerError;
    }

    public static DatabaseQueryResult<T> Found(T Value) => new()
    {
        Type = DatabaseQueryResultType.Found
        Value = value
    };

    public static DatabaseQueryResult<T> Empty() => new()
    {
        Type = DatabaseQueryResultType.Empty
    };

    public static DatabaseQueryResult<T> ClientError(string description) => new()
    {
        Type = DatabaseQueryResultType.ClientError,
        ErrorDescription = description
    };

    public static DatabaseQueryResult<T> ServerError(string description) => new()
    {
        Type = DatabaseQueryResultType.ServerError,
        ErrorDescription = description
    };
}

public void ShowItems()
{
    var result = GetAllItemsFromDatabase(...);

    if (result.IsFound(out var items))
    {
        foreach (var item in items)
        {
            Console.WriteLine(item.ToString());
        }
    }
    else if (result.IsError(out var description))
    {
        Console.WriteLine($"Error! {description}");
    }
    else
    {
        Console.WriteLine("No items found!");
    }
}
```

By implementing proper OO standards here, we've encapsulated everything about the operation of the result classes and we're exposing only exactly what's necessary. This makes it trivially simple to implement and I don't need a lot of documentation around here - most anyone could jump into the code here and implement a consumer of this result conforming to the proper standards. As a bonus, you should also move the enum definition inside the class and make it private. In addition, if your consumers care about the distinction between client and server errors, you'll want to implement some way of being able to discern that - extra `IsClientError` and `IsServerError` methods would allow consumers to choose whether they care about _any_ error case or if they case about _specific_ error cases.

There's another important point here that often goes overlooked in result implementations: it's _impossible_ for me to get the value of the result without performing a success check at the same time. Often those who encourage using the result pattern in C# focus on the negative aspects of throwing exceptions more than the positive effects of forcing success checks; indeed, this is because a lot of result implementations in C# don't force these checks. However, this is an essential - maybe _the_ essential - part of this pattern. It helps to put you and your team in the pit of success by forcing proper (or at least better) explicit error case handling.

**If you take nothing else away from this article, take away the above paragraph: Always use the result pattern to enforce proper error handling.**

There's one bit yet that annoys me about this code, and I think we can do better. However, it's going to force us to rewrite our solution here. My problem is the code which produces one of these results - in the empty or error cases, I still need to write out the full return type even though I don't have one! This seems like a minor inconvenience but look at the code above - if your objects are named longer that's a lot of eyesore. Wouldn't it be nice if there was some type inference to write something like:

```csharp
public DatabaseQueryResult<IEnumerable<Item>> GetAllItemsFromDatabase(...)
{
    try
    {
        var result = ContactDatabaseSomehow(...);

        if (result is IEnumerable<Item> items)
        {
            return DatabaseQueryResult.Found(items);
        }
    }
    catch (/*exception indicating client error*/)
    {
        return DatabaseQueryResult.ClientError(exception.Message);
    }
    catch (/*exception indicating server error*/)
    {
        return DatabaseQueryResult.ServerError(exception.Message);
    }

    return DatabaseQueryResult.Empty();
}
```

Being clear, that won't compile now. Instead, we'll look at a second approach.

# Second Approach: Multiple Classes, Implicit Casting

This approach is going to get us closer to how this pattern tends to be used in actual functional languages, and it's much closer to how I assume we'll be coding once the C# team adds discriminated unions to the language. With this approach, we're going to create one record per each result type, then use implicit casting on a generic base type to allow instances of these records to instantiate the base type. This will allow methods to use the base type as a return type. This base class will contain the flags and values and `isFound` checks and everything we had before, these extra classes will just be giving us a nicer way to instantiate and think about our results.

Why not use inheritance? Because that would not get me away from having to specify the generic type for all result values. You can certainly implement a result object using inheritance, but I don't think that the majority of use cases benefit from it.

Though we could extend the first approach, I'm going to start from the ground up here for the sake of simplicity. The first thing I'm going to do is to define all of my result types:

```csharp
public record Found<T>(T Value);

public record Empty();

public record ClientError(string Description);

public record ServerError(string Description);
```

The other half of this is the base result class, which we can implement with the implicit casts off the bat:

```csharp
public sealed class DatabaseQueryResult<T>
{
    private DatabaseQueryResult<T>() { }

    public static implicit operator DatabaseQueryResult<T>(Found<T> found) => new();

    public static implicit operator DatabaseQueryResult<T>(Empty empty) => new();

    public static implicit operator DatabaseQueryResult<T>(ClientError clientError) => new();

    public static implicit operator DatabaseQueryResult<T>(ServerError serverError) => new();
}
```

Now the records whih represent our result states already contain all of the information we need, so we don't need to duplicate those properties on the `DatabaseQueryResult` object. In order to keep our state we can define an `object` field for our result class to store whichever result state we've gotten. Since our result object is carefully guarding its instantiation and internal state, I don't think we need to worry about type safety here. If you've got a different situation you might need to implement something more robust, though.


```csharp
public sealed class DatabaseQueryResult<T>
{
    object _value;

    private DatabaseQueryResult<T>(object value) =>
        _value = value;

    // Implicit casts
}
```

Then we can use type checking to implement our `IsSuccess` and `IsError` methods:

```csharp
public sealed class DatabaseQueryResult<T>
{
    public bool IsSuccess([NotNullWhen(true)] out T? value)
    {
        if (_value is Found<T> found)
        {
            value = found.Value;
            return true;
        }

        _value = default;
        return false;
    }

    public bool IsError([NotNullWhen(true)] out string? description)
    {
        description = null;

        if (_value is ClientError clientError)
        {
            description = clientError.Description;
        }

        if (_value is ServerError serverError)
        {
            description = serverError.Description;
        }

        return description is null;
    }

    // Rest of implementation
}
```

We could imagine that a consumer might care only about an empty state, and it's easy to implement an `IsEmpty`, and this implementation is quite human-readable:

```csharp
public bool IsEmpty() =>
    _value is Empty;
```

This should give us everything we need for thsi nicer result type. Compare our previous implementation of `GetAllItemsFromDatabase` to this new one:

```csharp
public DatabaseQueryResult<IEnumerable<Item>> GetAllItemsFromDatabase(...)
{
    try
    {
        var result = ContactDatabaseSomehow(...);

        if (result is IEnumerable<Item> items)
        {
            return new Found<IEnumerable<Item>>(items);
        }
    }
    catch (/*exception indicating client error*/)
    {
        return new ClientError(exception.Message);
    }
    catch (/*exception indicating server error*/)
    {
        return new ServerError(exception.Message);
    }

    return new Empty();
}
```

I find myself very much in favor of this since `return new ServerError(exception.Message)` makes a lot more senes to me than `return DatabaseQueryResult<IEnumerable<Item>>.ServerError(exception.Message)`. It's not just shorter, it reads more clearly and in six months when I revisit my code I won't be scratching my head why this line needs to provide `IEnumerable<Item>` as a type argument when it's got nothing to do with a list of items.

This works great, but there's one more quality of life improvement we can give ourselves. When we consume one of these result objects, we'll need to consult either `IsSuccess`, `IsError`, or `IsEmpty` - maybe two or all three of these - in order to direct our control flow, and this involves several conditional statements. Suppose we want to map this result onto a different value depending on its state - that will look even worse! The afrementioned `OneOf` library contains a method for this purpose on the result object which accepts functions for each possible state to facilitate an easier way to achieve this mapping, though our custom implementation gives us the option of being able to name the arguments to this method in a way that match to the specific state of each, rather than the `t1`, `t2`, etc naming in that library.

```csharp
public sealed class DatabaseQueryResult<T>
{
    public U Map<U>(
        Func<T, U> found,
        Func<U> empty,
        Func<string, U> error
    )
    {
        if (_value is Found<T> foundValue)
        {
            return found(foundValue.Value);
        }
        else if (_value is ClientError clientErrorValue)
        {
            return error(clientErrorValue.Description);
        }
        else if (_value is ServerError serverErrorValue)
        {
            return error(serverErrorValue.Description);
        }

        return empty();
    }
    
    public U Map<U>(
        Func<T, U> found,
        Func<U> empty,
        Func<string, U> clientError,
        Func<string, U> serverError
    )
    {
        if (_value is Found<T> foundValue)
        {
            return found(foundValue.Value);
        }
        else if (_value is ClientError clientErrorValue)
        {
            return clientError(clientErrorValue.Description);
        }
        else if (_value is ServerError serverErrorValue)
        {
            return serverError(serverErrorValue.Description);
        }

        return empty();
    }

    // Rest of implementation
}
```

These two overloads for `Match` allow our consumer to choose whether it cares about the client/server error distinction but still enforces that they must provide a complete mapping from any possible result state to an object. We could use this to rewrite our `ShowItems` method from earlier in a clearer way:

```csharp
public void ShowItems() => Console.WriteLine(
    GetAllItemsFromDatabase(...)
    .Map(
        found: items => string.Join(items.Select(i => i.ToString()), "\n"),
        empty: () => "No items found!",
        error: description => $"Error! {description}"
    )
);
```

Now _that's_ looking properly functional!