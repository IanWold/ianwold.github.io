;;;
{
	"title": "It's Okay to be a Bit Techy in Your Gherkin",
	"description": "Gherkin tests are meant to be an abstraction over the code of automated tests to facilitate collaboration by all stakeholders, but that shouldn't mean that we banish any technical whiffs from them.",
	"date": "31 December 2024",
	"contents": false,
	"hero": "photo-1650072395437-223b4906207d",
    "topics": ["Testing", "Processes", "Standards", "DevEx"],
    "related": [
		{ "title": "Book Club 2/2024: Recovering from TDD and Unit Tests", "description": "TDD and unit tests are overused and often misprescribed. What do we really hope to gain from our tests, and what testing practices support our goals?", "fileName": "book_club-2-2024" },
		{ "title": "It's Better to be Consistently Incorrect than Inconsistently Correct", "description": "On consistency in code and what it means for something to be 'incorrect'", "fileName": "its_better_to_be_consistently_incorrect_than_consistently_correct" },
		{ "title": "Guerrila DevEx Testing", "description": "Developer experience is subjective. Employ the 'hallway test' method to ascertain your code's quality.", "fileName": "guerrila_devex_testing" }
    ]
}
;;;

No, the title is not an innuendo, I mean tests written in [Gherkin](https://www.functionize.com/blog/what-is-gherkin-how-do-you-write-gherkin-tests), the language developed to facilitate better [BDD](https://en.wikipedia.org/wiki/Behavior-driven_development) practices. Gherkin provides an abstraction between the definition of a test and the code which executes the test, allowing non-coding stakeholders to collaborate with engineers over automated tests for a project. While it's not the only way to [shift testing left](https://en.wikipedia.org/wiki/Shift-left_testing) in the development cycle, I've used it to great effect on professional projects over the last few years that I've been at Crate and Barrel.

Gherkin is meant to be written in as human-readable a style (read: _non-engineer style_) as possible, this being what can facilitate collaboration between all the various sorts of stakeholders. If you've used Gherkin for a period of time as I have, you'll have encountered the suggestion that Gherkin tests should have _no_ technical ontology, keeping its abstractions entirely separate from its test subject. Indeed, a proper distinction between a bit of code and its tests is necessary, but I want to push back on the idea that there shouldn't be any techyness in the Gherkin tests.

Indeed, for plenty of cases it does make more sense to allow the Gherkin tests to be more techy. I don't know that I can provide a formal definition of the word "techy" for my purposes here, but I think I can give a fair example: suppose I want to test a login page. The techy way might make use of identifiers and tech-focused concepts:

```
Given the user navigates to "http://example.com/login" using a Chrome browser
When the user inputs "john_doe" into the element with id "username-field"
And the user enters "password123" into the element with id "password-field"
And clicks the button with id "login-button"
Then the element with id "welcome-banner" should display "Welcome, John Doe!"
```

While the non-techy way would abstract those away to only describe user behavior:

```
Given John Doe is on the login page
When he enters his username and password
And clicks the "Login" button
Then he should see a welcome message saying "Welcome, John Doe!"
```

These styles are two extremes. Identifiers really don't need to be used in such a case, but on the other hand I could feasibly have different business rules for different browsers. Certainly it should be no trouble to explicitly state user input in the tests. Maybe a better middle ground would be:

```
Given John Doe is on the login page using a Chrome browser
When he enters username "john_doe" and password "password123"
And clicks the "Login" button
Then he should see a welcome message saying "Welcome, John Doe!"
```

If you were to take a dogmatic, anti-techy view regarding your Gherkin, you might feel that this is a bit too much techyness. I don't think most would see it that way - certainly not most stakeholders. This is exactly the point I mean to make: the stakeholders with whom we are collaborating are stakeholders on a software project; they _are techy_ to _some level_. Lesser than the engineers one would indeed hope, but clearly not incapable of navigating the tech world. In fact, I'll suggest that software tests do in fact need to be technical in plenty of ways, no matter the level of abstraction. They are, after all, automated tests for software!

To develop the idea a step further, I think I can give a good example of a time to have very-techy Gherkin. There's no doubt that the way stakeholders collaborate over a project is greatly - maybe mostly - influenced by _who_ the stakeholders are and what the project is. The same would be true for any Gherkin tests used for the project - their nature will be mostly influenced by the nature of the stakeholders and project. It follows that if the stakeholders are themselves very techincally-minded, or if the project is of a sufficiently technical level, that the Gherkin tests themselves would be written in a very techsome manner.

Suppose I have a project of standing up a new microservice to aggregate various events into `doodads` for other internal services. The few stakeholders that there are know what a `doodad` is, what a microservice is, and so on. I should have no problem with the following:

```
When a "create-doodad" even is published with the body
    """
    {
        "DoodadId": 12,
        "Name": "Bob"
    }
    """
And I send a GET request to "doodad-api/v1/doodads/12"
Then the response should have status code 200 with body
    """
    {
        "doodadId": 12,
        "name": "Bob"
    }
    """
```

If everyone understands concepts like events, event and response bodies, GET requests, and the whole shebang, then I suggest there's no issue with the above. In fact, being able to reduce the number of Gherkin steps to a small number of more generic, more technical steps can greatly simplify the testing. On a smaller project that's advantageous!

Now, this example is so technical as to bring into question why Gherkin is used at all - the tests read almost just as the code does. Truly, if there's no need for the Gherkin abstraction, then by all means don't use Gherkin. I did recently find myself in this position where the non-engineering stakeholders (such as a project manager, business analyst, and the like) knew perfectly well what HTTP requests and the like were - and preferred to define business rules in those terms - but would have found themselves too slowed reading actual test code. The uber-techy Gherkin provided exactly the right level of abstraction to facilitate collaboration and a successful BDD process.

So, don't be too dogmatic! There'll be plenty of times that various degrees of tech-speak need to make it into your Gherkin, and when done with respect to the expectations and capabilities of the project's stakeholders it can be a distinct benefit.