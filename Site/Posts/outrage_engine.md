;;;
{
	"title": "The Outrage Engine",
	"author": "Ian Wold",
	"date": "25 October 2016",
	"description": "Perhaps an ASCII game in the Windows Console is ridiculous. Something akin to Dwarf Fortress comes to mind, so it's not entirely off the mark. But a game engine devoted to ASCII games in the console? Perhaps that's outrageous. I don't know if it's been done (or is being done) currently, but that's what I'm doing right now, and I've called it the Outrage Engine."
}
;;;

Perhaps an ASCII game in the Windows Console is ridiculous. Something akin to Dwarf Fortress comes to mind, so it's not entirely off the mark. But a game engine devoted to ASCII games in the console? Perhaps that's outrageous. I don't know if it's been done (or is being done) currently, but that's what I'm doing right now, and I've called it the [Outrage Engine](https://github.com/IanWold/OutrageEngine). I'll give a little overview here of where my early engine is at after I've been working on it piecemeal between classes and summer jobs, and where I hope to go in future. It started as more of a *proof of concept* than an attempt to create a solid product, and I imagine that's where it will remain, but it's all good fun.

## The Current State

Overall, the engine is event-driven and not time-driven, so  the game will only ever update when the user provides input to the console. I attempted implementing a time-driven system (updating 24 times a second), but I was unable to capture input using the traditional *Console.ReadKey()*. So, for the time being (in the true POC method) we're stuck with event-driven games. In the engine's base directory lives the class *GameLoop*. This provides a game with the central loop that drives everything. You can see it keeps tract of the current *view* (a *scene* or a *menu*) that's on display, and updates after every key input. It also contains methods to navigate between views and scenes.

Going into the *Engine* directory, we see we have *camera*, *entity*, *scene*, and *terrain*. These components are rather simple concepts. A scene contains the other three, and the game can switch from scene to scene as needed. An entity is a "thing," so to speak - any object in the scene that can interact or be interacted with (the player, walls, flowers, etc.). The terrain is the thing which manages the locations of all the entities and detects collisions and whatnot. Finally, the camera provides the information for what to render - this way the view can be controlled independently of the state of the player or terrain.

With this setup, a game programmer would need to create classes for each of their entities, each inheriting from the entity class. However, they would not create classes for each scene - every scene is just an instance of the scene class. I'm not sure if this is the proper (most optimal and easiest to implement) system, but it's working pretty well. The programmer will also not need to concern themselves with the terrain management system - they just need to choose which one to use (I envision different ones for different uses). The camera is of much greater concern, because its state is tied to the state and capabilities of the console to which we're deploying.

## The Future State

As you can see on my GitHub repo, I have a shortlist of things to get done for a beta version:

```md
1. Menu system
2. Create a few [different] terrain management options
3. Create a template for VS
4. Write the docs
5. Optimize it
6. Create a samples library of pre-made assets
7. Finish a sample game showing off everything
```

I have an ad-hoc menu system on [another game](https://github.com/IanWold/ConsoleMazeWoot) I made with the engine, and I think that's the best route to go, that's just a matter of getting the work done. I'm much more interested in the terrain management and optimization (the terrain is largely where the optimization is needed).

Then, in the far future, color, time and physics (physics needs time) , and a view manager would be nice. By view manager, I mean something which could display both a scene and menu on the screen concurrently. Furthermore, if the game programmer could control the console settings (to be able to deliver a guaranteed-usable product), that could go a long way to making the engine actually usable.

In the meantime though, have fun with it! I'll write another post about this when I've got a Beta version done (or close to).