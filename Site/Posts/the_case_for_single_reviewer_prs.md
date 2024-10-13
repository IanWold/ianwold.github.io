;;;
{
	"title": "The Case for Single-Reviewer PRs",
	"description": "Or, strengthening your team and its code with communication, professionalism, and trust.",
	"date": "13 July 2024",
	"contents": false,
	"hero": "photo-1559629304-47a70c8c042e",
    "topics": ["Processes"],
    "related": [
		{ "title": "Develop Effective Coding Standards", "description": "Bad coding standards are worse than no standards, and even good standards are sometimes unnecessary. What's the utility in coding standards, and what makes a good one?", "fileName": "develop_effective_coding_standards" },
		{ "title": "It's Better to be Consistently Incorrect than Inconsistently Correct", "description": "On consistency in code and what it means for something to be 'incorrect'", "fileName": "its_better_to_be_consistently_incorrect_than_consistently_correct" },
		{ "title": "Reclaim Your Agile: The One Clever Trick Agile Coaches Don't Want You to Know", "description": "What if I told you there's one trick to being able to reshape your team's development process without your company knowing it? What if I told you that you can achieve actual Agile even though you work in a Scrum firm?", "fileName": "reclaim_your_agile" }
    ]
}
;;;

As with all things, there are a lot of different practices our teams might adopt around pull requests (which I prefer to call "peer reviews", but we can agree on "PRs"). Different teams in different environments or circumstances will certainly benefit from different processes. If you've suddenly inherited a huge legacy codebase and you need features _now_ then you're going to have a different process to a team that's cooking up large batches of copypasta microservices because your startup just got too much funding and decided to justify it by blowing money on AWS.

The majority of teams doing actual development in the real world (the percentage of teams this describes is inversely correlated with the amount of VC money available, I assume) don't need a whole lot of process. Less is more, really. There's one process that I prefer as a baseline for most teams, light enough to be able to flex when it needs but still providing comprehensive benefits.

This is the single-reviewer PR, where each engineer on a team assigns each of their PRs to a single other engineer on the team on a rotating basis per PR, such that each member of a team has an even number of PRs from each other member throughout the course of development.

Maybe you've tried some form of this before and it didn't work out, or maybe you've never tried it before and it doesn't sound quite right to you. Maybe you like this, but you're trying to articulate the benefits of this scheme to a skeptic on your own team. I'm going to try to show the benefits of this setup and extra considerations you might need to make, and I'm going to do my darndest to not say "oh, you just didn't do it right!"

# No Review Jam

Having multiple reviewers on a PR can lead to a situation where each reviewer implicitly defers to each other reviewer. "There's 6 reviewers on this PR, surely _someone_ will get to it!" This makes PRs take longer to be reviewed, or I have to constantly pester my peers for a review. I might need to constantly field questions like "Am I _really_ the right one to review the PR?"

If the process requires multiple approvals, or approvals from specific people, has the problem of creating bottlenecks. These are particularly insidious; I've seen such processees implemented before to the effect that PRs would take multiple days on average to merge.

Having a single reviewer eliminates all these issues. It's a level set with the team - you're going to get PRs assigned to you and you need to review them then. There's no deferring to other people, and if you're a teammember who consistently fails to find time to review PRs then we have an opportunity for a conversation.

In typical fashion, I would keep a running tally in my head of the next engineer who'd be picking up my next PR, but there are advantages in massaging that order now and then. I worked on one team with a member in Frankfurt, and if I had an early evening PR in I knew I could assign it to him and have a review by the next morning.

# Ultimate Knowledge Distribution

