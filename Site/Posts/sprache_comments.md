;;;
{
	"title": "Parsing Comments with Sprache",
	"description": "I recently made a comment parser for the Sprache framework, and I wanted to give a basic run-down on how it works.",
	"date": "6 February 2015",
	"contents": false,
	"hero": "photo-1617040619263-41c5a9ca7521",
	"series": "Past Articles",
    "related": [
		{ "title": "An Introduction to Sprache", "description": "Sprache is a parser-combinator library for C# that uses Linq to construct parsers. In this post I describe the fundamentals of understanding grammars and parsing them with Sprache, with several real-world examples.", "fileName": "sprache" },
        { "title": "SpracheDown", "description": "I created a Markdown parser with the Sprache library after it was recommended to me at the Iowa Code Camp.", "fileName": "sprachedown" },
        { "title": "SpracheJSON", "description": "A small library serializing and deserializing JSON using Sprache.", "fileName": "sprachejson" }
    ]
}
;;;

I recently made a comment parser for the Sprache framework, and I wanted to give a basic run-down on how it works.

The CommentParser class gives you the option to define the header styles for comments, and it can parse both single- and multi-line comments. It's rather basic as of right now, but that's (hopefully) subject to change in the future.

Using CommentParser is pretty simple, but it's a tad different from the rest of the flow of Sprache as a combinator library. You'll need to make an instance of the CommentParser class, using the comment headers and (optional) newline character you require as arguments:

```csharp
static CommentParser comments = new CommentParser("//", "/*", "*/");
```

From there, CommentParser gives you a couple parsers you can use to parse single- and multi-line comments:

```csharp
static Parser<string> myParser = Parse.String("foobar").Text().Or(comments.AnyComment);
```

CommentParser.AnyComment will parse either single- or multi-line comments for you, while CommentParser.SingleLineComment and CommentParser.MultiLineComment will parse those individually.

A real, working example using the CommentParser class can be found in Sprache's [XMLParser](https://github.com/sprache/Sprache/tree/master/src/XmlExample) example.

In the future, it would be awesome if multiple comment headers could be included, and if whitespace could be defined to include comments. Some work towards this effort has been done on my GitHub [here](https://github.com/IanWold/Sprache/blob/Comments/src/Sprache/CommentParser.cs).
