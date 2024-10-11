;;;
{
	"title": "Guerrila DevEx Testing",
	"description": "Developer experience is subjective. Employ the 'hallway test' method to ascertain your code's quality.",
	"date": "11 October 2024",
	"contents": false,
	"hero": "photo-1708358097162-be6494dd44b3",
    "related": [
		{ "title": "The Case for Single-Reviewer PRs", "description": "Or, strengthening your team and its code with communication, professionalism, and trust.", "fileName": "the_case_for_single_reviewer_prs" },
		{ "title": "Reclaim Your Agile: The One Clever Trick Agile Coaches Don't Want You to Know", "description": "What if I told you there's one trick to being able to reshape your team's development process without your company knowing it? What if I told you that you can achieve actual Agile even though you work in a Scrum firm?", "fileName": "reclaim_your_agile" },
		{ "title": "Don't Retro the Same Twice", "description": "Different retrospective formats are mostly the same thing in different flavors - don't argue about them; try all the flavors (at least once)", "fileName": "dont_retro_the_same_twice" }
    ]
}
;;;

I first became aware of hallway usability testing (also referred to as "guerrila" testing) at the beginning of my career when I was encouraged to read [The Joel Test](https://www.joelonsoftware.com/2000/08/09/the-joel-test-12-steps-to-better-code/), a now-famous article on what makes a good team. It's a brilliant article, and at more than two decades on I think it's still foundational as to how I think about the health of a team or a project. It contains the following description of `hallway usability test`:

> A hallway usability test is where you grab the next person that passes by in the hallway and force them to try to use the code you just wrote. If you do this to five people, you will learn 95% of what there is to learn about usability problems in your code.

Here, the phrase "use the code you just wrote" means you're going to _execute_ the code and ask the participant to use your actual software, navigating your UI and simulating a user. I love this phrasing though because that isn't where my mind went the first time I read that sentence. No, I instead thought it might be more interesting to yoink other engineers from the hallway and ask them to navigate my code. Not a code review, not some full architectural scrutiny, but a real-world test of whether my code actually works as _code_.

Nowadays we have the term "developer experience" to refer to this concept of the developer-usability of a piece of code or a development environment or the like, though this is a famously hand-wavey term. Especially considering my code's quality, I'm not sure there's ever going to be a great objective way to measure that. At the broad level, different groups of engineers are going to be biased in different ways towards one pattern or practice or another. Different styles are better suited for different problems or types of applications, and even throughout the lifetime of a codebase we might find that changing styles might work better with a changing environment around the codebase.

And yet it isn't nonsensical to talk about code quality or developer experience more broadly. While disagreeing on specifics we've all worked on codebases that were easy to modify or extend, as well as codebases that aren't. A few colleagues on the fringe might suggest - intending to argue that "developer experience" is either meaningless or not worth focus - that instead of good or bad codebases there's just varying levels of bad. Beware of this rhetorical play: it's still an admission that there do exist codebases of a higher quality than others! Developer experience is a finicky thing to capture but worth our consideration.

So that raises the question as to how to consider developer experience. Can I measure it? Well, I can't quantify it - please comment below if _you_ can! However, when I write bad code, or at least code which my colleagues consider bad, they're usually quite ready to let me know. Giddy, sometimes, to point out my faults. Being fair, I do have a blog which allows me to anonymously gripe to the world - literally _tens_ of people on a good week - about _their_ bad code.

This is how I gauge developer experience, and it's the basis for a lot of practices I've developed over the years. This is a terribly imprecise art, considering not just the subjectivity of the matter but also the large number of variables at play that can alter how we think about quality in code. That said, I defy you to come up with a better method than this: watch your colleagues "use" your code. Sit them down and watch them debug an issue or add a feature.

This is the hallway devex test, and as I mentioned it is higher-level than a code review and lower-level that some fine-grained walk through a whole codebase. Code reviews are done frequently and periodically focusing narrowly on the bit of code being added or removed, while the latter exercise is so detailed that I've only ever heard of such practice in theory. This test is the in-between: given a functioning codebase (one that's in production), how well does it align with our [quality attribute requirements](https://www.infoq.com/articles/avoid-architecture-pitfalls/)?

Running this test is very easy. Given a codebase, you surely have a set of bugs or simple features you've identified which could be worked on. From your firm's hallway (or some sort of Slack roulette if you're remote) yoink a colleague and promise to give them coffee and/or doughnuts in 30 minutes (or an hour or whatever amount of time makes sense) under one condition: they have to work on a card you give them on your codebase. For what it's worth, the promise of a doughnut is typically unsatisfactory over Slack; in this case you can just promise to [subscribe to your colleague's book club](https://buttondown.com/ianwold).

The crucial step is to watch your colleague. Look for where the pain points are, look for the assumptions they make, and look at the expression on their face. If you ask them how they feel or what they think, you'll get absolutely nowhere. Humans are funny beings; we're bad at knowing how we feel and sometimes we're bad at knowing what we think. There's frequently a disconnect between a person's actions, beliefs, and perceptions. Crucially, there's often a disconnect about what we think or say about our own actions, beliefs, and perceptions, so you can't trust me to describe myself to you. Further, you can't directly observe my beliefs or perceptions. However, you can directly observe my actions _and you don't need to take my work for it_.

How I go about _doing software engineering_ on your code is what reveals my developer experience with your code. How your other colleague works reveals _their_ developer experience with your code. In a way this cuts down to exactly what "developer experience" is: you're observing the experience of your colleagues doing developering with your code. As Joel said of usability testing, just 5 hallway passersby will teach you 95% of what you need to know - I find this largely true for devex as well. This test is easy enough to run periodically, and from there you can develop a feedback loop of collecting pain points, addressing them, letting the codebase evolve a bit further, and repeat.

What specifically you're looking to address is tough to universally quantify - the types of experiences working on an embedded system will be quite different from a CRUD webpage or a video game. You might even find your colleagues reveal whole new categories of pain points you hadn't previously considered! I don't think about this too much beforehand, but I can think of a few high-level questions I typically look to resolve:

1. Was it easy for my colleague to understand what the codebase is doing and how? Do they understand the architecture, patterns, conventions, and structures in the code?
2. Was my colleague ultimately able to identify where to make the change or insertion?
3. Did my colleague feel or become confident with the codebase? Did their confusion increase or decrease over time? Did unexpected side effects occur with their changes?
4. What role did the development environment play - did they have difficulty working with the code in their IDE? More fundamentally, were they able to get it building right away?
5. Where did they become confused? Where did they become frustrated? Where did they have to reach out to me? Can I identify styles, patterns, or such which they dislike?

In conclusion, I submit that "developer experience" is a useful subject for consideration, and in spite of its subjective nature I feel that I can say, definitively, that developer experience can be observed by observing the experience of developers. The hallway or "guerrila" test works for me to gauge this quality, and I'm sure it can work for you as well.