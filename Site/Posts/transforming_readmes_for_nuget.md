;;;
{
	"title": "Transforming READMEs for Nuget",
	"description": "I like making my project's README look pretty, but Nuget doesn't support all the same rendering options that git providers usuall have; it is necessary to set up some intermediary transform.",
	"date": "25 April 2026",
	"contents": false,
	"hero": "photo-1702467439812-eef67d80539d",
    "topics": ["How-To", "Deployment", "Nuget"],
	"related": [
        { "title": "Successful Personal Projects", "description": "The process and thinking I follow to follow through on personal projects and get what I want out of them.", "fileName": "successful_personal_projects" },
        { "title": "Deploying Your Prolog API with Docker", "description": "It can be tough living on the bleeding edge of modern technology. If you've jumped on the hype train and developed your latest API with Prolog only to find there aren't any tutorials to Dockerize it - look no further!", "fileName": "deploying_your_prolog_api_with_docker" }
	]
}
;;;


I'm a fan of pretty readme files. In fact, I like having centered content on the top. I'm particularly happy with my readmes for [PlanningPoker](https://github.com/IanWold/PlanningPoker), [PostgreSignalR](https://github.com/IanWold/PostgreSignalR), and my very recent [Dovetail](https://github.com/IanWold/Dovetail).

It's on that last project in fact - Dovetail - that I spent quite a bit of time on the readme. It's a small library that doesn't need lots of documentation files, so I figured it would be a fun exercise to make a nice, pretty readme that covered all the topics it would need to.

This turned out to be an excellent exercise, and one which I might recommend. GitHub has a lot of features you can use to render different kinds of _things_ on your readme, which in turn you can use to keep content readable and well-organized. The readme I've ended up with is a little on the longer side, but really I'm quite proud of it.

There is a bit of a downside though: it seems like every fun feature that GitHub has put into their Markdown rendering is absent from how Nuget renders its readme files. The biggest issue is that Nuget doesn't allow `<div align="center">`, which causes a bunch of ugly HTML to show up right at the top of the readme. Another annoyance is that [callout](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax#alerts) headers will render as plain text.

The solution, of course, is to have some kind of scrubbing mechanism run as part of the packaging step to remove unwanted artifacts from the README that gets sent to Nuget. But, how?

Well, [having switched to Linux recently](https://ian.wold.guru/Posts/ive_switched_to_linux.html), I now have easier access to a lot of tools I haven't used much before, and I've been enjoying learning them. One of those is [`awk`](https://en.wikipedia.org/wiki/AWK) - seems like I should have had a grasp of that before but c'est la vie. Anywho, that's the ideal tool for this surely (adding the obvious requirement that I'll be running my CI on Linux). But again, how?

Well, first I need an `awk` script. Really, this is simple enough: my centered content always appears before the first line break (`---`), so I just need to exclude everything above that:

```awk
/^---$/{f=1; next} f
```

And then callout headers match a pretty simple regex:

```regex
> \[![A-Z]*\]
```

Smashing those two together yields the desired output, now this needs to run when msbuild packages all the files together. This is the kind of thing that [msbuild targets](https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-targets?view=visualstudio) are for. In this case, the target we'd want is `_GetPackageFiles`. I'll drop the whole `<Target>` here:

```xml
<Target Name="GenerateNuGetReadme" BeforeTargets="_GetPackageFiles">
    <PropertyGroup>
        <_NuGetReadmePath>$(IntermediateOutputPath)README.md</_NuGetReadmePath>
    </PropertyGroup>
    <Exec Command="awk '/^---$/{f=1; next} f &amp;&amp; !/&gt; \[![A-Z]*\]/' &quot;$(MSBuildProjectDirectory)/../README.md&quot; &gt; &quot;$(_NuGetReadmePath)&quot;" />
    <ItemGroup>
        <None Include="$(_NuGetReadmePath)" Pack="true" PackagePath="README.md" Visible="false" />
    </ItemGroup>
</Target>
```

The exact location of your project's readme relative to the csproj will probably vary, but you can see the awk command reads out fine after breaking down the pieces (unlike most awk scripts). If you've got other bits of your readme you like to filter out or modify before it goes to Nuget, you'd just need to update that awk script. The whole csproj, for reference, [is on GitHub](https://github.com/IanWold/Dovetail/blob/main/Dovetail/Dovetail.csproj)

Now, my readme looks very nice indeed [on Nuget](https://www.nuget.org/packages/Dovetail)!