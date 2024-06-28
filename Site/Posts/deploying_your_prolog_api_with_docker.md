;;;
{
	"title": "Deploying Your Prolog API with Docker",
	"description": "It can be tough living on the bleeding edge of modern technology. If you've jumped on the hype train and developed your latest API with Prolog only to find there aren't any tutorials to Dockerize it - look no further!",
	"date": "28 June 2024",
	"contents": false,
	"hero": "photo-1464095718138-d9c47312eac5",
    "related": [
		{ "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "fileName": "deploying_aspdotnet_7_projects_with_railway" },
        { "title": "\"Should I Learn (Insert Some Tech Here)?\"", "description": "One of the most common questions - would it be good to learn this or that language, framework, database, etc? Taking even a little time to learn something new is good all around, but is it really worth making an investment in yourself to grow personally and professionally? Let's take a deep dive into this topic.", "fileName": "should_i_learn_insert_some_tech_here" },
        { "title": "My (Continuing) Descent Into Madness", "description": "It started simply enough, when I asked myself if I should try an IDE other than Visual Studio. Mere months later, I'm now using a tiling window manager. This is the story of my (continuing) descent into madness.", "fileName": "my_continuing_descent_into_madness" }
    ]
}
;;;

[Prolog](https://www.swi-prolog.org/) is truly a leader at the forefront of modern technology, and if you're anything like me then you're convinced that this langauge is the way forward for our microservice APIs. Part of our evidence for this (as though it isn't inherently obvious) is how easy it is to containerize and deploy with Docker.

To demonstrate this, let's set up a simple Hello World API with SWI Prolog:

```prolog
:- use_module(library(http/thread_httpd)).
:- use_module(library(http/http_dispatch)).
:- use_module(library(http/html_write)).

:- http_handler('/', handle_request, []).

handle_request(_Request) :-
	format('Content-type: text/plain~n~n'),
	format('Hello, World!').

server(Port) :-
	http_server(http_dispatch, [port(Port)]),
	thread_get_message(_).
```

There's a couple things to point out here. First, accepting a `Port` variable for the `server` predicate allows us to keep the port flexible as an environment variable, best to not hardcode that. Second, invoking the `thread_get_message` predicate isn't necessary when we're debugging locally but it is going to be necessary when we run it in Docker to prevent the server from halting immediately.

The dockerfile isn't terribly difficult at all. We can base off of `ubuntu:latest`, install SWI Prolog, and then run the command to start the prolog interpreter with the call to the `server` predicate:

```dockerfile
# Use an official Ubuntu runtime as a parent image
FROM ubuntu:latest

# Set the working directory
WORKDIR /usr/src/app

# Install SWI-Prolog
RUN apt-get update && \
    apt-get install -y software-properties-common && \
    apt-add-repository ppa:swi-prolog/stable && \
    apt-get update && \
    apt-get install -y swi-prolog

# Copy the current directory contents into the container at /usr/src/app
COPY . .

# Make port 5000 available to the world outside this container
EXPOSE 5000

# Run swipl when the container launches
CMD ["swipl", "-g", "server(5000)", "-t", "halt", "server.pl"]
```

I'm hardcoding `5000` as the HTTP port here but it's just as easy to use an environment variable at this point. Note too I'm assuming the source is in `server.pl`.

With this we can build the image:

```plaintext
docker build -t prolog-server .
```

And deploy:

```plaintext
docker run -p 5000:5000 prolog-server
```

And that's it, we're off to the races! With dev ex this smooth, Prolog will concur the world. Any day now. Surely.
