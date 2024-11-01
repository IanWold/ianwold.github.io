;;;
{
	"title": "I Have a Blogroll Now!",
	"description": "An actual blogroll, not just a blog post with a bulletted list of links!",
	"date": "1 November 2024",
	"contents": false,
	"hero": "photo-1639695854238-f80b14594a3a",
    "topics": ["Projects", "Blogging"],
    "related": [
		{ "title": "I've IndieWebbed My Site", "description": "A small, loose collection of formats and protocols, IndieWeb is an interesting supplement (maybe alternative) to social media", "fileName": "ive_indiewebbed_my_site" },
		{ "title": "My (Continuing) Descent Into Madness", "description": "It started simply enough, when I asked myself if I should try an IDE other than Visual Studio. Mere months later, I'm now using a tiling window manager. This is the story of my (continuing) descent into madness.", "fileName": "my_continuing_descent_into_madness" },
		{ "title": "Why I Have This Blog", "description": "Reflecting on the last year of blogging.", "fileName": "why_i_have_this_blog" }
    ]
}
;;;

A bit ago I collected [a list of links](https://ian.wold.guru/Posts/book_club_9-2024.html) to blogs that I enjoy tuning into. I think this is a good list of blogs; these are the ones I enjoy coming back to with some regularity - even the ones which haven't posted in a while have great articles that are worth revisiting.

There's a couple problems here though. First, I'm generally averse to blog posts with lots of updates; I think a post is a relatively static thing, so updates should be limited to correcting spelling/grammar errors and the occasional clarification. Thus, as I collect more blogs I'm disinclined to keep _that_ page updated. The second problem is reading them: they're links in my browser and it's quite sporadic how I go about reading them. I've got most of them saved in an RSS reader on my phone but I tend to go months without remembering that that exists.

So I want a couple things: I want a dynamic page that I can update with new RSS feeds I want to tune into, and I want an easy way to be able to catch updates from these blogs. The obvious step is to add a page at the top of this site that contains the list; this isn't a post so I can update it as frequently as I want (like my [now page](https://ian.wold.guru/now.html)), but then this still requires that I go onto this page and click through to read the blogs. That's not much better than having a favorites bar, though it does make the blogroll public.

It's obviously advantageous that these are RSS feeds; I should be able to set up some sort of scheme to get the latest posts from them. To me, the best place for this would be my browser homepage; I like to start my day a bit lighter before an onslaught of morning meetings, so if I could have a reading list right when I get started I'll be more inclined to tune in more frequently.

As it happens, I do have a project that I haven't finished - a [static site generator](https://github.com/IanWold/Metalsharp) which I use to generate this blog. I already know how to hook that up with GitHub Actions and Pages to get a static site really easy, so it really shouldn't be any work to set up a site that can host my bogroll and pull the latest posts from their feeds!

Indeed, it wasn't difficult to set up a first pass. I just have a JSON with a list of RSS links, and a C# script to download these RSSes, parse them, and build the site. It's just two pages: a list of blogs and a list of their latest articles (the latter page now being my browser homepage). What a deal! I didn't want to get bogged down in UI design or any super-fancy coding, so I stuck to the C# script setup that I know works, and I used [MVP.css](https://andybrewer.github.io/mvp/) to give an attractive style to plain, old HTML. Super simple and out the door in no time!

This did replace [Daily Grug](https://ian.wold.guru/Posts/daily_grug.html) as my homepage, so I'm going to need to get the daily grug quote inserted into the page.

I'll encourage you all to check out [my blogroll](https://ian.wold.guru/Blogroll/)! If you've got suggestions for blogs I should follow (or your own blog), comment below or [reach out to me](https://ian.wold.guru/connect.html) on any of my other channels, I'd love to find more.

In future I'll be putting in a bit of work to set up a template repo in GitHub to make it easy for anyone to use this scheme to publish a blogroll - stay tuned!