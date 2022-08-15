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
