;;;
{
	"title": "Looking Beyond GitHub",
	"description": "In the process of reevaluating my tools, GitHub now appears on the chopping block",
	"date": "21 February 2026",
	"contents": false,
	"hero": "hoto-1556075798-4825dfaaf498",
    "topics": ["Tooling", "DevEx"],
    "related": [
		{ "title": "I've Switched to Linux", "description": "Just in the nick of time as Windows is speedrunning 'Worst OS Ever,' all of my development activities are now supported on Linux.", "fileName": "ive_switched_to_linux" },
		{ "title": "I've IndieWebbed My Site", "description": "A small, loose collection of formats and protocols, IndieWeb is an interesting supplement (maybe alternative) to social media", "fileName": "ive_indiewebbed_my_site" },
		{ "title": "I Have a Blogroll Now!", "description": "An actual blogroll, not just a blog post with a bulletted list of links!", "fileName": "i_have_a_blogroll_now" }
    ]
}
;;;

Like most folks in our industry, I've been using GitHub extensively for a long time, and I've become very familiar with it (and, as a consequence, Git). For the most part, I do like doing version control in Git. There's a number of efforts afoot to try to propose alternate VC systems that solve common issues with Git, but I don't really see these gaining a huge traction. Then again, there's a lot of folks reevaluating a lot of common practices these days.

GitHub is a huge advantage gained for using Git: the tool makes it incredibly easy for me to host public and private projects, with all the CI bells and pages whistles I need. Nonetheless, I'm going to be working on getting myself off of GitHub over the next several months. Instead, I'll be spinning up my own [Gitea server](https://about.gitea.com/) on Railway. Why? Ownership, cost, and control.

I have a very significant amount of code in GitHub: some public, mostly private, some in between. GitHub has fine features to facilitate collaboration on code, but nothing so granular as what I can do with my own server. Controlling code access is one thing, but being able to have easier control over my CI/CD infrastructure is also appealing. I might be getting some cost saving in some areas, but more importantly it's easier to set up custom infrastructure with my own server than fooling around with GitHub.

At an even higher level though, being able to have ownership of your own things is good these days. It's [devastatingly simple to spin up a Gitea](https://docs.gitea.com/installation/install-with-docker), committing me to not much more time than I normally would put into maintaining my GitHub while gaining a bit more platform independence. Lately I've been a huge advocate for owning your own tools; I think this mirrors my preference to roll your own code where practical. Increasingly, everything about our lives is becoming a subscription service requiring us to pay for the privilege of owning less - how exponentially impoverished we're becoming! Ownership isn't now about wealth; it's a matter of freedom.

So I'll be spinning up my own Gitea server, migrating off GitHub, and gaining a huge benefit - by my eyes. There remain issues:

1. If I delete my GitHub, how do I find others' repos? How do they find mine?
2. Gitea doesn't have an equivalent to Pages, but obviously this can be done through Railway. Need to figure a good architectural approach.
3. Railway, funny enough, is built entirely around "give me a GitHub repo and we deploy that with no effort." There will be more effort now.

These problems will be ironed out, and I'll document on my blog how I get through these. The first point is a sticky one though: the industry is so used to being on GitHub. That's not as hard an expectation now as it was five years ago, but lacking any separate protocol for repo discovery this may be interesting. For public projects I might set up some kind of GitHub replication. Goodness knows!
