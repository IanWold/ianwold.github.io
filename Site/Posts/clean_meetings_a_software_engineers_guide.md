;;;
{
	"title": "Clean Meetings: A Software Engineer's Guide",
	"description": "If being in meetings all day isn't bad enough, spending more time thinking about them seems horrible. Here's a simple guide on making sure you're getting the most out of your meetings.",
	"date": "3 December 2023",
	"contents": true,
	"hero": "photo-1568992687947-868a62a9f521",
    "related": [
        { "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "fileName": "deploying_aspnet_7_projects_with_railway" },
        { "title": "Book Club 10/2023: Functional Patterns in C#", "description": "This month I've focused on functional domain modeling and related patterns. We're just a few weeks away from the release of the next version of C#, and like each previous version it'll introduce even more functional features.", "filename": "book_club_10-2023" },
		{ "title": "An Introduction to Sprache", "description": "Sprache is a parser-combinator library for C# that uses Linq to construct parsers. In this post I describe the fundamentals of understanding grammars and parsing them with Sprache, with several real-world examples.", "fileName": "sprache" }
    ]
}
;;;

If being in meetings all day isn't bad enough, spending more time thinking about them seems horrible. However, meetings are going to continue to be inflicted upon us, and there will come a time (perhaps more than a few) that we'll need to inflict meetings upon our colleagues in turn. Meetings should be short, concise, and mutually beneficial to everyone involved, and in order to ensure their utility it's necessary to be mindful and considerate when facilitating or participating in a meeting.

I've had to hold a lot of meetings in my time. I've spent many years in the roles of engineer, team leader, and architect, and I have to conduct meetings outside of work besides. If you want to go full-on meeting nerd I recommend that you pick up a copy of Robert's Rules, but I wanted to distill my experience and thinking on the matter into an easy-to-follow checklist for the 99.99% of us that don't want to have to spend more time than necessary on one of the admittedly more onerous parts of our profession. So, here's a simple guide on making sure you're getting the most out of your meetings.

Before we get started though, I want to give the single most important advice regarding meetings. If you read no further, take this away: **Meetings are an unfortunate tool of last resort.** Do you actually need to have this meeting? Would an email chain suffice? How about a Slack message, thread, or channel? Uncle Bob says (paraphrasing) that we should strive to write as few comments in our code as we can, and when we do we should acknowledge it as a failure. Take a similar approach to meetings: While maybe not a failure, we should rarely be having meetings and should, let's say, acknowledge that there might be a potential for a better way of working here. (Is that appropriately diplomatic?)

_Note that meetings can take any level of formality between ad-hoc meetings between junior engineers and a senior-level presentation to the CEO. Some meetings require you to write some or all of these down, such as formulating and distributing an agenda, but some do not. Irrespective the level of formality and preparation, it's good to keep these points at least in mind as you conduct meetings at different levels._

# Meeting Structure

The most often overlooked aspect to keeping meetings clean is the meeting structure. Meetings have a **purpose** and an **outcome** that should be specified in an **agenda** (whether it is explicitly written or not) that is understood and agreed to by all participants. Some participants play different **roles** in the meeting, and they will take different **actions** (what Robert would call "motions" but I'm not going to use that language lest I might confuse those of you who use Vim BTW) during the meeting. This might seem low-level, but it's often the details that can keep a meeting productive.

## Purpose and Outcome

A meeting is scheduled with a specific set of participants agreen on a **purpose** and **outcome**: Why is it necessary to gather this group of people and what do they need to achieve? You might need a brainstorming session, you might need to reach a decision or consensus, or you might need to share information. Either way, _be clear about each of these_.

✔️ Do define a **purpose**

✔️ Do specify an **outcome**

✔️ Do ensure all participants understand the **purpose** and **outcome**

❌ Do not neglect this step! Don't neglect the others but especially this!

❌ Do not assume all participants understand or agree with the **purpose** and **outcome**

## Agenda

Simply put, an agenda is a set of topics to discuss at the meeting. This can be as simple as a single bullet point `* Brainstorm a name for the new microservice` or a detailed breakdown of gaps in technical knowledge.

_Side note: if your agenda is "Brainstorm a name for the new microservice that's just a Slack thread. I've been in that exact meeting too many times and I'm here to tell you - your microservice will be replaced by 5 others and deleted in 2 years anyway, just call it "Craig" or something._

Some agendas are written, some aren't, but each meeting _has an agenda_. There are few absolute truths in the world, but I can tell you that meetings which follow the agenda are good, meeting that don't aren't.

Another often-overlooked point is that the agenda needs to be agreed on by all participants. It is not the job of the individual calling the meeting to carve the agenda in stone, it's their responsibility to make sure the agenda fulfils the purposes intended by the participants and the outcomes desired by them. Are these intents and desires contradictory? Sounds like multiple meetings.

✔️ Do be specific about the purpose and outcome in the agenda

✔️ Do communicate the agenda to participants ahead of time

✔️ Do ask participants to contribute to the agenda, both before and at the beginning of each meeting

❌ Do not require all potential participants to attend even if they feel they aren't interested in the agenda

❌ Do not try to write a perfect, final agenda. Instead, write a "first pass" and then give ownership to the other participants

## Roles

Each meeting has roles. At the least, you need to acknowledge:

* **Facilitator** leads the meeting, ensures the agenda is followed, and facilitates discussion,
* **Secretary** records the minutes and important decisions,
* **Participants** engage in the discussion, provide input, and carry out assigned action items post-meeting.

✔️ Do be clear about which role is fulfilled by which participant

❌ Do not assign multiple roles to a single person in important meetings

# Before the Meeting

There are a few steps to take before holding a meeting, and they can greatly help to set the meeting up for success. The most important thing before the meeting is to remember the most important advice: Is this meeting actually necessary? Abort if not.

## Prepare the Agenda

I covered all the dos and don'ts in the previous section, but this is the most important step before the meeitng. Write the agenda down and distribute it as early as you can. Some meetings are regular rituals, and it still helps to write the agenda for these down and distribute them. Sometimes there is requisite technical or business information for the meeting - provide these as resources for participants. If they suggest suggest changes, make them!

✔️ Do include knowledge perrequisite for the meeting in the agenda (links and short descriptions, please)

✔️ Do take feedback - if necessary, send a revised agenda to participants

✔️ Do include any necessary discription or goal for points needing clarification

❌ Do not write a novel as the agenda - stick to bullet points

Sometimes the objective of the meeting is reached while distributing the agenda. If it is - abort the meeting. Your objective is reached. As Sun Tzu says in The Art of War - "The wisest general is the general who never fights."

## Pre-Meeting Preparation

While the facilitator has the most work to understand the agenda and work towards the objective, participants need to prepare as well. Read the agenda, suggest feedback if needed, and study the prerequisite information.

✔️ Do review the agenda beforehand

✔️ Do suggest agenda changes. If you think you might have solved the problem or resolved the outcome of the meeting, even if only minutes before, say so and _abort the meeting_.

❌ Do not attend the meeting if you feel you don't have to. Best to ask the facilitator to drop, explaining why.

# During the Meeting

In a meeting, everyone should be engaged and it should be kept as short as possible. If you're not engaged, drop. I've worked at places where I was told explicitly to not do this, but I would drop anyway and never heard any complaints. Your mileage might vary. If you're facilitating a meeting, let your participants drop.

## Adhrere to the Agenda

The agenda specifies the purpose and outcome of the meeting, and the topics worthy of consideration in furtherance of the objective. At the start of the meeting, the first order of business is to approve the agenda: make any last-minute changes needed by the participants. Once everyone agrees on the agenda, _stick to it_. Topics not on the agenda are to be tabled, either for another meeting or preferrably for a Slack thread.

✔️ Do ensure everyone agrees to and understands the agenda

✔️ Do be clear when moving from one agenda item to the next

✔️ Do cut participants or yourself off if non-agenda topics start being discussed: "Let's table that thought"

✔️ Do allow for a brief period at the end of the meeting for any additional topics if it's a wide-ranging meeting, but be eager to move individual conversations to later meetings

❌ Do not amend the agenda mid-meeting. It was agreed to, and if it's not right now that signifies that we can _abort the meeting_. It's not too late.

❌ Do not "afterparty" - these are separate conversations and either separate meetings (or Slack threads better yet).

## Time Management

The meeting should have a specific amount of time. My favorite, and I am very serious when I say favorite - historical fact is that Winston Churchill would limit all meetings _during the war_ to 20 minutes. **YOUR MICROSERVICE'S RAM CONSUMPTION IS NOT MORE IMPORTANT THAN THE ALLIES WINNING THE SECOND WORLD WAR**.

I don't know how accurate that fact is but I quote it more than once a week on average and in my experience nobody ever checks me on it so it's either true or you can also use it to keep your meetings below 20 minutes. Whatever the historical accuracy, my experience has taught me that 20 minutes is enough for almost all meetings I've had to attend or conduct. Meetings that have gone over 20 minutes deliver exponentially less value to participants per minute of runtime. The ideal length of a meeting is the amount time it takes to compose a Slack message. Oh look at that, your meeting can be a Slack message!

✔️ Do adhere to a strict time limit. Some objectives need to be reached and the meeting extended if not, but the vast majority don't. Set the expectation with everyone that you'll hold them to a timeframe and, as if by magic, the meeting will resolve in the right amount of time.

✔️ Do keep conversation flexible but insert yourself when it needs to move along.

✔️ Do set approximate time limits for top-level items.

❌ Do not tell participants about those time limits - just convey how long the meeting will be.

❌ Don't harp on participants about time, they're doing their best to navigate the meeting. It's always OK to politely interrupt with a brief reminder about time when necessary.

## Take Notes

Take notes. This is why I enumerated "secretary" as one of the roles in the meeting. Someone other than the facilitator should be writing down notes. "Even for daily standup meetings?" you might ask. Well, you should consider whether those are necessary, but yeah you probably should be taking notes if you're having that meeting.

Notes will serve as important documentation after the fact for participants about what's been discussed and agreed to. What's more is that not everyone in your company can be in every meeting, but you need to make sure that information is available to everyone in your organization. Keep minutes to allow everyone to revisit them when necessary.

✔️ Do appoint a "secretary" to take notes

✔️ Do share the notes with everyone during the meeting

✔️ Do take note of important questions, information, and decisions

❌ Do not record every word of every participant

❌ In fact, do not record the actions of specific participants unless it's absolutely necessary

❌ Do not let everyone amend the minutes during the meeting - have one person dedicated to this task (brainstorming sessions maybe aside)

## Everyone Must Participate

If you have a participant who isn't participating, then you don't have a "participant" - you have a voyeur. I'm creeped out by voyeurs, and you probably are too. Excuse them.

Everyone in a meeting should be participating, either actively listening or contributing to the present conversation. If you're facilitating and notice someone isn't participating, ask their opinion. Create an atmosphere where they're welcome to share their opinion, no matter their seniority (either rank-wise or domain-wise). Participants asking clarifying questions, no matter how basic they may be, must be encouraged.

_Side note on the topic of conversations: if a participant gives a hostile answer to a question, call them out on it. This is difficult but it's a win-win - the participant at the receiving end of the hostility knows you have their back, and the hostile participant (and everyone else) knows that behavior isn't acceptable._

✔️ Do encourage a diversity in opinions and voices

✔️ Do ask everyone their thoughts regularly

✔️ Do actively engage in the conduct of the meeting and encourage a positive atmosphere

❌ Do not let participants be passive in a meeting

❌ Do not overlook non-verbal cues in virtual meetings; they can be indicators of agreement, confusion, or the desire to speak

❌ Do not avoid tough conversations. About that...

## Engage Difficult Conversations

Meetings are called for specific purposes - sometimes to reach decisions, sometimes to explore new ideas, or any multitude of reasons that might touch on the passions of several participants. Meetings can bring up difficult, complicated, or heated conversations or even arguments. Make no mistake - this is good, and it's a good sign that a meeting was needed if this happens. A team that is passionately engaged is infinitely more preferrable than a team that always agrees with itself. There will always be a difference in opinion, and the best thing for the team(s) involved is to engage these points head-on.

✔️ Do allow heated conversations and respectful arguments

✔️ Do ensure the topic is stuck to during these moments, and ensure everyone maintains the overall focus of the meeting

✔️ Do act to keep participants in-line when necessary

✔️ Do suggest a breather if necessary. Do reprimand participants if they cross a line

✔️ Do create a safe space for dissent and disagreement. DO emphasize the focus on ideas and not individuals

❌ Do not shut down difficult conversations or honest arguments

❌ Do not disengage if an argument comes up

# After the Meeting

There's still a bit more work to do after the meeting - you're not done yet! All the more reason to respect the time of the meeting, or to try to avoid it altogether. Remember that what was discussed needs to be appropriately documented and easy to reference for participants. Remember also that not everyone in your organization was able to attend this meeting, but goodness knows when the topics discussed will impact them.

I recommend keeping your meeting minutes in a single place that's easy to reference for everyone in your organization. Products like Notion, Monday, or Confluence allow you to add tags and @ members as necessary, making them searchable too.

## Finalize the Minutes

After the meeting, ask all participants to spend a minute or two to add anything to the minutes that might have been missed. You asked them to refrain from adding anything to the minutes during the meeting so they could stay focused on the conversation, now that the conversation is over they can add any extra context they need.

✔️ Do leave the minutes editable, at least for a period of time. Preferrably store them in a system with history tracking.

✔️ Do follow up and ensure everyone agrees on the final minutes

✔️ Do broadcast to your organization that your meeting is done and the minutes are available

✔️ Do set a deadline for when minutes must be finalized and shared post-meeting

❌ Do not fail to publish the minutes

❌ Do not allow a "he said, she said" fight in the minutes after the meeting

## Follow-Up Actions

A lot of meetings will result in participants being assigned specific tasks. You should either follow up with them in the appropriate timeframe, or ensure their managers do, that they've completed these tasks. It's good to update the minutes at this time to reflect that this was done, and if possible link to the result.

✔️ Do ensure all participants are clear on follow-up items

✔️ Do record follow-up work in the minutes

❌ Do not assign follow up work after the meeting

# Keep in Mind Always

## Meeting Etiquette

✔️ Do be mindful of your role, participation, and conduct during meetings

✔️ Do respect everyone's time

✔️ Do respect everyone's individual contributions

✔️ Do adhere to virtual meeting norms, such as muting when not speaking

✔️ Do leave, not attend, or _abort_ the meeting if you can

❌ Do not dominate a meeting, either as a participant or a facilitator

❌ Do not underestimate the impact of your physical environment in virtual meetings (e.g., background, lighting)

## Regular Review

✔️ Do regularly assess the necessity and effectiveness of meetings, especially regular rituals

✔️ Do be open to feedback and make adjustments as needed

✔️ Do make adjustments or _abort the meeting_ if and when necessary

✔️ Do periodically ask if the frequency of regular meetings is still appropriate or if adjustments are needed

❌ Do not ignore patterns of unproductive meetings. If certain meetings consistently fail to achieve their objectives, it's a sign that they need to be reevaluated

❌ Do not continue meetings just because they are a routine

## Diverse Perspectives

✔️ Do value and seek a range of opinions and ideas, and encourage participation from everyone

✔️ Be mindful of inclusive participation, especially in diverse teams

❌ Do not allow the same individuals to dominate the conversation in every meeting

❌ Do not dismiss ideas without proper consideration, and do not create an environment where only certain opinions are valued over others

# Conclusion

Yeah that's a lot of bullet points, but I have a lot of ideas on this topic. I feel strongly that meetings are very productive tools, but there's a lot of awkwardness in our industry around how any why we have meetings. Be considerate and hold your meetings with intention and purpose, drive towards your outcome, and your meetings will work for you and all of your colleagues.

Above all, remember: _you probably don't need a meeting for it._
