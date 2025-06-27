;;;
{
	"title": "I Like Petite-Vue",
	"description": "Simplicity is good, focusing on what's necessary is better.",
	"date": "27 June 2025",
	"contents": false,
	"hero": "photo-1463725876303-ff840e2aa8d5",
    "topics": ["Architecture", "DevEx", "Standards"],
    "related": [
		{ "title": "Giscus Is Awesome", "description": "I can add comments to my statically generated blog? Using GitHub Discussions?? For Free??? And it works????", "fileName": "giscus_is_awesome" },
		{ "title": "Using Interfaces", "description": "I'm on a quest to make it happen less", "fileName": "using_interfaces" },
		{ "title": "I've Stopped Using Visual Studio", "description": "... mostly. And so can you!", "fileName": "ive_stopped_using_visual_studio" }
    ]
}
;;;

I like simple things in software: simple engineering, simple architecture, simple tools. The primary, almost entire, focus of our jobs in software is to wrangle the [demon spirit complexity](https://grugbrain.dev/), the best tool for which is a focus on necessity: what is _necessary_ to build a thing, and what is _necessary_ about that thing?

If you need a personal site (hello!) then a statically generated page is much simpler than writing your own server. Server not necessary! If you are generating a static site, you might find that [90% of it isn't necessary](https://ian.wold.guru/Posts/90_of_my_homepage_was_useless.html) - and that's bytes, not content! If you need a page with interactivity, React is probably overkill for you; [Alpine](https://alpinejs.dev/) might be all you really need.

I like Alpine a lot! In order to get all of the interactivity you'll ever really need for most simple applications, it's got the right level of learning at a low package size. It's easy to recommend for a lot of smaller applications, but there's some limitations. _Very_ small pages make use of query selectors just fine, and applications on even the _slightly_ larger side might, depending on the domain, be a bit wary of adopting a tool which might not be able to satisfy changing requirements long-term. A lot of applications don't have vastly changing requirements over their lives. Some do. Products almost certainly do.

These are two problems that are solved by [Petite Vue](https://github.com/vuejs/petite-vue) (in addition to there being, or maybe having been, a ["minimal" memory leak in Alpine](https://markaicode.com/alpine-js-vs-petite-vue-performance-comparison/)). As its name suggests, the project is a subset of Vue that provides a similar functionality as Alpine. After resolving dependencies it has on the main Vue project it still comes in well under the size that Alpine does and it runs faster! It lacks support for extensions like Alpine has, but being a subset of Vue, you can swap Petite Vue out for regular Vue and none of your code will need to change. This makes it a lot easier to recommend for very small projects as well as those which need to accommodate changing requirements. It satisfies more needs.

The neatest part is that it's complete; you'll see no commits in three years (as of this writing) on their repo! So much software can't ever be completed, too frequently the product is not small or concise enough. We often look to the commit frequency on a project to be a determiner of status, that a product in "active" development is more worth investing in, but this seems entirely backwards to me. If they're always fixing bugs, isn't this an indicator that the project is unstable? If they're always introducing new required features, isn't this an indicator that the project is underbaked? If they're always introducing new unnecessary features, isn't this an indicator that the project is bloating?

So I like Petite Vue. It's a role model, I think, for the best kind of software development: it exemplifies the focus on necessity.
