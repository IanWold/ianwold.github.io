;;;
{
	"title": "Meanwhile in the Win Console ... Minesweeper!",
	"author": "Ian Wold",
	"date": "23 August 2017",
	"description": "My tyranny of classic game implementations in the console expands. Knows it no end?"
}
;;;

My tyranny of classic game implementations in the console expands. Knows it no end?

## Why?

I've done [2048](https://github.com/IanWold/Console2048), [Minesweeper](https://github.com/IanWold/ConsoleMinesweeper), Solitaire (coming soon to GitHub), and a number of others. They only take a few hours and, one ultimately concludes, they are more or less exactly the same thing. I've found the exercises of producing these classic desktop arcade games from scratch to be very useful. The obvious excuse, besides boredom, is in line with the addage "practice makes perfect": just as a musician must continuously practice their instrumentation skills, a programmer must practice their programmering skills. This is a bit of a cop-out. My admirable high school band director would quip "practice makes *permanent*; *perfect* practice makes perfect," and so I extend his correction across the analogy to its relation to programming. Practice is not enough in itself, perfect practice is necessary.

The implementation of these sub-400-lines games are intuitively obvious, even to non-programmers. It is very easy to perfect their implementation. There is no database, no business logic, a single file, and the most complicated data structure you should need to rely on is a two-dimensional array.

Not only does this allow you to craft a perfect implementation of the game, it allows you to craft a perfectly horrendous implementation. C# 7.0 introduced language-level tuples, and my latest creation is a good testament as to why this was not the best decision. It could easily have been a perfect testament to that fact (speaking of perfection), but I desired to preserve my sanity.

## What?

Now, you might not agree with me that C#7 tuples were a bad idea, and you might even still hold that opinion after you read my code. Unfortunately, most countries will legally allow you to hold that opinion. One of my goals, as I've briefly expounded upon, was to prove the opposite.

Another goal was more interesting for the programmer looking to sharpen their skills: I wrote the entire program in one sitting without testing it. When I did hit F5, I am proud to report, it worked exactly as I intended. I shouldn't have to mention that this really isn't best practice, but it's a fun accomplishment. It probably helps your brain practice thinking sorts of skills, and so I assume it is helpful to the practicing programmer. Practgrammer, if you will.

There's really not too much more to write here, but if you've stumbled across this I encourage you to check out the projects I've linked to on [my GitHub](https://github.com/IanWold). I'm also developing (at a snail's pace) a [game engine](https://github.com/IanWold/OutrageEngine) for the Windows console. I've got no design notes and I hit programmer's block with some frequency, but you might find it not *completely* lacking.
