---
title: An Introduction to Sprache
author: Ian Wold
date: 22 January 2016
---

As my activity on this blog and my GitHub account may attest, I'm quite fond of a C# library called Sprache. Sprache is a parser-combinator that uses LINQ (Language INtegrated Query) to allow for the elegant construction of parsers in C#. I've been using Sprache for three years now, before I started college, and I've used it to implement a number of domain-specific languages (DSLs) both in side projects on my GitHub and on applications I've worked on. It's only natural I would want to share my favorite C# library with my fellow undergraduate classmates, but there are several factors which make it rather unapproachable for the average undergraduate computer science student. Thus, I have written this piece to provide a completely introductory tutorial to using Sprache. 

I'll explain LINQ and BNF, and then I'll walk you through the implementation of a few simple grammars in Sprache such that I may touch upon all the most important concepts in the Sprache library to allow the reader to immediately begin to implement the grammars which they desire. At the end of this post, I link to several articles which cover the framework and other related readings. In the future, I may also write a short handbook/reference to certain Sprache concepts.

One does not necessarily need to have an understanding of C# to begin using Sprache, but a familiarity of a similar language (i.e. Java) would go a long way. I'm going to assume the reader has an understanding of object-oriented programming. I won't be going into an explanation of what a parser-combinator is, nor what a "combinator" is, in general. If you would like to become more involved in the development of Sprache, though, you should definitely familiarize yourself with the concept. I provide some links at the end of this tutorial to that end.

To begin with, of course, you'll need to download Sprache. You can find it [on GitHub](https://github.com/sprache/Sprache).

## LINQ

LINQ, short for Language INtegrated Query, is a wonderful feature of Visual C# which adds data-querying operators to C#. LINQ expressions are sometimes (grammatically incorrectly) referred to as "LINQ queries" as they read rather fluently as a query on a data set. Here is an example of a LINQ expression:

```c#
var myList = new List<string>()
{
    "hello",
    "world",
    "how",
    "are",
    "you"
};

var startsWithH =
    from s in myList
    where s.ToCharArray()[0] == 'h'
    select s;

foreach (var a in startsWithH)
	Console.WriteLine(a);
```

Here, we start with a list of words, and we desire to print to the console each word which begins with the letter 'h'. The variable _startsWithH_ is defined with the following LINQ expression, which is how we sort out those words which start with 'h':

```c#
from s in myList
where s.ToLower().ToCharArray()[0] == 'h'
select s;
```

Let's look at what's going on here. First, we have a _from_ statement. This will iterate over each object in _myList_, using _s_ as the iterator variable. Next, we have a _where_ statement, which filters out the objects in _myList_ based on the condition provided. Note that several _where_ statements could be specified here. At the end of this LINQ expression, as with every LINQ expression, we have a _select_ statement, which returns each "queried" object. In this case, we only desire to return the strings which begin with the letter 'h'.

