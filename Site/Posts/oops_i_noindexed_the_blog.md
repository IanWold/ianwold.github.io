;;;
{
	"title": "Oops, I noindexed the Blog!",
	"description": "File under 'mistakes made and lessons learned'",
	"date": "26 January 2025",
	"contents": false,
	"hero": "photo-1641909267244-70d7f51ff7c5",
    "topics": ["Blogging"],
    "related": [
        { "title": "90% of my Homepage was Useless", "description": "In a few days, I reduced the size of my homepage to 10% of what it had been, and sped it up by 50-66%.", "fileName": "90_of_my_homepage_was_useless" },
        { "title": "Why I Have This Blog", "description": "Reflecting on the last year of blogging.", "filename": "why_i_have_this_blog" },
		{ "title": "I Have a Blogroll Now!", "description": "An actual blogroll, not just a blog post with a bulletted list of links!", "fileName": "i_have_a_blogroll_now" }
    ]
}
;;;

Many months ago, I set up [Umami](https://umami.is/) to track views on my blog. It doesn't track users, just counts page hits and referring sources and the like. The results were encouraging: I do get a fair amount of traffic. Hello! I realized too that about 50% of it is from mobile devices. There goes my theory that I don't need to make it look good on mobile...

Last month I noticed a trend: I was getting many fewer clicks and almost no traffic was coming from Bing or Google (having a lot of .NET content I came up in Bing results a fair amount). This was odd but I attributed it to the holiday season. I was still getting plenty of traffic from Yandex and Baidu.

Well, it happens that the decrease in traffic coincided with my release of [StaticBlogroll](https://github.com/IanWold/StaticBlogroll), a repo/template I made to encourage others to share a public blogroll. It's intended to be deployed with GitHub Pages, and I figured it would be good to include `noindex, nofollow` by default in the `robots.txt` for this site, lest others' articles might appear under my domain on Google. Unlikely, but I didn't want the chance.

Well, as it happens, GitHub likes to deploy Pages sites under your domain if you've already registered it. Thus, my blogroll deployed to [ian.wold.guru/Blogroll](https://ian.wold.guru/Blogroll). No problem by my book!

Well, as it also happens, Bing and Google will read _every_ `robots.txt` file on your site. As it further happens, `noindex nofollow` in _any_ `robots.txt` overrides _everything_ else.

Oops.

Two learnings:

1. _That's_ why I had fewer hits, and
2. Yandex and Baidu are (maybe predictably) not following best practices.

Luckily, Bing and Google have webmaster tools with which you can notify any updates to these files, so the problem should be corrected. I've got no clue if some of my previously-most-popular articles will regain their positions though. I'm particularly happy with my article on [E2E encryption in Blazor WASM](https://ian.wold.guru/Posts/end_to_end_encryption_witn_blazor_wasm.html) so I hope that one recovers. Finally, I've updated the default for StaticBlogroll and I've edited my (now only) `robots.txt` for this site to exclude `/Blogroll`.
