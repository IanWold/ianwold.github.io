;;;
{
	"title": "How I am Super Techy in my Gherkin",
	"description": "Gherkin doesn't need to just be weird and contrived pseudo-business language. Here's how I drive techy projects faster with techy Gherkin.",
	"date": "8 March 2025",
	"contents": false,
	"hero": "photo-1666785556501-4ed68b00965e",
    "topics": ["Testing", "How-To"],
    "related": [
		{ "title": "It's Okay to be a Bit Techy in Your Gherkin", "description": "Developer experience is subjective. Employ the 'hallway test' method to ascertain your code's quality.", "fileName": "its_okay_to_be_a_bit_techy_in_your_gherkin" },
		{ "title": "Testing Logging in ASP.NET Core", "description": "Comprehensive integration tests may need to validate that specific logs are output in certain conditions. Luckily, this is very easy in ASP.NET Core.", "fileName": "testing_logging_in_asp_net_core" },
		{ "title": "Book Club 2/2024: Recovering from TDD and Unit Tests", "description": "TDD and unit tests are overused and often misprescribed. What do we really hope to gain from our tests, and what testing practices support our goals?", "fileName": "book_club-2-2024" }
    ]
}
;;;

A few weeks ago I wrote about how [it's okay to be a bit techy in your Gherkin](https://ian.wold.guru/Posts/its_okay_to_be_a_bit_techy_in_your_gherkin.html), by which I mean that it's okay to include technical sorts of language in your Gherkin. Gherkin is designed to abstract away the specific technical details of automated tests to make it easy for engineers to collaborate with non-engineer stakeholders over automated tests for a codebase. The general guideline goes that Gherkin should be written in a "business-friendly" manner, but this is actually not quite right; rather, our Gherkin should be written in a "stakeholder-friendly" manner.

If your stakeholders include very-technically-illiterate folks, then you'll define your Gherkin steps to account for that. If, on the other hand, your stakeholders all understand what it means to send an HTTP request into the service you're testing, the following is (almost certainly) going to be easier to collaborate over:

```
Scenario: Create a resource when myFlag is on
    Given the feature flag "myFlag" is ON
    When I send a POST request to "/my/resource" with the body
        """
        {
            "name": "Bob",
            "age": 42
        }
        """
    Then the response status code should be 200
```

Since I wrote that last blog post, I've had a lot of successful collaboration over a couple projects at work, and I want to share some of my findings here. Ultimately, I'm impressed by the very tight testing loop which this "low-level" form of Gherkin supports for my team's backend services. If we observe a bug or find one while doing exploratory testing, then we'll already have the request body - this Gherkin format makes it very easy for anyone - not just engineers - to write a test which can be immediately executed on the codebase.

So in a matter of minutes I have the issue reproducible on my local machine, and I can then reach out to other stakeholders to confirm the expected behavior. Again, everyone understands what an HTTP request is, so I can send them the Gherkin, and they can respond to me with updated, valid Gherkin in no time. This all shows the value in ensuring that you set up your Gherkin so that it works in the environment it's supposed to, _and_ that you're using it to actually facilitate collaboration.

## Steps to Arrange Data

In my environment (this could be entirely different for you) anyone who understands what an HTTP request is knows, generally, how SQL or Redis or the like work. Setting up data for the test can be extremely simple that way. It's even easier for a document database. Take the following:

```
Given the "items" table in the Postgres database has the following records:
    | id | name  | age |
    | 1  | 'Bob' | 42  |
    | 2  | 'Sue' | 50  |
And the "some" key in the Redis cache has the value "hello"
And the external endpoint "/my/service/other/resource/1" will respond with status code 200 and body
    """
    {
        "id": 1,
        "favorites": [ "Bob", "Sue" ]
    }
    """
```

Database tables are set up with - surprise - tables, Redis values are set directly, and responses from external services can have their bodies written out in full. Redis is an interesting case actually, since it can have lists and hashes and so on. It's relatively simple to add those:

```
Given the "some" key in the Redis cache has the list [ 1, 2, 3 ]
And the "other" key in the Redis cache has the hash
    | name   | value |
    | first  | 1     |
    | second | 2     |
```

To reiterate, this works so long as all collaborators understand those sorts of systems. When they do, this straightforward approach gives the path of least resistance in moving from an observed scenario into the Gherkin language which allows collaborating over what the business requirements (outputs from the service) are in these conditions. Speaking of which:

## Steps to Test Responses

One key point is that a service (an HTTP API, in this case) does not _just_ output responses to requests. Yes, that's the primary thing to test, but you might want to ensure that events are enqueued in a Kafka or that specific logs are output in certain scenarios to drive alerting, or the like.

These extra scenarios get a bit tricky and probably require that your tests have some way to read all of these output channels. That solution might vary wildly for your project, but the Gherkin steps can be very simple. Remember to keep the focus on what kinds of steps are both most understandable to all your stakeholders _and_ produce the least amount of friction when creating Gherkin scenarios from real-world observed scenarios.

```
Then an error level log is written containing "SuperImportantService failed to respond"
And the following event was sent to the Kafka topic "topic"
    """
    {
        "id": 1,
        "name": "Bob"
    }
    """
```

Testing the actual response from the service is just as easy, though in this case we might want to validate more than just the body - response status code is quite meaningful over HTTP:

```
Then the response status code is 200
And the response body is
    """
    {
        "id": 1,
        "name": "Bob",
        "favorites": [ "Bob", "Sue" ]
    }
    """
```

Now, there is a catch here testing the bodies of both the event and the API response: how is the code written to match the bodies? If I write a multi-line string in my test but the server actually responds with a single-line JSON, I can't do a string compare. Even if we could solve perfectly for whitespace (I don't recommend trying) property rearranging is an issue. But then I also don't necessarily want to test _every_ property; maybe for some tests I just want to make sure one property is coming back altered.

