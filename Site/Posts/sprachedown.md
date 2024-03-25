;;;
{
	"title": "SpracheDown",
	"description": "I created a Markdown parser with the Sprache library after it was recommended to me at the Iowa Code Camp.",
	"date": "26 September 2014",
	"contents": false,
	"hero": "photo-1504851149312-7a075b496cc7",
	"series": "Past Articles",
    "related": [
		{ "title": "An Introduction to Sprache", "description": "Sprache is a parser-combinator library for C# that uses Linq to construct parsers. In this post I describe the fundamentals of understanding grammars and parsing them with Sprache, with several real-world examples.", "fileName": "sprache" },
        { "title": "Parsing Comments in Sprache", "description": "I recently made a comment parser for the Sprache framework, and I wanted to give a basic run-down on how it works.", "fileName": "sprache_comments" },
        { "title": "SpracheJSON", "description": "A small library serializing and deserializing JSON using Sprache.", "fileName": "sprachejson" }
    ]
}
;;;

About a year and a half ago I attended one of the [Twin Cities Code Camps](http://www.twincitiescodecamp.com), and there I was shown a nifty library called Sprache. Sprache is, by no means, a novel invention. It's a monadic parser combinator based on years of programming done with similar libraries popular in functional languages. I say it's nifty because it seems to be the cleanest monadic parser combinator made in C#. It's also got a large enough following that keeps it up-to-date well enough, and it's what I was taught at the code camp.

Half a year after that, almost a full year ago now, I attended my first [Iowa Code Camp](http://www.iowacodecamp.com). One guy I taught about Sprache and monadic parser combinators asked if it was possible to create a markdown parser with it, and my answer was a brief "Well, yeah, you can totally do that." I've been mulling that over for about a year now, and I decided to look for a Markdown parser implemented with a monadic parser combinator to see what it would look like.

I found an excellent [article](http://www.greghendershott.com/2013/11/markdown-parser-redesign.html) by Greg Hendershott who implemented a MarkDown parser with a variation of Parsec for Racket. I browsed through his [parser](https://github.com/greghendershott/markdown) on GitHub, and to my alarm it seemed that the parser itself was 1,000 lines long (I could be misreading that, I've no prior experience with Racket, but I'm assuming "parse.rkt" contains the parser). This compelled me to attempt such a parser with Sprache. If nothing else, it would be an interesting comparison.

So I began writing this parser in steps, gradually adding features to it. I started out with headers, then lists, then paragraphs, and so forth. As I was writing this parser, I deliberately omitted features from MarkDown, notably reference-style links and inline HTML. I think if you can parse the majority of MarkDown the little bits could be implemented with equal ease, so I don't believe this invalidates the parser. I may go on and add inline HTML (Sprache ships with an XML parser example), but so long as this is a neat little pet project, I don't think I'll go too far beyond that.

At first, I parsed the MarkDown text directly into strings representing the HTML output. This was efficient, but of course it wouldn't do for any parser - the user needs to be able to manipulate the output. To save time, I borrowed the syntax tree that Sprache's XML example comes with. I adapted and modified the objects with a couple methods to bend them to my will, so to speak, and from there it only took a minute to plug them into my parser.

One problem that Mr. Hendershott faced was parsing MarkDown's nested list feature (this is achieved by inserting spaces before the asterisk in MarkDown). I don't know if this was due to the language he was using or a limitation of the parser, 

Now, I can't say I actually ran into any major problems when I implemented the parser. Granted, I ignored the MarkDown I didn't like so well, but as I said, I don't believe that invalidates the parser. In fact, my success with this parser speaks to the beauty inherent in parser combinators, specifically Sprache in this case. Over the past two days, I've spent a total of six hours working on this, and it's already relatively well-polished. The code is clear and readable, thoroughly commented, and the syntax tree is easily scalable. In addition, my parser is significantly smaller than Mr. Hendershott's parser (perhaps an advantage of Sprache in C# over Parsack in Racket?), and I don't believe I could top 1000 lines in the parser alone if I were to bring the parser up to speed with all of MarkDown's features.

As I've been saying, I wrote this as a proof of concept, and I don't really intend for it to go anywhere, but if you think this is the coolest thing ever and you want it to be something, you're more than welcome to submit pull requests, or you can fork it and do your own thing with it. I believe I've documented everything thoroughly, so it should be easy to find your way around, but if I've missed something don't wait to contact me. Have fun!

You can find SpracheDown on GitHub right [here](https://github.com/IanWold/SpracheDown).