Most teams do, I think, value distributing knowledge across each of its members. This is such a significant consideration that [Uncle Bob suggests it be part of our code of ethics](https://www.youtube.com/watch?v=BSaAMQVq01E). Not only is the single-reviewer PR the best _PR process_ to encourage knowledge distribution, it might be the best single _process_ to do so.

Each member of the team - from the junior out of college to the principal who's been there just a _bit_ too long - is going to be reviewing the full breadth of work that the team produces. Of particular importance to those lower on the ladder, the juniors will be reviewing PRs from the principals.

It becomes everyone's professional responsiblity to take the time to understand what's being engineered. While you'd expect a more senior engineer to be able to provide a good review for a more junior engineer, they're advantaged by being required to consider the full breadth of work. The junior though is particularly assisted by needing to consider sometimes quite advanced code. There's no hiding, you've got to learn it.

Now, we certainly wouldn't consider it good to leave any engineer alone with a bit of code they don't understand and ask them to sign their name to it. These cases give us the extra advantage of promoting communication. When I get assigned a PR that doesn't make a lot of sense to me, I want to have two video calls at least (assuming we're remote): one with the author to walk through the _what_ and _why_ of the PR, and another call with a second engineer who understands the code better to answer any more technical questions. You might adopt something else, but this gives me great contact with the team. I especially recommend this for juniors to maximize exposure to others' knowledge.

Here's the most fun I have with this process: when someone is about to open a PR in an area that one member of the team knows a lot about, I will suggest that they ask for a review from anyone _but_ that engineer. Indeed, the reviewer will (probably) need to reach out to that knowledgeable engineer in order to complete the review. This is, to me, the easiest way to achieve this sort of knowledge distribution.

# Quality Reviews

If you haven't worked this way before, it might not seem intuitively obvious that it produces, I think, the highest quality reviews on average. I think the inclination would be that by distributing PRs around, you'd get quality reviews from those more experienced but not necessarily from others.

Instead, I find that the review-ability of everyone on the team increases with this scheme. The key is it's not just that _you_ are receiving reviews equally from each member of the team, but that _they_ are receiving reviews equally as well. After a few cycles of reviews, it becomes clear who is good at giving reviews and who isn't. Behavior tends to normalize across the team with more contact between its members, and team members are biased towards adopting the practices of the quality reviewers.

This, as with all things in software, requires communication, but the advantage is that the communication between members of the team is _at the minimum_ required through this PR system. Indeed, it naturally creates more communication between members on average. This is the key that causes quality reviewership to spread across the team. And again, if there's one member who is consistently failing to pick up what's being put down, there's an excellent opportunity for a conversation.

# Transparency and Trust

The natural outcome of increasing both knowledge distribution and communication across the team is increasing transparency. Team members are more aware of what's happening in the code, what features and bugs are being tackled, and even just how their peers are feeling. Sometimes it's the small things that we don't pay enough attention to.

Our tooling nowadays usually boast excellent automation features. One that I particularly like is when a distributed team uses one software for communication (Teams, Slack, etc.) you can set up an automation from GitHub to copy a conversation between a PR in GitHub and a thread on your communication platform, ensuring that your PR conversations are all recorded in the same place all of your other conversations are. This makes it incredibly easy for others to follow along, or to go back over previous conversations and decisions at a later date.

Now here's one disadvantage you might be imagining. Wouldn't members of the team who _feel_ a particular attachment to some part of the system interject themselves here? Indeed, I have seen this happen, but it subsides over time. Because this process encourages knowledge distribution and accountability across the entire team, I find that members trust each other much more with this process, and tend not to interfere. Indeed, interference should be discouraged as in the PR the author and reviewer are engaged in a sort of mini pair programming session; their decisions are professional and trustable.

More often than not - in fact almost always - when I see the situation that one member feel a knee jerk need to intervene, it's a learning opportunity for them. Frequently the author and reviewer are engaged with a new problem which alters some fundamental part of the original code. Sometimes the documentation is entirely insufficient leading to maybe more of a reach in th code than what's strictly required. These are not cases which require interference, they're cases where the original engineer can stand to learn - maybe to learn from a mistake they made or to learn from the wisdom of their peers. And, yes, sometimes it's a principal engineer learning from the wisom of two junior engineers struggling to comprehend the demon-speak of some terrible regex. Shame on you for checking in that regex in the first place!

In reality though, these situations are rare even though they're incredibly beneficial (if for unexpected reasons). The knowledge distribution naturally causes a _work_ distribution, and paired with the increased trust across the team means that a codebase will, over time, have fewer and fewer landmines of particular engineers' passions. It encourages us to keep attachments at a professional distance, and allows the sort of iterations necessary across the entire codebase.