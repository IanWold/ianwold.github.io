#! "net6.0.4"
#r "nuget: Metalsharp, 0.9.0-rc.4"
#r "nuget: Metalsharp.LiquidTemplates, 0.9.0-rc.3"
#r "nuget: Metalsharp.SimpleBlog, 0.9.0-rc.1"

using Metalsharp;
using Metalsharp.LiquidTemplates;
using Metalsharp.SimpleBlog;

new MetalsharpProject(new MetalsharpConfiguration { Verbosity = Metalsharp.Logging.LogLevel.Info })
.AddInput("Site", ".\\")
.UseFrontmatter()
.UseMarkdown()
.UseSimpleBlog(new()
{
	PostsDirectory = ".\\Posts",
	PostsOrderQuery = f => DateTime.Parse(f.Metadata["date"] as string ?? DateTime.Now.ToString()),
	PostMetadata = new()
	{
		["template"] = "article",
		["highlight"] = "blog"
	},
	BlogFilePath = ".\\blog.html",
	BlogMetadata = new()
	{
		["template"] = "blog",
		["highlight"] = "blog",
		["title"] = "Blog"
	},
})
.UseLeveller()
.UseLiquidTemplates("Templates")
.AddOutput("Static", ".\\")
.Build(new BuildOptions
{
	OutputDirectory = "output"
});
