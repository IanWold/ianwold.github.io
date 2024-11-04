;;;
{
	"title": "Publish Your Blogroll Now",
	"description": "I've done it and so can you; in fact, I've made it easier for you - way easier!",
	"date": "3 November 2024",
	"contents": false,
	"hero": "photo-1602496875770-0b3d129671e6",
    "topics": ["Blogging", "How-To", "Projects"],
    "related": [
		{ "title": "Thing I Made: FreePlanningPoker.io", "description": "I made a free planning poker tool and named it aptly.", "fileName": "free_planning_poker" },
		{ "title": "90% of my Homepage was Useless", "description": "In a few days, I reduced the size of my homepage to 10% of what it had been, and sped it up by 50-66%.", "fileName": "90_of_my_homepage_was_useless" },
		{ "title": "Giscus Is Awesome", "description": "I can add comments to my statically generated blog? Using GitHub Discussions?? For Free??? And it works????", "fileName": "giscus_is_awesome" }
    ]
}
;;;

Do you read blogs? Of course you do! How do you keep those blogs saved? How do you keep up with their latest posts? How do you share these blogs with everyone else?

I recently became motivated (by the realization that [I keep forgetting about my RSS reader](https://ian.wold.guru/Posts/i_have_a_blogroll_now.html)) to answer these questions by creating a blogroll. But not just any blogroll, no! A _super cool_ blogroll - a standalone, statically generated page that lists all the latest posts from the blogs I follow. It's like an RSS reader but way better: I can share it with the internet, and I set my browser's homepage to the collection of latest posts. I can keep up with my reading and share it with everyone.

The best part is you can too! I developed [StaticBlogroll](https://github.com/IanWold/StaticBlogroll), a very creatively-named template repo you can use to get your own blogroll page going in no time flat. It uses GitHub Actions and Pages, so there's no work beyond just a little config to set up.

# Okay, but why do I need a blogroll?

I'm a big advocate that everyone should be maintaining a blogroll. Not super actively - maybe more passively - but you should definitely have one. No doubt there's dozens of blogs a year you tune into - an article here to get help on an issue, an article there to learn about a topic, articles for fun or insight or curiousity.

A public blogroll helps others discover the blogs you enjoy. Even if only a few friends end up looking at your blogroll, you're helping the author find new readers at the same time you're helping your friends find something new. Even just recognizing an author on your blogroll is a kind gesture that shows your appreciation for their work.

If you happen to have your own blog, keeping a blogroll is an even better investment. Connecting your readers with blogs of similar interests gives a better, deeper experience for your readers. As for me, I feel like I can represent more aspects of my interests and personality by linking to other blogs that are better-focused than my blog on topics which I enjoy and influence my writing here.

So what's the utility in a _statically generated_ blogroll? The only thing I've added to take advantage of this setup (yet) is to read the latest posts in from the RSS feeds on my blogroll. As I mentioned, this was selfish because I needed an easy way to read them. I'm certain that could be handy for you as well, but I also like the idea that this makes it easier for visitors to my blogroll to skim through for articles they might be interested in.

# That sounds awesome! How do I get one?

I've published [StaticBlogroll](https://github.com/IanWold/StaticBlogroll) as a template repository on GitHub; you can [create a new repo](https://github.com/new?template_name=StaticBlogroll&template_owner=IanWold) from that template and [follow the setup steps](https://github.com/IanWold/StaticBlogroll?tab=readme-ov-file#setup) on the readme - these should take no more than 5 minutes to get all set up!

The way it works is very simple: there's a `config.json` at the root level that has all the configurations. This is the name of the page, some headers and the like, but most importantly the list of `feeds`. These are the URLs to the RSS feeds of the blogs you want on your roll.

From there, there is a GitHub Actions workflow that generates the static site based on this config, though there's a few other files (templates and static files) you can peruse at the root level if you want to customize it further. The workflow runs whenever you push to `main` _and_ it runs once a day at midnight to regenerate the latest posts.

After you've gone through the simple steps to configure Actions and Pages, you'll have a blogroll page of your own! Woohoo! The only next step would be to share it with the world. I set up a discussion topic on my repository to showcase blogrolls made with it - please [add your own](https://github.com/IanWold/StaticBlogroll/discussions/new?category=showcase) so I can see the cool blogs y'all read.

If you'd like a reference with some further customizations, check out [my blogroll repo](https://github.com/IanWold/Blogroll) made off of this template.
