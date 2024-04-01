;;;
{
	"title": "I've IndieWebbed My Site",
	"description": "A small, loose collection of formats and protocols, IndieWeb is an interesting supplement (maybe alternative) to social media",
	"date": "1 April 2024",
	"contents": false,
	"hero": "photo-1589933767411-38a58367efd7",
    "related": [
		{ "title": "Giscus Is Awesome", "description": "I can add comments to my statically generated blog? Using GitHub Discussions?? For Free??? And it works????", "fileName": "giscus_is_awesome" },
		{ "title": "Daily Grug", "description": "Need inspiration start day, made API.", "fileName": "daily_grug" },
		{ "title": "90% of my Homepage was Useless", "description": "In a few days, I reduced the size of my homepage to 10% of what it had been, and sped it up by 50-66%.", "fileName": "90_of_my_homepage_was_useless" }
    ]
}
;;;

The [IndieWeb](https://indieweb.org/) is a small and loose collection of formats and protocols which allow those of us with our own sites to identify each other and their content across the web. Even that description is a bit much - there's only the [webmention](https://webmention.net/) protocol and a couple so-called [microformats](https://indieweb.org/microformats) for identifying people and content.

Obviously I own my own site, and I had to do _very_ little work to ensure that the markup of this site conformed to the flexible specifications of the microformats. With this all in place, I can [authenticate](https://indieweb.org/IndieAuth) myself using my site, others can [identify me by my domain](https://indieweb.org/h-card), y'all can [scrape my site for posts](https://indieweb.org/h-entry), and more. This gives a common foundation for individuals to be able to control their own content and exposure to the wider web. By using the `h-card` microformat to _link_ to each other across blogs, it also decentralizes the relationships between this content, obivating the utility of scoail media platforms as providers of these relationships.

The glue that holds it together is the webmention protocol. This is a simple way for someone who publishes content to notify another when an author mentions someone on their site, the idea being that each person maitains a listener at a certain endpoint (identified by a `<link rel="webmention">` header tag) that will listen for and save pings from others, and then display them on their site. My webmention URL is `https://webmention.io/ian.wold.guru/webmention`, and if you webmention one of my posts it wil lshow up at the bottom of the post! The other half is publishing of course - I'm working on a clean way for this site's [build pipeline](https://github.com/IanWold/ianwold.github.io/blob/master/.github/workflows/build.yml) to send these pings. The tricky part is a ping should only be sent once, when the mention is first published. I used the excellent site [webmention.io](https://webmention.io) In order to set up receiving and querying webmentions. A [simple script](https://github.com/IanWold/ianwold.github.io/commit/8e16adb457ee367c159635f66710c82363e8b4a9) queries webmentions for each article and displays them.

The [IndieWeb wiki](https://indieweb.org/) was an excellent resource that got me quite excited about the potential for this idea. The intent seems to be that this is a supplement to social media, but I see potential in it as an alternative to social media. I'm not a fan of any of the social platforms nowadays, and attempts at federated platforms like Mastodon don't seem to provide all that much for me. The notion that I might be able to replace the conversation aspect of social media with something as simple as the IndieWeb is attractive.

I'd encourage everyone with their own site to implement at least the IndieWeb formats. [IndieWebify.me](https://indiewebify.me/) is great for testing your site, and there's a lot more resources in the wiki. If you're not so keen on coding your own site and want to use a hosting provider instead, it happens that WordPress will have no problem IndieWebifying you, and there's [several other hosts](https://indieweb.org/web_hosting) you can consider as well. Finally, there are [chats set up in Slack, IRC, and Discord](https://indieweb.org/discuss) to help get going.

Is there some IndieWeb component which I'm missing on this site but I should include? Webmention me and I'll set it up; this is an ongoing project for me!