LINQ supports several operators apart from _from_, _where_, and _select_, though these are the main ones. Microsoft, naturally, provides a very in-depth [list of LINQ operators](https://msdn.microsoft.com/en-us/library/bb394939.aspx), though Wikipedia has [a much more succinct list](https://en.wikipedia.org/wiki/Language_Integrated_Query#Standard_Query_Operators).

Sprache uses LINQ to construct its parsers. This allows for quick implementation and easy and intuitive readability.

## Backus-Naur Form

Backus-Naur Form, or BNF for short, is a metalanguage used to describe the grammars and syntax of context-free grammars (essentially, for our purposes, this means the grammars of computing languages). BNF defines expressions in terms of other expressions and strings using a number of rules which will become more familiar as we begin implementing these grammars in Sprache.

As an example, suppose I want to define a grammar which specifies an arithmetic expression which might add, subtract, multiply, or divide two digits. I'll provide a BNF definition of this grammar, and then explain it.

```bnf
<expr>      ::= <add> | <subtract> | <multiply> | <divide>

<add>       ::= <digit> "+" <digit>
<subtract>  ::= <digit> "-" <digit>
<multiply>  ::= <digit> "*" <digit>
<divide>    ::= <digit> "/" <digit>

<digit>     ::= "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9"
```

Let's look at each of the elements and what they do. First, the most notable and important element is the reference for an expression, which looks like the following:

```bnf
<expression_name>
```

The expressions are referenced by this convention, and they are defined with the _::=_ operator. In defining such expressions, a number of rules can be used. Look at the definition for *expr*:

```bnf
<expr>      ::= <add> | <subtract> | <multiply> | <divide>
```

The bar ('|') denotes an _or_ relationship. That is, an expression *expr* can be either an *add*, *subtract*, *multiply*, or *divide* expression.

Now let's look at the definition of the *add* expression:

```bnf
<add>       ::= <digit> "+" <digit>
```

This specifies that an *add* can be a *digit*, followed by a plus sign, followed by another *digit*.

You might notice a bit of an inefficiency in the grammar I defined above. Namely, we define *add*, *subtract*, *multiply*, and *divide* separately, but due to the similarity in their structures, it feels like we should be able to define them all together. While there are certainly good reasons one might want to define them separately as I did above, for succinctness one might desire to redefine the grammar as such:

```bnf
<expr>      ::= <digit> ("+" | "-" | "*" | "/") <digit>

<digit>     ::= "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9"
```

Here I introduce a grouping of terms, defined by the parentheses. Now, *expr* is defined to be two *digits* separated by either an addition, subtraction, multiplication, or division symbol. This is, however, a rather dumb grammar, in that only two *digits* can be used in the arithmetic expression, while we might want to allow any number to be used. We can extend the grammar further to allow for this:

```bnf
<expr>      ::= <number> ("+" | "-" | "*" | "/") <number>

<number>    ::= <integer> ["." <integer>]
<integer>   ::= +("0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9")
```

Here we have broken a number into two parts, a *number* and an *integer*. Where I define *integer*, I introduce a plus sign, which allows the expression which it suffixes to be repeated one or more times. Where I define *number*, I introduce the square brackets, which surround optional expressions. Thus, the following terms are captured by the expression *number*: 0, 125, 3.14, and 123.456. However, the following terms are not captured by *number*, and I will allow the reader to postulate why they are not, and how the grammar might need to be altered to capture them: .31, -12, -12.56, and -.987.

When we want to parse a language with Sprache (or any other parser, for that matter), we will first define the language in BNF, so that we can easily reference the pieces of the parser we must create, and to keep track of our progress.

## Sprache

Ultimately, once you get used to using LINQ to construct parsers, Sprache is just another library, and becoming proficient in Sprache is the same process one should be used to of learning the methods given by the library and learning how to ask questions on stack overflow.

Let's begin familiarizing ourselves by constructing a parser which can parse the string "hello" into a string "hello." This is an entirely non-useful task for Sprache, but it gets our feet wet:

```c#
Parser<string> myParser =
    from str in Parse.String("hello").Text()
    select str;

string val = myParser.Parse("hello");
```

*val* will, unremarkably, be "hello."However, the parser should be very easy to understand, especially given our understanding of the working of a LINQ expression. The method *String(string)* is a parser which parses any string you desire (in this case, we desired to parse the string "hello"). The *String* parser returns an enumerable of chars, so we need to use *Text* to turn the enumerable into a string. From there, it should be rather obvious what is going on.

Now, let's suppose we want to parse the string "hello" multiple times, separated by whitespace, and we want to know how many times "hello" appears. We can extend our parser above like so:

```c#
Parser<int> myParser =
    from str in
        Parse.String("hello").Text()
        .DelimitedBy(Parse.WhiteSpace.Many())
    select str.Count();
```

Testing this parser with the string "hello   hellohello  hello" should return a result of 4. Because of the way our parser is constructed, it is relatively straightforward to read it as "parse the string "hello" delimited by whitespace." But let's look at what's going on here. *DelimitedBy* will attempt to match the "hello" parser, and then it will look for whitespace (*WhiteSpace().Many()* is a parser itself which matches 0 or more different whitespace characters in a row), and will then look to match "hello" again and more whitespace, until the parser is no longer able to match either "hello" or whitespace, at which point it returns an *IEnumberable* containing several "hello"s. Our *select* statement can then select the *Count* of that *IEnumerable*, and thus we can obtain the number of times "hello" is parsed.

This idea of chaining parsers onto each other, as *DelimitedBy* is chained onto *String*, is the entire concept behind parser-combinators.

A slightly more complicated task might be to try to parse a variable name surrounded by whitespace (often called an "identifier"). This example is given on the Sprache GitHub page:

```c#
Parser<string> identifier =
    from leading in Parse.WhiteSpace.Many()
    from first in Parse.Letter.Once()
    from rest in Parse.LetterOrDigit.Many()
    from trailing in Parse.WhiteSpace.Many()
    select new string(first.Concat(rest).ToArray());
```

Thus, "   abc123   " should come out as "abc123". Notice how staggering several *from* statements in a row reads as though we are saying "then". For example, this parser could be read by a human as "Parse many whitespace characters, *then* parse one letter, *then* parse 0 or more letters or digits, *then* parse more whitespace, and return the first letter and the rest of the letters/digits concatenated to it".

## Our First Language

So, let's now define the grammar for a small DSL, and we'll try to parse it. Let's make a language that defines variables: we can have an identifier, followed by a colon, and then a string, and we can define as many variables as we want on different lines. Ultimately, we want to parse this into a dictionary. So, our resulting language could look like this:

```
identifier1 : "hello"
identifier2 : "world"
identifier3 : "yay parsing"
```

The BNF for the language looks like this:

```BNF
<block>        ::= <expr> *(<newline> <expr>)
<expr>         ::= <identifier> [<whitespace>] ":" [<whitespace>] <string>
<identifier>   ::= <letter> *(<letter> | <digit>)
<string>       ::= "\"" *(<any_character>) "\""
```

I'll imagine you can determine what *newline*, *letter*, *digit*, and *any_character* are. Note, though, that we technically want *any_character* to parse any character except a quotation mark.

Sprache already contains parsers for a letter, digit, and any character, so we should be all good to go from here. We already have our identifier parser, so let's add to that by constructing our string parser:

```c#
Parser<string> identifier =
    from first in Parse.Letter.Once()
    from rest in Parse.LetterOrDigit.Many()
    select new string(first.Concat(rest).ToArray());

Parser<string> stringParser =
    from first in Parse.Char('"')
    from text in Parse.AnyChar.Except(Parse.Char('"')).Many().Text()
    from last in Parse.Char('"')
    select text;
```

Here we use *Except* to add an exception to the *AnyChar* parser. In addition, we use *Text* at the end to tell sprache to convert the IEnumerable returned by *Many* into a string. Now, we can add the parser for the expressions:

```c#
//Adding to the code above:

Parser<Dictionary<string, string>> expr =
    from id in identifier
    from colon in Parse.Char(':').Token()
    from str in stringParser
    select new Dictionary<string, string>() { { id, str } };
```

*Optional* is used here - that does exactly what it says - it makes the parser optional. In addition, I introduced *Token*, which will parse whitespace before and after the *CHar* parser. Notice how we are able to reference the parsers we created earlier, and we can use the values they return to create a new object. Let's finish it off by creating the "block" parser, which is supposed to parse several expressions separated by newlines:

```c#
//Adding to the code above:

Parser<IEnumerable<Dictionary<string,string>>> block =
    expr.DelimitedBy(Parse.Char('\n'));
```

Notice  how we are not using the fancy LINQ expressions here. Because our parser fits on one line, and *DelimitedBy* returns the type that we want, then we can condense our parser a bit. Now that we're done with our parser, we should be able to parse our example file just fine into an *IEnumberable* of dictionaries containing our identifier-string pairs.

## Comma-Separated Values

CSV files are extremely popular for storing tables in plaintext, and they're very easy to parse, as you might imagine. Frequently, programs which read from CSV files desire to read the files into their own data structures. So, we'll imagine a couple different scenarios involving CSVs, and we'll look at how we can go about parsing each one.

First, I'll provide a rough CSV parser that sorts the CSV into a list of a list of strings, and from there we can talk about a custom data structure for it.

```c#
Parser<IEnumerable<IEnumerable<string>>> csv = 
    Parse.AnyChar.Except(Parse.Char(',')
        .Or(Parse.Char('\n'))).Many().Text()
        .DelimitedBy(Parse.Char(',').Token())
        .DelimitedBy(Parse.Char('\n'));
```

This parser is quite fun, as it can be written in one line, yet it parses a CSV file pretty much alright - you might notice that none of the values can contain a comma (Bonus problem: see if you can get the parser to recognize escape characters so that the user can insert commas. Later, in the JSON parser, we'll implement escape characters).

Let's digress now, and suppose we have the following simplistic data structure:

```c#
class Row
{
    public string Title { get; set; }
    public IEnumerable<string> Items { get; set; }
}
```

And let's further suppose that in a CSV, the first item of every row is the title of that row, and the remaining elements in that row are the items, corresponding to the structure above. So, we want to get a list of these rows, presumably. We can very easily modify our parser:

```c#
Parser<Row> line =
    from first in
        Parse.AnyChar
        .Except(Parse.Char(',')
        .Or(Parse.Char('\n')))
        .Many().Text()
    from comma in Parse.Char(',').Token()
    from rest in
        Parse.AnyChar
        .Except(Parse.Char(',')
        .Or(Parse.Char('\n'))).Many().Text()
        .DelimitedBy(Parse.Char(',').Token())
    select new Row() { Title = first, Items = rest };

Parser<IEnumerable<Row>> csv = line.DelimitedBy(Parse.Char('\n'));
```

Naturally, if you're just interested in obtaining the rows, then this parser works perfectly. But let's suppose we didn't want the nested lists to contain the rows, but the columns. In this case, we can do some nifty snafu:

```c#
Parser<IEnumerable<string>> line =
    Parse.AnyChar.Except(Parse.Char(',')
        .Or(Parse.Char('\n'))).Many().Text()
        .DelimitedBy(Parse.Char(',').Token());

Parser<IEnumerable<IEnumerable<string>>> csv =
    from l in line.DelimitedBy(Parse.Char('\n'))
    select Transform(l);

//Here's the Transform method:
//Assume the table is n-by-n
static IEnumerable<IEnumerable<string>>
Transform(IEnumerable<IEnumerable<string>> t)
{
    var toReturn = new List<List<string>>();
    
    for (int i = 0; i < t.ElementAt(0).Count(); i++)
    {
        for (int j = 0; j < t.Count(); j++)
        {
            if (toReturn.Count == i) toReturn.Add(new List<string>);
            if (toReturn[i].Count == j) toReturn[i].Add("");
            
            toReturn[i][j] = t.ElementAt(j).ElementAt(i);
        }
    }

    return toReturn;
}
```

*Transform* just rotates the list of lists as though it's a matrix, so we're not adding anything too special here. What if we wanted to do what we did above with the rows, but with the columns? Try modifying this code to do just that. Bonus points if you can eliminate *Transform* and perform the transformation within the parser!

## XML

Obviously, XML is a rather complex language, and a [complete BNF specification](http://www.w3.org/TR/REC-xml/) is thus very large. Therefore, we'll be using a much simpler variation of XML, which we can see below:

```bnf
<tag> ::= <single_line_tag> | <multi_line_tag>
<short_tag> ::= "<" <identifier> <whitespace> <attribute>* "/>"
<full_tag> ::= "<" <identifier> <whitespace> <attribute>* ">" <tag>*
    "</" <identifier> ">"
<attribute> ::= <identifier> "=" "\"" <any_characters> "\""
```

You might notice I'm leaving out a few unnecessary components: Expressions which are intuitively obvious aren't defined, *whitespace* is only used where necessary (we'll use *Token* prolifically to allow for flexibility on the user's part), and when defining a "full" tag, the identifier of the opening and closing tags must be the same. The latter component cannot be defined in vanilla BNF, so it's something we'll need to account for in our code.

Instead of writing this code out, I'll reference the [XML example](https://github.com/sprache/Sprache/blob/master/src/XmlExample/Program.cs) which is included with Sprache, and I'll explain the new elements and solutions found there.

Right off the bat, looking at the *Document* parser, we're introduced to a new use of LINQ. As I alluded to previously, any LINQ query can be written in one line without the LINQ statements. Here's the critical portion of line 98:

```c#
Node.Select(n => new Document { Root = n })
```

To make this easier to comprehend, we can write it out using the regular LINQ notation we're familiar with:

```c#
from n in Node
select new Document() { Root = n };
```

In fact, the LINQ statements we've been are just a shorthand (or perhaps more of a "paraphrasing," as they tend to be longer) for the inline notation. *Document*
very well could have been written using solely the LINQ statements, and that would have looked like this:

```c#
public static readonly Parser<Document> Document =
    from leading in Parse.WhiteSpace.Many()
    from doc in from n in Node.End()
                select new Document() { Root = n }
    select doc;
```

The next parser up from *Document* is *Item*. We'll ignore the code regarding the comments (if you're interested in this, please see my post [Parsing Comments with Sprache](https://ianwold.silvrback.com/parsing-comments-with-sprache)). This makes it easy to see that an *Item* is either a *Node* cast as an Item, or a *Content*, and a *Node* (looking above in the document) is either a short or full node.

Looking at the *ShortNode* parser, we can see it seems completely familiar, except that the LINQ expression is used as an argument to the *Tag* method. The *Tag* method returns a parser that parses a greater than and less than sign before and after the parser you specify. This abstraction allows us to write cleaner code.

*FullNode* is fun for a couple reasons. First, look at how they solved the issue of requiring opening and closing tags to be named the same with the *EndTag* method. In addition, notice the use of the *Ref* parser. In *FullNode*, we need to use *Item*, but it has obviously not yet been created. *Ref* allows us to reference a parser later in the document, thus allowing us to create some recursive or ambiguous grammars with Sprache.

## JSON

JSON, or JavaScript Object Notation, is kind of like XML. It's a way of storing data in plaintext (in a key-value pair manner) which is also easily readable by a human. In addition, it's very easy to construct a parser for it. The BNF form is very clear and concise - here, I have transcribed the [informal definition on json.org](http://json.org/) into the more formal BNF notation which we have been using in this tutorial:

```bnf
<object>    ::= "{}" | "{" <members> "}"
<members>   ::= <pair> | <pair> "," <members>
<pair>      ::= <string> ":" <value>
<array>     ::= "[]" | "[" <elements> "]"
<elements>  ::= <value> | <value> "," <elements>
<value>     ::= <literal> | <array> | <object>
<literal>   ::= <string> | <number> | <bool> | "null"
```

An example of a valid JSON file might be the following:

```json
{
  "firstName": "John",
  "lastName": "Smith",
  "age": 25,
  "address": {
    "streetAddress": "21 2nd Street",
    "city": "New York",
    "state": "NY",
    "postalCode": "10021-3100"
  },
  "phoneNumbers": [
    {
      "type": "home",
      "number": "212 555-1234"
    },
    {
      "type": "office",
      "number": "646 555-4567"
    }
  ]
}
```

This should allow us to construct the data structure we're going to store our data in (called an Abstract Syntax Tree):

```c#

public class JSONValue {}

public class JSONObject : JSONValue
{
    public Dictionary<string, JSONValue> Pairs { get; set; }

    public JSONObject(IEnumerable<KeyValuePair<string, JSONValue>> pairs)
    {
        Pairs = new Dictionary<string, JSONValue>();
        if (pairs != null)
            foreach (var p in pairs)
                Pairs.Add(p.Key, p.Value);
    }
}

public class JSONArray : JSONValue
{
    public List<JSONValue> Elements { get; set; }

    public JSONArray(IEnumerable<JSONValue> elements)
    {
        Elements = new List<JSONValue>();
        if (elements != null)
            foreach (var e in elements)
                Elements.Add(e);
    }
}

public class JSONLiteral : JSONValue
{
    public string Value { get; set; }

    public LiteralType ValueType { get; set; }

    public JSONLiteral(string value, LiteralType type)
    {
        Value = value;
        ValueType = type;
    }

    pubilc static enum LiteralType
    {
        String,
        Number,
        Boolean,
        Null
    }
}
```

To implement the parser, we'll start from the bottom and work our way up, as we usually do. Literal values are expressed nicely by our JSONLiteral class, which stores every value as a string, and also keeps track of the type of literal it is. Parsing them all out is a bit of a pain, so I'll post each parser here and explain it briefly.

```c#
Parser<JSONLiteral> JNull =
    from str in Parse.IgnoreCase("null")
    select new JSONLiteral(null, JSONLiteral.LiteralType.Null);

Parser<JSONLiteral> JBoolean =
    from str in Parse.IgnoreCase("true").Text()
    			.Or(Parse.IgnoreCase("false").Text())
    select new JSONLiteral(str, LiteralType.Boolean);
```

Parsing a literal null or boolean value isn't all too complicated. We just need to parse the strings which represent them, ignoring the case, and return new JSONLiteral objects.

```c#
Parser<string> JExp =
    from e in Parse.IgnoreCase("e").Text()
    from sign in Parse.String("+").Text()
                 .Or(Parse.String("-").Text())
                 .Optional()
    from digits in Parse.Digit.Many().Text()
    select e + ((sign.IsDefined) ? sign.Get() : "") + digits;

Parser<string> JFrac =
    from dot in Parse.String(".").Text()
    from digits in Parse.Digit.Many().Text()
    select dot + digits;

Parser<string> JInt =
    from minus in Parse.String("-").Text().Optional()
    from digits in Parse.Digit.Many().Text()
    select (minus.IsDefined ? minus.Get() : "") + digits;

Parser<JSONLiteral> JNumber =
    from integer in JInt
    from frac in JFrac.Optional()
    from exp in JExp.Optional()
    select new JSONLiteral(integer +
                           (frac.IsDefined ? frac.Get() : "") +
                           (exp.IsDefined ? exp.Get() : ""),
                           LiteralType.Number);
```

Parsing a number is much more exciting. We need to account for integers, decimals, negation, and 'e'. The code above for *JNumber* knows we need at least an integer, and can be optionally followed by the fraction or the exponential term. Notice that *Optional* returns a special object which may or may not be defined. Thus, we need to check whether it is defined with *IsDefined* before we can *Get* its value.

```c#
List<char> EscapeChars = new List<char>
    { '\"', '\\', 'b', 'f', 'n', 'r', 't' };

Parser<char> ControlChar =
    from first in Parse.Char('\\')
    from next in Parse.EnumerateInput(EscapeChars, c => Parse.Char(c))
    select ((next == 't') ? '\t' :
            (next == 'r') ? '\r' :
            (next == 'n') ? '\n' :
            (next == 'f') ? '\f' :
            (next == 'b') ? '\b' :
            next );

Parser<char> JChar =
    Parse.AnyChar
    .Except(Parse.Char('"')
    .Or(Parse.Char('\\')))
    .Or(ControlChar);

Parser<JSONLiteral> JString =
    from first in Parse.Char('"')
    from chars in JChar.Many().Text()
    from last in Parse.Char('"')
    select new JSONLiteral(chars, LiteralType.String);
```

To parse a string, we want to make sure that we allow for control characters (the control characters are all given on [json.org](http://json.org)). As you can see, the string will be zero or more characters, which are in turn any character except a quotation mark or the escape character. Where *ControlChar* is defined, *EnumerateInput* is used on our list *EscapeChars*. This instance of *EnumerateInput* will return the following parser:

```c#
Parse.Char( '\"').Or(Parse.Char('\\')).Or(Parse.Char('b')) ...
```

That is, it chains each element in *EscapeChars* along as the parser *Parse.Char()* using the *Or* parser.

```c#
Parser<JSONLiteral> JLiteral =
    JString
    .XOr(JNumber)
    .XOr(JBoolean)
    .XOr(JNull);
```

Finally, we're able to piece them all together to form our *JLiteral* parser. Luckily, this is half of our entire parser!

As you can see from our JSON BNF, the rest of the grammar is recursive. That is, self-referential. This is where *Ref* will come in handy. We need to implement objects and arrays, and those two **plus** literals will be defined as a value. So, let's define our *JValue* parser, and proceed from there.

```c#
Parser<JSONValue> JValue =
    Parse.Ref(() => JObject)
    .Or(Parse.Ref(() => JArray))
    .Or(JLiteral);
```

Here, we are using *Ref* to reference our yet-undefined *JObject* and *JArray* parsers. Of course, we've already created our *JLiteral* parser, so we do not need to use *Ref* to access it.

Now we just need to parse JSON arrays and objects. For convenience, let's recall the portion of the JSON BNF which defined them:

```bnf
<object>    ::= "{}" | "{" <members> "}"
<members>   ::= <pair> | <pair> "," <members>
<pair>      ::= <string> ":" <value>
<array>     ::= "[]" | "[" <elements> "]"
<elements>  ::= <value> | <value> "," <elements>
```

We can see that *arrray* and *object* look very much alike, and the definition of *array* appears to be a tad more simple. Therefore, we should write our *array* parser first, and we can copy it down to create our slightly more complicated *object* parser.

```c#
Parser<IEnumerable<JSONValue>> JElements =
    JValue.DelimitedBy(Parse.Char(',').Token());

Parser<JSONValue> JArray =
    from first in Parse.Char('[').Token()
    from elements in JElements.Optional()
    from last in Parse.Char(']').Token()
    select new JSONArray(elements.IsDefined ? elements.Get() : null);
```

Notice how our *JElements* parser almost perfectly matches the definition of *elements* in the BNF. *DelimitedBy* will parse any number of *JValue* here, so long as they are separated by commas - this removes our need to call *JElements* recursively. Our *JArray* parser, then, just encases the *JElements* parser in square brackets. If we desired we could combine the parsers into one. The reason I separated them here, however, was to demonstrate the close relationship between BNF and parsers like Sprache. Here is how the combined parsers would look:

```c#
Parser<JSONValue> JArray =
    from first in Parse.Char('[').Token()
    from elements in
        JValue.DelimitedBy(Parse.Char(',').Token()).Optional()
    from last in Parse.Char(']').Token()
    select new JSONArray(elements.IsDefined ? elements.Get() : null);
```

Now, we can move on to write our *JObject* parser.

```c#
Parser<KeyValuePair<string, JSONValue>> JPair =
    from name in JString
    from colon in Parse.Char(':').Token()
    from val in JValue
    select new KeyValuePair<string, JSONValue>(name.Value, val);

Parser<IEnumerable<KeyValuePair<string, JSONValue>>> JMembers =
    JPair.DelimitedBy(Parse.Char(',').Token());

Parser<JSONValue> JObject =
    from first in Parse.Char('{').Token()
    from members in JMembers.Optional()
    from last in Parse.Char('}').Token()
    select new JSONObject(members.IsDefined ? members.Get() : null);
```

By now, this should all be trivial to you - especially considering *JObject* and *JMembers* are copies of *JArray* and *JElements*, respectively. With that, we should now be able to parse any document which conforms to the JSON standard. Notice that every JSON document is itself a single JSON object. Thus, given a JSON document, we would parse it with our *JObject* parser.

If you would like to see the parser in full, there is a version [on my GitHub](https://github.com/IanWold/SpracheJSON/blob/master/SpracheJSON/JSONParser.cs).

## My Work With Sprache

As I mentioned above, I've been working with Sprache for three years now, after seeing a presentation about it at the [Twin Cities Code Camp](http://twincitiescodecamp.com/), which is totally awesome and you should all go (it's even free).

I've contributed to Sprache by adding a [comment parser](https://ianwold.silvrback.com/parsing-comments-with-sprache), I've published a [JSON serializer/mapper](https://github.com/IanWold/SpracheJSON), and I'm working on a [Markdown parser](https://github.com/IanWold/SpracheDown).

I've used Sprache in small amounts in a couple other projects, and I enjoy using it wherever I'm able. An idea suggested to me at an [Iowa Code Camp](http://iowacodecamp.com/), which is also awesome and free and you should all go, was to write a tool to convert BNF into Sprache. I haven't done anything with this concept yet, but that is further work that could be done - if you're feeling the Sprache bug and you want to tackle that, go right ahead!

## Further Reading

I'll keep this list updated as I encounter more on the interwebs. This list should provide a good base to continue exploring the topics introduced in this tutorial.

* MSDN has [extensive documentation](https://msdn.microsoft.com/en-us/library/bb397926.aspx) of LINQ.
* [Wikipedia](https://en.wikipedia.org/wiki/Parser_combinator) provides an excellent starting point for learning more about parser-combinators
* The [Sprache GitHub](https://github.com/sprache/Sprache) links several examples, projects, and other tutorials.
* The [StackOverflow tag](http://stackoverflow.com/unanswered/tagged/sprache) receives regular traffic.