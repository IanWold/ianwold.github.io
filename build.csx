﻿#! "net8.0"
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
			["slug"] = post.Name
		};
	},
	BlogFilePath = @".\index.html",
	BlogMetadata = new()
	{
		["title"] = "",
		["template"] = "archive",
		["removeScrollspy"] = true,
		["fontRequirement"] = "index"
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
})
.Use(project => // Add table of contents to posts
{
	var posts = project.OutputFiles.Where(f => f.Directory.StartsWith(@".\Posts"));// && f.Metadata.TryGetValue("contents", out object isContentsObject) && isContentsObject is bool isContents && isContents);

	foreach (var post in posts)
	{
		var postLines = post.Text.Split('\r', '\n').Where(l => !string.IsNullOrWhiteSpace(l));
		var postBuilder = new StringBuilder();
		var sections = new List<object>();
		var isInContainingSection = false;
		var isInInnerSection = false;

		void addSection(Match headerMatch, int level)
		{
			var sectionName = headerMatch.Groups[1].Captures[0].Value;
			var sectionId = string.Join('-', sectionName.Split(' ')).ToLower();

			sections.Add(new { name = sectionName, id = sectionId, level = level });
			postBuilder.AppendLine($"<section id=\"{sectionId}\">");
		}

		foreach (var line in postLines)
		{
			if (Regex.Match(line.Trim(), @"(?:<h1>)([^<]*)(?:<\/h1>)$") is Match matchContainingHeader && matchContainingHeader.Success)
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

				addSection(matchContainingHeader, 1);

				isInContainingSection = true;
			}
			else if (Regex.Match(line.Trim(), @"(?:<h2>)([^<]*)(?:<\/h2>)$") is Match matchInnerHeader && matchInnerHeader.Success)
			{
				if (isInInnerSection)
				{
					postBuilder.AppendLine("</section>");
				}

				addSection(matchInnerHeader, 2);

				isInInnerSection = true;
			}

			postBuilder.AppendLine(line);
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
.Build(new BuildOptions()
{
	OutputDirectory = "output",
	ClearOutputDirectory = true
});

record SeriesInfo(string Title, string Slug, string Description);