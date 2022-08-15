;;;
{
	"title": "SpracheJSON",
	"author": "Ian Wold",
	"date": "10 April 2015",
	"description": "A small library serializing and deserializing JSON using Sprache."
}
;;;

I've been meaning to write about this for a while now, but college got in the way of that. I wrote a deal I called SpracheJSON to parse JSON text into C# objects with Sprache, and it's kinda neat. I also played with the GitHub pages feature on this project, but that's not really interesting.

I got this thing to the point where is parses the JSON just fine, no problem. But I also figured it made sense to serialize between JSON and C# objects. Like the other parsers I've written have ended up, it's got its own AST to throw the JSON into, but I can't imagine I'd ever want to use generic objects to store my data (isn't that the whole point of JSON?) That said, I've never been a fan of ISerializable, so I've got my own custom serializer thing going on. Obviously that makes it kinda imperfect, but that's how the world goes, I suppose.

In the future I'm going to use ISerializable to make that part of the project go easier, but until then, I've got a nice half-baked parser here. I'm not going to detail the functionality of the library too much, but it's pretty slick, so you should definitely go check it out [here](https://github.com/IanWold/SpracheJSON).