What's wanted then is to test that the response JSON is a superset (for lack of a better word - is there something else we call this?) of what we expect. Luckily, this is quite easy to define:

```csharp
bool IsJsonSuperset(JsonElement targetJson, JsonElement expectedJson) =>
    expectedJson.ValueKind switch
    {
        JsonValueKind.Object =>
            expectedJson.EnumerateObject().All(expectedProperty =>
                targetJson.TryGetProperty(expectedProperty.Name, out var targetValue)
                && IsJsonSuperset(targetValue, expectedProperty.Value)
            ),
        JsonValueKind.Array =>
            targetJson.GetArrayLength() >= expectedJson.GetArrayLength()
            && expectedJson.EnumerateArray().All(expectedArrayElement =>
                targetJson.EnumerateArray().Any(targetArrayElement => IsJsonSuperset(targetArrayElement, expectedArrayElement))
            ),
        _ => targetJson.GetRawText() == expectedJson.GetRawText()
    }
```

## Accounting for HTTP Headers

Ah, headers are just a table - surely I could just include them along with the body when I send the request:

```
When I send a POST request to "/my/resource" with header and body
    | name          | value            |
    | Content-Type  | application/json |
    | Authorization | Bearer hello     |

    """
    {
        "name": "Bob",
        "age": 42
    }
    """
```

Alas, Gherkin does not support such an idea. In a traditional setup, we'd break this down into multiple steps:

```
Given the request has header "Content-Type" with value "application/json"
And the request has header "Authorization" with value "Bearer hello"
When I send the request ...
```

But barrier! My goal is to make it as easy and seamless as possible to move from scenarios we observe in test and prod into our Gherkin - and I want _anyone_ to be able to do that. We need more simple.

How this shakes out in any particular circumstance is going to be affected by tooling and preferences and the usual factors, but what I've found works is to adopt the [frontmatter](https://daily-dev-tips.com/posts/what-exactly-is-frontmatter/) concept to the JSON bodies. With my specific projects (and I can't stress enough that this may be drastically different depending on a lot of factors) the easiest thing has been to include the headers as YAML frontmatter atop the JSON in the body:

```
When I send a POST request to "/my/resource" with the body
    | name          | value            |
    | Content-Type  | application/json |
    | Authorization | Bearer hello     |

    """
    ---
      headers:
        - Content-Type: application/json
          Authorization: Bearer hello
    ---
    {
        "name": "Bob",
        "age": 42
    }
    """
```

It's easy then to parse out `---` and get the information from the YAML. That "frontmatter" could just as easily be JSON, or you could even abandon the pseudo-frontmatter idea and write the whole body using [http file syntax](https://timdeschryver.dev/bits/http-files).
