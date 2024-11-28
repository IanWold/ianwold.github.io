;;;
{
	"title": "Book Club 11/2024: No",
	"description": "On forming quality opinions and saying 'no'",
	"date": "28 November 2024",
	"contents": false,
	"hero": "photo-1672309558498-cfcc89afff25",
    "topics": ["Processes", "Industry", "Learning"],
	"series": "Book Club",
    "related": [
		{ "title": "Book Club 10/2024: Fallacies of Distributed Computing", "description": "Putting a pin in the series I wrote this year on the topic of these Fallacies", "fileName": "book_club_10-2024" },
		{ "title": "Book Club 9/2024: Blogroll: Labor", "description": "I've been dying to ask you; I really want to know: where do you get your ideas from?", "fileName": "book_club_9-2024" },
		{ "title": "Book Club 8/2024: Labor", "description": "Using Labor Day as an excuse to wonder about some recent trends in the industry", "fileName": "book_club_8-2024" }
	]
}
;;;

Happy Turkey Day gobble gobble! Those of us in the US are celebrating Thanksgiving Day, and today I'm thankful for all my colleagues who make a habit of saying "no". Does the period go inside the _"_ in this case? I've always been unclear.

Anyway, I'm not a big fan of "yes" - "yes, we can add all those features", "sure you can add those extra layers to the architecture", "Decompose our microservices into nanoservices in spite of only having 12 users? You betcha!" These are kind of silly examples admittedly but it's a common picture that's been painted by plenty of opinion-havers in our industry before me. This isn't a problem unique to ours too, the so-called yes men are everywhere.

I spend a lot of time speaking with my colleagues about the importance of qualifying knowledge, learnings, and opinions with conditions; too many of us are too absolute in our thinking. Instead of holding the opinion "we should _always_ use onion architecture" it's better to assert "_if_ we have a large monolith that requires robust domain segregation, _then_ onion architecture is preferred." Whether you hold either opinion or not, I hope it's obvious that we would all prefer to work with the engineer holding the second. When we learn new things or develop new strategies or _whathaveyou_, the success always happened in the context of some environment, and the facts of that context are as much a part of the success as the actual good thing that was implemented. Our work is more robust when we incorporate those facts into our learnings in these qualifying conditions.

This is a way of not saying "yes" at each opportunity. In my example, we change from "yes, let's use onion architecture" to "maybe, let's see if we should use onion architecture." The same comes from the other direction with "no" statements. Think of all the software engineering opinions you hold, and let me ask you how many are "no" opinions? I mean propositions like "don't use onion architecture" or "don't put opening curlies on the same line" or the like. Now of those "no" opinions you have, how many of them are just the opposite of a concrete "yes" opinion you hold? I mean, for example, if you hold the opinion "don't put opening curlies on the same line" you also think "always put opening curlies on the next line." When I did this exercise I found that a lot of my "no" opinions are just mirrors of other "yeses."

These are equally unproductive, and transforming them into conditionals will do good. It's clear then that offering a "no" opinion, or saying "no" in any way, is always best served by making sure it's the right "no" for the context, and being open to the possibility of a "yes" instead. That said, there's infinitely more "no" than "yes" to be had at any given time. Think about the last trivial bug you had to solve at work - there's a million choices you could take that are obviously wrong. You didn't need to rewrite the whole application, you didn't need to distribute it out into a separate service, you didn't need to introduce a cache. Well, probably not for all of those, but you get the point: there are very few good paths and an infinite number of bad paths.

So "no" positions are significantly more common, and you're going to be in a better position defaulting to "no." This comes back around to our starting point being annoyed by our yes-colleagues: if _most_ paths before us are bad, they get us into trouble indiscriminately saying yes. If only a few paths are good paths forward, skepticism and carefulness behoove us. Being good at saying "no" helps to keep us on the right course.

On a final note, I've heard from a few folks that their interest in the topic of "no" is more inclined towards a largely unexplored field of ethics within software engineering. I think there's a growing sense that we need some manner of organization in this area; our industry has kind of been flying by the seat of its pants for decades, and almost every other profession is better organized with a proper code of ethics. I can see how the "no" topic fits in here: what are our ethical obligations as to when to say "no?" I'll confess that I'm not terribly interested in this question at the moment, I was more interested in exploring how "no" can shape our outlook on the problem space, and the space itself. Certainly though I'll link some resources below.

* [Would it help if I spoke to your management? - Adam Ralph](https://adamralph.com/2019/10/22/would-it-help-if-i-spoke-to-your-management/)
* [No one can teach you to have conviction - Ben Kuhn](https://www.benkuhn.net/conviction/)
* [Grug on Saying No](https://grugbrain.dev/#grug-on-saying-no)
* [Saying "NO" - A Superpower - Anand Safi](https://dzone.com/articles/the-power-of-saying-no-a-superpower)
* [I'm David Heinemeier Hansson, Basecamp CTO, and This Is How I Work](https://lifehacker.com/im-david-heinemeier-hansson-basecamp-cto-and-this-is-1820470919)
* [Not Having Problems - Lucas F. Costa](https://lucasfcosta.com/2020/09/05/not-having-problems.html) (loosely related)
* Finally a general link to [Renegade Otter](https://renegadeotter.com/), where each blog post is a "no"

Video/Podcast:

* [Say No by Default - REWORK](https://www.youtube.com/watch?v=pcGW-FiapG8)
* [Focusing is about saying no - Steve Jobs](https://www.youtube.com/watch?v=H8eP99neOVs)

On Ethics:

* Uncle Bob has an interest in this space:
    * [Software Engineering Ethics Manifesto](https://www.youtube.com/watch?v=4T0ivYGSNpg)
    * [The Scribe's Oath](https://www.youtube.com/watch?v=Tng6Fox8EfI)
* [So You Can Sleep At Night - Jonathan Rothwell and Steve Freeman](https://www.youtube.com/watch?v=A5umy4lUOOY)
* ["I'm sorry Dave, I can't do that": Ethics in Software Development - Dr. Morgan Leigh](https://www.youtube.com/watch?v=mw-OAGCcmSA)
* [Software development, responsibility and ethics: the coming crisis - Richard Fontana](https://www.youtube.com/watch?v=___k3hCQHEU)