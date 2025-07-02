;;;
{
	"title": "Successful Personal Projects",
	"description": "The process and thinking I follow to follow through on personal projects and get what I want out of them.",
	"date": "2 July 2025",
	"contents": false,
	"hero": "photo-1497561813398-8fcc7a37b567",
    "topics": ["Projects", "How-To"],
    "related": [
		{ "title": "The Modular Monolith Won't Save You", "description": "I must once again insist there are no silver bullets; knowing architectural patterns is no substitute for knowing how to write software.", "fileName": "the_modular_monolith_wont_save_you" },
		{ "title": "Three Laws", "description": "Some 'folk laws' that are commonly known but seldom applied.", "fileName": "three_laws" },
		{ "title": "Guerrila DevEx Testing", "description": "Developer experience is subjective. Employ the 'hallway test' method to ascertain your code's quality.", "fileName": "guerrila_devex_testing" }
    ]
}
;;;

I have this one specific psychic power, when I speak to a software engineer I can tell them how many half-finished, abandoned, sometimes barely-started personal projects they have scattered between source control providers and dusty hard drives. I can even prove to you I have this psychic power! Just focus your mind on your favorite color, and I'll tell you now how many such projects you have. Ready? The answer that's coming to me is "a lot."

Uncanny, isn't it?

We all spend a lot of time with software, and we spend a lot of time thinking about cool software things we can do. We know we have the ability to create these things, we see others releasing their fun projects, but this or that happens and our project stalls. Then the next one and the next one, to the point that for every successful project we've released we have several unfinished ones.

This isn't a problem in itself; they're on our own time and impacting nobody. However, we did start each project for _some reason_ (presumably), and failing to have finished a project is to have failed to get what we wanted from it. Maybe we wanted to learn a new technology, fully develop an idea, or have the satisfaction of having built something. Consistently engaging with personal projects - and that means finishing them - comes with a number of benefits in increasing our abilities and confidence in our work.

I'm not going to claim to be great at finishing personal projects, but I can claim to frequently see folks stepping in the same potholes I've learned to avoid, so I can share my approach to personal projects.

## 1. What do you want to gain?

The most important consideration for a personal project is having a realistic personal goal. Not a product goal necessarily, but a _personal_ project is done, ultimately, to fulfil a _personal_ need or want. What is that goal? Are you trying to learn a framework, a pattern, a protocol, or a language? Are you trying to gain a deeper understanding of a technology you commonly use? Are you trying to deliver some kind of product - for yourself, a friend, or a club? Are you trying to showcase your work to colleagues, potential employers, or a blog audience?

I'm under no illusions that the majority of personal projects are begun under the motivation of creating some end result; most are not contrived after the decision upon the _personal_ goal. That's entirely fine, and I think it's short-sighted to consider such a workflow as putting the cart before the horse. I'm definitely more inspired thinking about delivering a cool thing than I am by a satisfaction with being able to bring up cool facts about [RFC 1605](https://www.rfc-editor.org/rfc/rfc1605).

Nonetheless, before you start building you must be upfront about what the personal goal is and make sure you define it well. To reiterate, because these projects are done on _personal_ time with _personal_ resources, they won't be successful without being rooted firmly in _personal_ aims.

## 2. Define done

Indeed, there's no escaping the same "definition of done" we must engage in at work. The only real difference being that your definition of done for your personal project should be significantly lower in scope than you have at work. Descope everything and MVP-max. It's not glamorous but it is realistic. Sure I know that I _can_ deliver a set of 12 features for a project, but I also know that I _definitely will_ deliver the project if it's only 3 of those 12 features. It's not insulting to my abilities to descope the project, it's a guarantee that I can achieve what I want to achieve. Remeber, delivering the resulting software is subordinate to filling my personal requirements.

Done means two things: the software is working at the state that I want _and_ I have achieved my personal goal. These go hand-in-hand: the software and technology need to support the personal goal, and the personal goal constrains the scope of the software. Just as I'm focusing on a single personal goal, I'm focusing on achieving a single piece of software, that does one thing. [It's okay if it's a little ugly](https://goodinternetmagazine.com/my-website-is-ugly-because-i-made-it/).

The faster I can get to "done," the better. Sensible personal goals and super-MVP-ified software gets me there. This always leaves extra bandwidth at the end of the project for me to continue it. It's a separate project in itself, often, to add more features to this piece of software - here we can rinse and repeat the personal project cycle. Or, I can publish the project open source and have a very helpful backlog of interesting addons. Sometimes others on the internet do engage with your projects this way!

## 3. Architect and develop pragmatically

Or, put another way, focus on the core competency of the project. If your project requires auth but your goal is to learn about VOIP, don't write your own auth layer. If your goal is not to learn a new language then use a familiar one. There's no need to dress your codebase up with all of the bells and whistles endemic to professional engineering. C# and Java code tends to be riddled with too many interfaces, passthrough layers, and unit tests. Your personal projects sute don't need unit tests, not out of the gate at least. 

Architecture is a very common pitfall. Really consider what you actually need. If you can get away with hosting your site on GitHub pages then do that. If you only absolutely require one small piece of server functionality then spin up a serverless function (I love [Railway's functions](https://docs.railway.com/reference/functions) for this). Seriously interrogate every piece of infrastructure. Do you _need_ Postgres or can you get away with a JSON file or [Google Sheets](https://www.levels.fyi/blog/scaling-to-millions-with-google-sheets.html)? Do you _need_ a server or can you serve using existing technologies? Do you _need_ all your favorite NPM packages or do you just need [simple reactivity](https://ian.wold.guru/Posts/i_like_petite_vue.html)?

Remember you've only got one goal, not one main goal and a bunch of secondary ones. I'm sure you'd really enjoy learning a new JS framework, but if that isn't the one goal of your project then that's not what we're going to be doing on _this_ project, we'll be doing it in another project. Conversely, if your one goal _is_ to learn a JS framework then our software isn't going to be using an unfamiliar or burdensome architecture, and it definitely should have similar functionality to other projects you've developed before!

To keep development from stalling, I specifically keep two things in mind. First, I want to get to an executable piece of software as step #0 in development; I won't start writing any code until I can execute code, and then I'll keep the project ni a state of being able to run. Second, I'll always focus on the difficult and "core competency" component first, at the exclusion of anything else; if a CSS style is taking too long I'll let it be ugly until I can get the core component where I want it.

## 4. Finish it

Every step you take on the project should be a tangible, obvious step towards the end. Because you've kept your project scoped as low as possible you'll have the time and energy to do the coding work, and because you're clear about your personal goal you'll identify when you've achieved that.

If you're spinning your wheels you'll need to reevaluate what you haven't scoped down. Sometimes we make mistakes, sometimes projects take off on the wrong trajectory and need to be seriously rethought. Sometimes projects do need to be killed - not abandoned, but killed. I advise restarting with the same personal goal and software concept, but after a rearchitect. That itself might be an architecture project!

When we finish something we get the satisfaction of having finished, the confidence that we _can_ finish it, and we've gained whatever knowledge, experience, or product that we set for ourselves.
