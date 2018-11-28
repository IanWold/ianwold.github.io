---
title: Parsing Comments with Sprache
author: Ian Wold
date: 6 February 2015
---

I recently made a comment parser for the Sprache framework, and I wanted to give a basic run-down on how it works.

The CommentParser class gives you the option to define the header styles for comments, and it can parse both single- and multi-line comments. It's rather basic as of right now, but that's (hopefully) subject to change in the future.

Using CommentParser is pretty simple, but it's a tad different from the rest of the flow of Sprache as a combinator library. You'll need to make an instance of the CommentParser class, using the comment headers and (optional) newline character you require as arguments:

```c#
static CommentParser comments = new CommentParser("//", "/*", "*/");
```

From there, CommentParser gives you a couple parsers you can use to parse single- and multi-line comments:

```c#
static Parser<string> myParser = Parse.String("foobar").Text().Or(comments.AnyComment);
```

CommentParser.AnyComment will parse either single- or multi-line comments for you, while CommentParser.SingleLineComment and CommentParser.MultiLineComment will parse those individually.

A real, working example using the CommentParser class can be found in Sprache's [XMLParser](https://github.com/sprache/Sprache/tree/master/src/XmlExample) example.

In the future, it would be awesome if multiple comment headers could be included, and if whitespace could be defined to include comments. Some work towards this effort has been done on my GitHub [here](https://github.com/IanWold/Sprache/blob/Comments/src/Sprache/CommentParser.cs).
