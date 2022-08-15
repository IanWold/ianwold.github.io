# ianwold.github.io

My personal website. Generated with [Metalsharp](https://github.com/ianwold/metalsharp).

This website is a bit of a "beta-run" for Metalsharp, and as such it isn't available at ianwold.com (that's still a SquareSpace site), and the [code](https://github.com/IanWold/ianwold.github.io/blob/master/src/Build/Program.cs) is ugly. As Metalsharp progresses towards v1, this code will (hopefully) clean up nicely. Once a working version of Metalsharp is got, I'll get it up on Nuget, and that should allow me to be entirely rid of the `Build` solution, and use a single C# script file instead (along with `scriptcs` to manage the nuget packages). I'm not sure whether that will reduce the number of files I need to check into git, but it will make more sense to navigate to `/src` and see `build.csx` instead of `build.sln`.
