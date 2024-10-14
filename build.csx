#! "net8.0"
#r "nuget: Metalsharp, 0.9.0-rc.5"
#r "nuget: Metalsharp.LiquidTemplates, 0.9.0-rc-3"
#r "nuget: Metalsharp.SimpleBlog, 0.9.0-rc.2"
#r "nuget: System.ServiceModel.Syndication, 8.0.0"

using Metalsharp;
using Metalsharp.LiquidTemplates;
using Metalsharp.SimpleBlog;
using System.Text;
using System.Text.RegularExpressions;
using System.ServiceModel.Syndication;
using System.IO;
using System.Xml;
using System;
using System.Text.Json;

IEnumerable<SeriesInfo> seriesInfo = [];

using (var reader = new StreamReader("Config/series.json"))
{
	seriesInfo = JsonSerializer.Deserialize<IEnumerable<SeriesInfo>>(reader.ReadToEnd());
}

new MetalsharpProject()
.AddInput("Site", @".\")
.UseFrontmatter()
.RemoveFiles(file =>
	file.Metadata.TryGetValue("draft", out var isDraftObj)
	&& isDraftObj is bool isDraft
	&& isDraft
)
.Use(project => // Add reading time to posts
{
	foreach (var file in project.InputFiles.Where(f => f.Directory.StartsWith(@".\Posts")))
	{
		file.Metadata.Add("readingTime", (int)Math.Ceiling(file.Text.Split(' ').Length / 200D));
	}
})
.UseMarkdown()
.UseSimpleBlog(new()
{
	PostsDirectory = @".\Posts",
	PostsOrderQuery = f => DateTime.Parse(f.Metadata["date"] as string ?? DateTime.Now.ToString()),
	PostMetadata = post =>
	{
		var postDate = DateTime.Parse(post.Metadata["date"]?.ToString() ?? "");
		return new()
		{
			["template"] = "article",
			["year"] = postDate.Year.ToString(),
			["month"] = postDate.Month.ToString("D2"),
			["day"] = postDate.Day.ToString("D2"),
			["isodate"] = postDate.ToUniversalTime().ToString("o", System.Globalization.CultureInfo.InvariantCulture),
			["slug"] = post.Name
		};
	},
	BlogFilePath = @".\index.html",
	BlogMetadata = new()
	{
		["title"] = "Software Engineer, Architect, and Team Leader",
		["template"] = "archive",
		["removeScrollspy"] = true,
		["hidePastArticles"] = true,
		["fontRequirement"] = "index",
		["includeCanonicalIndex"] = true
	},
})
.Use(project => // Generate RSS feed
{
	var rssItems = project.OutputFiles.Where(f => f.Directory.StartsWith(@".\Posts")).Select(post => new SyndicationItem(
		post.Metadata["title"].ToString(),
		post.Text,
		new Uri($"https://ian.wold.guru/Posts/{post.Name}.html"),
		post.Name,
		DateTime.Parse(post.Metadata["date"]?.ToString() ?? "")
	));

	var rssFeedContent = string.Empty;
	var rssFeed = new SyndicationFeed(
		"Ian Wold",
		"Ian Wold's Blog",
		new Uri("https://ian.wold.guru/feed.xml"),
		rssItems.Select(i =>
		{
			i.PublishDate = i.LastUpdatedTime;
			return i;
		})
	);

	var xmlSettings = new XmlWriterSettings
    {
        Indent = true,
        OmitXmlDeclaration = true,
        Encoding = Encoding.UTF8
    };
	using (var memoryStream = new MemoryStream())
    using (var xmlWriter = XmlWriter.Create(memoryStream, xmlSettings))
    {
        rssFeed.SaveAsRss20(xmlWriter);
        xmlWriter.Flush();
        memoryStream.Position = 0;

        using (var reader = new StreamReader(memoryStream))
        {
            rssFeedContent = reader.ReadToEnd();
        }
    }

	project.AddOutput(new MetalsharpFile(rssFeedContent, "feed.xml"));
})
.Use(project => // Add SEO and "series" metadata to posts
{
	var seriesPosts = new Dictionary<string, IEnumerable<Dictionary<string, object>>>();
	var topicPosts = new Dictionary<string, IEnumerable<Dictionary<string, object>>>();

	var posts = project.OutputFiles.Where(f => f.Directory.StartsWith(@".\Posts"));
	foreach (var post in posts)
	{
		post.Metadata.Add("structuredData", $$"""
			{
				"@context": "https://schema.org",
				"@type": "Article",
				"author": [{
					"@type": "Person",
					"name": "Ian Wold"
				}],
				"datePublished": "{{DateTime.Parse(post.Metadata["date"]?.ToString() ?? "")}}",
				"image": "https://images.unsplash.com/{{ post.Metadata["hero"]!.ToString()}}",
				"headline": "{{post.Metadata["title"]!.ToString()}}",
				"description": "{{post.Metadata["description"]!.ToString()}}",
				"publisher": {
					"@type": "Person",
					"name": "Ian Wold",
					"logo": {
						"@type": "ImageObject",
						"url": "https://ian.wold.guru/images/hero1.svg"
					}
				},
			}
			"""
		);

		if (post.Metadata.TryGetValue("series", out var seriesObject) && seriesObject is string seriesName)
		{
			if (seriesPosts.TryGetValue(seriesName, out var seriesPostsList))
			{
				seriesPosts[seriesName] = [ ..seriesPostsList, post.Metadata ];
			}
			else
			{
				seriesPosts.Add(seriesName, [ post.Metadata ]);
			}
		}

		if (post.Metadata.TryGetValue("topics", out var topicsObject) && topicsObject is JsonElement topicNamesElement)
		{
			foreach (var topicNameElement in topicNamesElement.EnumerateArray())
			{
				var topicName = topicNameElement.GetString();
				if (topicPosts.TryGetValue(topicName, out var topicPostsList))
				{
					seriesPosts[topicName] = [ ..topicPostsList, post.Metadata ];
				}
				else
				{
					topicPosts.Add(topicName, [ post.Metadata ]);
				}
			}
		}
	}

	foreach (var series in seriesPosts)
	{
		project.AddOutput(new MetalsharpFile(string.Empty, $".\\Series\\{seriesInfo.Single(s => s.Title == series.Key).Slug}.html", new Dictionary<string, object>()
		{
			["title"] = series.Key,
			["series"] = series.Key,
			["template"] = "archive",
			["removeScrollspy"] = true,
			["posts"] = series.Value.OrderByDescending(p => DateTime.Parse(p["date"].ToString()))
		}));
	}

	foreach (var topic in topicPosts)
	{
		project.AddOutput(new MetalsharpFile(string.Empty, $".\\Topics\\{topic.Key.ToLowerInvariant()}.html", new Dictionary<string, object>()
		{
			["title"] = topic.Key,
			["topic"] = topic.Key,
			["template"] = "archive",
			["removeScrollspy"] = true,
			["posts"] = topic.Value.OrderByDescending(p => DateTime.Parse(p["date"].ToString()))
		}));
	}
})
.Use(project => // Add table of contents to posts
{
	var posts = project.OutputFiles.Where(f => f.Directory.StartsWith(@".\Posts"));// && f.Metadata.TryGetValue("contents", out object isContentsObject) && isContentsObject is bool isContents && isContents);

	static string getSectionSlug(string sectionName) =>
		string.Join('-', sectionName.Split(' ')).ToLower();

	foreach (var post in posts)
	{
		var postLines = post.Text.Split('\r', '\n').Where(l => !string.IsNullOrWhiteSpace(l));
		var postBuilder = new StringBuilder();
		var sections = new List<object>();
		var isInContainingSection = false;
		var isInInnerSection = false;

		void addSection(string sectionName, string sectionSlug, int level)
		{
			sections.Add(new { name = sectionName, id = sectionSlug, level = level });
			postBuilder.AppendLine($"<section id=\"{sectionSlug}\">");
		}

		string getHeaderWithSectionLink(string sectionSlug, string line) =>
			$"{line.Substring(0, line.Length - 5)} <a class=\"section-link\" href=\"https://ian.wold.guru/Posts/{post.Name}.html#{sectionSlug}\">#</a>{line.Substring(line.Length - 5, 5)}";

		foreach (var line in postLines)
		{
			var lineToAdd = line;
			var trimmedLine = line.Trim();

			if (Regex.Match(trimmedLine, @"(?:<h1>)([^<]*)(?:<\/h1>)$") is Match matchContainingHeader && matchContainingHeader.Success)
			{
				if (isInInnerSection)
				{
					postBuilder.AppendLine("</section>");
					isInInnerSection = false;
				}

				if (isInContainingSection)
				{
					postBuilder.AppendLine("</section>");
				}

				var sectionName = matchContainingHeader.Groups[1].Captures[0].Value;
				var sectionSlug = getSectionSlug(sectionName);

				addSection(sectionName, sectionSlug, 1);
				lineToAdd = getHeaderWithSectionLink(sectionSlug, trimmedLine);

				isInContainingSection = true;
			}
			else if (Regex.Match(trimmedLine, @"(?:<h2>)([^<]*)(?:<\/h2>)$") is Match matchInnerHeader && matchInnerHeader.Success)
			{
				if (isInInnerSection)
				{
					postBuilder.AppendLine("</section>");
				}

				var sectionName = matchInnerHeader.Groups[1].Captures[0].Value;
				var sectionSlug = getSectionSlug(sectionName);

				addSection(sectionName, sectionSlug, 2);
				lineToAdd = getHeaderWithSectionLink(sectionSlug, trimmedLine);

				isInInnerSection = true;
			}

			postBuilder.AppendLine(lineToAdd);
		}

		if (isInInnerSection)
		{
			postBuilder.AppendLine("</section>");
			isInInnerSection = false;
		}

		if (isInContainingSection)
		{
			postBuilder.AppendLine("</section>");
			isInContainingSection = false;
		}

		post.Text = postBuilder.ToString();
		post.Metadata.Add("sections", sections);
	}
})
.Use(project => // Add slug and description for "series" files
{
	foreach (var file in project.InputFiles.Concat(project.OutputFiles))
	{
		if (file.Metadata.TryGetValue("series", out var seriesNameObj) && seriesNameObj is string seriesName)
		{
			if (!file.Metadata.ContainsKey("seriesDescription"))
			{
				file.Metadata.Add("seriesDescription", seriesInfo.Single(s => s.Title == seriesName).Description.Split("\n").Select(d => $"<p>{d}</p>"));
			}
			
			if (!file.Metadata.ContainsKey("seriesSlug"))
			{
				file.Metadata.Add("seriesSlug", seriesInfo.Single(s => s.Title == seriesName).Slug);
			}
		}
	}
})
.UseLeveller()
.UseLiquidTemplates("Templates")
.AddOutput("Static", @".\")
.Use(project => // Add sitemap
{
	var builder = new StringBuilder();
	builder.AppendLine("https://ian.wold.guru/");

	foreach (var page in project.OutputFiles.Where(f => f.Extension.Contains("html") && !f.Name.Contains("index")))
	{
		builder.AppendLine($"https://ian.wold.guru/{page.FilePath.Replace(".\\", "").Replace("\\", "/")}");
	}

	project.AddOutput(new MetalsharpFile(builder.ToString(), "sitemap.txt"));
})
.Build(new BuildOptions()
{
	OutputDirectory = "output",
	ClearOutputDirectory = true
});

record SeriesInfo(string Title, string Slug, string Description);