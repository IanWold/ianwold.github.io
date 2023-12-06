;;;
{
	"title": "90% of my Homepage was Useless",
	"description": "In a few days, I reduced the size of my homepage to 10% of what it had been, and sped it up by 50-66%.",
	"date": "6 December 2023",
	"contents": false,
	"hero": "photo-1686061593213-98dad7c599b9",
    "related": [
		{ "title": "Giscus Is Awesome", "description": "I can add comments to my statically generated blog? Using GitHub Discussions?? For Free??? And it works????", "fileName": "giscus_is_awesome" },
		{ "title": "Daily Grug", "description": "Need inspiration start day, made API.", "fileName": "daily_grug" },
		{ "title": "Book Club 11/2023: New .NET, New C#", "description": "The release of .NET 8 brings a lot of features I'm excited for!", "fileName": "book_club_11-2023" },
    ]
}
;;;

One week ago, my homepage was around 550k uncompressed and took about as many ms to load. I thought this was light, though I'd have liked it to be faster - I just attributed this to a side-effect of hosting with GitHub pages.

This weekend I discovered the [512kb Club](https://512kb.club) which ranks websites by how big they are, the catch being that you need to be below 512k uncompressed. I looked at some of the sites on the high end of that list, and I felt pretty disappointed. There were sites that had much more content, some with many large images, while staying below 512k. I have an SVG and a bunch of black-and-white text.

Well, I made some small optimizations and got it below the threshold, but then I caught the bug and _presto changeo_ before I knew it I had eliminated 90% of my website _and it is pixel-perfect to how it was before_. I'm now at 62.8k, a member of the "green team" on the 512kb Club site, and equally important I reduced the load time of the site (by any time metric you like) by around 50% just with the size optimizations.

# Check Your Fonts

The single most impactful thing I did was I checked my fonts. I use Google WebFonts, and their API gives you a few opportunities for improvement. First, make sure you're not loading any fonts you're not using.

Then eliminate any weights you're not using. I use `Rubik` for my text, and the homepage only uses weights 300 and 500, but I had been loading everything from 100 to 900:

```html
<link href="https://fonts.googleapis.com/css2?family=Rubik:wght@100;200;300;400;500;600;700;800;900&display=swap" rel="stylesheet">
<link href="https://fonts.googleapis.com/css2?family=Rubik:wght@300;500&display=swap" rel="stylesheet">
```

I use `Montserrat` for my name at the top and nothing else, and Google provides an API for you to specify the exact text you need to load in a font. This took `Montserrat` from 30k to 2k or so:

```html
<link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@100;200;300;400;500;600;700;800;900&display=swap" rel="stylesheet">
<link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@700&display=swap&text=IAN%20WOLD" rel="stylesheet">
```

# Optimize Your SVGs

[SVGOMG](https://svgomg.net/) is a great tool for this. The SVG at the top of my page was 64k, now it's about 8k. _That's huge!_

I had also previously been using [Feather Icon](https://feathericons.com/)'s JS client to swap out `i` tags for their icon SVGs. However, by directly embedding the SVGs (which is easy and maintainable because they've got a great site) I was able to eliminate 30k and several ms from my page load time.

# Torchlight is Pretty Cool

I had been using [Highlight.js](https://highlightjs.org/) for syntax highlighting, and it was easy enough to set up (I _highly_ recommend it if you need to get up and going fast) but I wasn't too much a fan of the highlighting for my cases, _and_ it was putting a 30k dependency on my site. Yes, I was able to take it off my homepage - no code there - but I figured I could also help my post pages out.

[Torchlight](https://torchlight.dev) is absolutely awesome. They run a server with the same language servers that VSCode uses with an API that will perform syntax highlighting for your snippets. Best part is they've got a Node package that you can run on a local directory to detect any `<pre><code></code></pre>` blocks in your HTML, post the code to their server, and swap out your code blocks with their highlighted blocks. It's free for non-revenue-generating sites, works like charm in the build step, and generates the best syntax highlighting with the most features.

# Maybe Minify Your Pages

I only saved a few kb doing this. What's most important I think is to consider bundling assets that can be bundled to save on the chore of loading them from a server. If your files are sufficiently small (as they tend to be on a statically-generated site) you're not going to save too much by minifying. However, if it's easy to add, then go for it.

# Next Optimizations

* SEO
* Accessibility
* Performance
* The SVG at the top of the page still doesn't turn white in dark mode
