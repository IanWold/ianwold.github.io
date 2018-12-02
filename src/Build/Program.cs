using Metalsharp;
using System.Linq;
using Metalsharp.FluidTemplate;
using System.IO;
using System.Collections.Generic;
using System;

namespace Build
{
    class Program
    {
        static void Main(string[] args) =>
            new MetalsharpDirectory("..\\..\\..\\..\\Site", ".\\")
                .UseFrontmatter()
                .UseMarkdown()
                .Use(dir =>
                {
                    dir.Meta("posts", new List<Post>());

                    foreach (var file in dir.OutputFiles
                        .Where(file => file.Directory == ".\\Posts")
                        .OrderByDescending(file => DateTime.TryParse(file.Metadata["date"] as string, out var date) ? ((date.Year * 10000) + (date.Month * 100) + date.Day) : 0))
                    {
                        file.Metadata.Add("template", ".\\Templates\\article.liquid");
                        file.Metadata.Add("highlight", "blog");

                        (dir.Metadata["posts"] as List<Post>).Add(new Post(file.Metadata["title"] as string, file.Metadata["date"] as string, file.Metadata["description"] as string, file.Metadata["author"] as string, file.Name));
                    }
                })
                .Use(dir => dir.AddOutput(new MetalsharpFile("", ".\\blog.html")
                {
                    Metadata = new Dictionary<string, object>()
                    {
                        ["posts"] = dir.Metadata["posts"],
                        ["highlight"] = "blog",
                        ["title"] = "Blog",
                        ["template"] = ".\\Templates\\blog.liquid"
                    }
                }))
                .Use(LeveledFiles)
                .AddInput("..\\..\\..\\..\\Templates", ".\\Templates")
                .UseFluidTemplate(".\\Templates\\layout.liquid")
                .AddOutput("..\\..\\..\\..\\Static", ".\\")
                .Build(new BuildOptions
                {
                    OutputDirectory = "built",
                    ClearOutputDirectory = false
                });

        static void LeveledFiles(MetalsharpDirectory directory)
        {
            foreach (var file in directory.InputFiles.Concat(directory.OutputFiles))
            {
                var dirLevels = file.Directory.Split(Path.DirectorySeparatorChar);
                var dirLevelCount = dirLevels.Count() - (dirLevels[0] == "." ? 1 : 0);

                if (file.Metadata.ContainsKey("level"))
                {
                    file.Metadata["level"] = dirLevelCount;
                }
                else
                {
                    file.Metadata.Add("level", dirLevelCount);
                }
            }
        }
    }

    class Post
    {
        public Post(string title, string date, string description, string author, string slug)
        {
            Title = title;
            Date = date;
            Description = description;
            Author = author;
            Slug = slug;
        }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Slug { get; set; }
    }
}
