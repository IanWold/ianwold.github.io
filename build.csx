﻿#! "net6.0.4"
#r "nuget: Metalsharp, 0.9.0-rc.5"
#r "nuget: Metalsharp.LiquidTemplates, 0.9.0-rc-3"
#r "nuget: Metalsharp.SimpleBlog, 0.9.0-rc.2"

using Metalsharp;
using Metalsharp.LiquidTemplates;
using Metalsharp.SimpleBlog;
using System.Text;
using System.Text.RegularExpressions;

new MetalsharpProject()
.AddInput("Site", @".\")
.UseFrontmatter()
.Use(project =>
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
			["day"] = postDate.Day.ToString("D2")
		};
	},
	BlogFilePath = @".\index.html",
	BlogMetadata = new()
	{
		["title"] = "",
		["template"] = "archive"
	},
})
.UseLeveller()
.Use(project =>
{
	foreach (var post in project.OutputFiles.Where(f => f.Directory.StartsWith(@".\Posts") && f.Metadata.TryGetValue("contents", out object isContentsObject) && isContentsObject is bool isContents && isContents))
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
.UseLiquidTemplates("Templates")
.AddOutput("Static", @".\")
.Build(new BuildOptions()
{
	OutputDirectory = "output",
	ClearOutputDirectory = true
});