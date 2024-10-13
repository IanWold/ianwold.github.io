;;;
{
	"title": "I've Stopped Using Visual Studio",
	"description": "... mostly. And so can you!",
	"date": "30 September 2024",
	"contents": false,
	"hero": "photo-1649180564403-db28d5673f48",
    "topics": ["Tooling", "DevEx", "Dotnet"],
    "related": [
		{ "title": "My (Continuing) Descent Into Madness", "description": "It started simply enough, when I asked myself if I should try an IDE other than Visual Studio. Mere months later, I'm now using a tiling window manager. This is the story of my (continuing) descent into madness.", "fileName": "my_continuing_descent_into_madness" },
		{ "title": "90% of my Homepage was Useless", "description": "In a few days, I reduced the size of my homepage to 10% of what it had been, and sped it up by 50-66%.", "fileName": "90_of_my_homepage_was_useless" },
        { "title": "\"Should I Learn (Insert Some Tech Here)?\"", "description": "One of the most common questions - would it be good to learn this or that language, framework, database, etc? Taking even a little time to learn something new is good all around, but is it really worth making an investment in yourself to grow personally and professionally? Let's take a deep dive into this topic.", "fileName": "should_i_learn_insert_some_tech_here" }
    ]
}
;;;

One year ago, Microsoft released the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) extension for VS Code, promising to bring the solution explorer to the editor along with quality of life updates around building, packages, testing, and other niceties. I was doing a fair amount of JavaScript at work at the time this was released, so I thought I'd install it in VS Code and use that editor for all my work instead of switching between editors several times a day.

Over the last year, I've opened Visual Studio less and less. There are some legacy .NET Framework projects I need to use it for, otherwise all of my work is done in VS Code. This is due to a few factors: I can actually develop in several langauges, I can customize it better, its extensions allow me to use it for more workflows than just development, and I find I'm actually more productive in the simpler environment. To be clear, I'm not and never have been a Visual Studio hater - I don't like Rider or ReSharper, I've always been very happy with Visual Studio. I just think the simpler editor leads to a better workflow.

# Customizations

To start with, I mapped all of the panels on the activity bar to key commands. `CTRL+SHIFT+_` gets me around all the panels, and I only mapped keys on the left of the keyboard for quick, one-hand navigation. `x` opens the _e_xplorer, `c` opens version _c_ontrol, `space` opens or hides the bottom panel, `a` closes the side panel entirely, and so on. I can keep the activity bar closed and get around everywhere I need quickly.

Then I made some UI updates so that the tab bars at the top of the editor are much narrower, and I cleared up the clutter on the status bar. I installed [Studio Icons](https://marketplace.visualstudio.com/items?itemName=jtlowe.vscode-icon-theme) because after the last 14+ years of using Visual Studio I'm still used to its icons. Finally, I imported by [Monokai Gray](https://packagecontrol.io/packages/Monokai%20Gray) theme, which I originally made for Sublime but now use on all my editors.

# Background

The best idea I had though was to install the [Background](https://marketplace.visualstudio.com/items?itemName=Katsute.code-background) extension. This extension allows you to specify background images for your code panels. I know it sounds like a distraction or a pain when you first consider the idea, but in practice it's anything but.

I installed it and loaded it with several different-colored space wallpaper images (one is red, one is blue, one is green, and so on). This has made a huge improvement in my ability to keep groups of tabs distinct when working. I use an ultrawide monitor most of the time, so it's not rare that I have six tab groups all open side-by-side. Now they're all (gently) colored! Not to mention, i makes me look like one of the cool kids 😎

# Other Extensions

The [WSL](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-wsl) extension has made working in the WSL so much simpler. It wasn't necessarily difficult before, but if you d oany work in the WSL this one will sell VS Code to you (if you're not using it already - it's been well-known for some time that VS Code works well with it).

The [Postman](https://marketplace.visualstudio.com/items?itemName=Postman.postman-for-vscode) extension is so good that I barely need to open the Postman application anymore! It's really excellent to be able to keep Postman open in the same context I'm using to edit code. I don't use Postman to execute local requests for debugging, but when you need to test calls to services you're connecting to in your code it keeps the workflow entirely inside VS Code.

[GitHub Pull Requests](https://marketplace.visualstudio.com/items?itemName=GitHub.vscode-pull-request-github) and other extensions from GitHub have allowed me to do the majority of my GitHub work right from th editor. Anyone who's reviewed PRs from their editor will (I'm assuming) be quick to describe how much better the experience is to see code changes in the context of the whole codebase instead of through the small window on the online interface.

Finally, the [Excalidraw](https://marketplace.visualstudio.com/items?itemName=pomdtr.excalidraw-editor) extension is really excellent. Im' a big fan of Excalidraw not just as a product, but I'm impressed by the development they've put into the product. With this exension I keep all my diagrams as local `.excalidraw` files, and I edit them all from the editor.

The best part about the last three extensions is they all use my space backgrounds too!

# Workflow

Using VS Code for the last year has had an improvement, I think, on my workflow. For the most part, the editor stays out of the way. At first configuring building and testing took a bit of doing, but not more than a couple days' worth of reading docs online. I'm friends with the editor now, and its simplicity (or maybe its _ability_ to be simple) is its strongest suit.

That does mean, of course, that there are a lot of features it doesn't support through the UI. Of course, anything can be done through the terminal, and the editor makes that very easy as well. I have `CTRL+SHIFT+SPACE` mapped to open the panel, and it opens right to the terminal now. I make a lot of use of that anymore; I barely did when using .NET for C# projects. For complicated operations - particularly some of the more finicky Git things - the terminal is a much better tool anyway.

Almost every small problem I have with the editor either has a workaround or different flow to avoid it, or gets fixed in the not-too-distant future. The one major gipe I have left though is that it _is_ an Electron app. Microsoft has gone a fair way to address some of the performance issues resulting from that, but they are still there. F12 (go to definition) takes noticeably longer in VS Code than Visual Studio. Even button clicks seem to take an extra split second to register. Even opening a file is faster in the other.

At the end of the day though, I'm incredibly happy with the editor and the workflow I've found with it here. I'm going to be keeping it going forward, and I'd recommend anyone on Visual Studio give it a try.
