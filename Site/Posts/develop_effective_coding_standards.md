;;;
{
	"title": "Develop Effective Coding Standards",
	"description": "Bad coding standards are worse than no standards, and even good standards are sometimes unnecessary. What's the utility in coding standards, and what makes a good one?",
	"date": "14 February 2024",
	"contents": false,
	"hero": "photo-1523240795612-9a054b0db644",
    "topics": ["Standards", "Processes"],
    "related": [
		{ "title": "Eight Maxims", "description": "A few principles for thoughtful software engineering.", "fileName": "eight_maxims" },
		{ "title": "Clean Meetings: A Software Engineer's Guide", "description": "If being in meetings all day isn't bad enough, spending more time thinking about them seems horrible. Here's a simple guide on making sure you're getting the most out of your meetings.", "fileName": "clean_meetings_a_software_engineers_guide" },
		{ "title": "Reclaim Your Agile: The One Clever Trick Agile Coaches Don't Want You to Know", "description": "What if I told you there's one trick to being able to reshape your team's development process without your company knowing it? What if I told you that you can achieve actual Agile even though you work in a Scrum firm?", "fileName": "reclaim_your_agile" }
    ]
}
;;;

Coding standards can be a blessing or a curse: a practical resource that helps the team consistently maintain their codebases or an overbearing cudgel of impractical formatting prescriptions. Indeed, ineffective coding standards are worse than no coding standards: they are frustrating and can exacerbate any maladies on the team. I'd go so far as to suggest that even _effective_ coding standards are not a necessity in the industry; many teams might naturally have no need for standards.

There are several situations where it's beneficial to have a solid set of standards - if the team has a large number of codebases (say, microservices) in various states of disrepair, if the team needs to onboard engineers frequently (say, for intern/junior rotations), or if your team is embarking on a 1+ year refactor of a large, legacy system with too many different styles. These are all situations where good coding standards can help ensure that a whole group of people is aligned over a period of time.

I'll admit failure on my part here. I have created some godawful coding standards in the past - ones which have an exclusive focus on style that explain nits for twenty pages. I hope I've learned from my younger self, that proper standards are a very different beast. I like to think that since then I've been able to establish some standards which have been quite good. If you take nothing else away from this article, remember that the practice of creating and maintaining standards is an active, full team activity. The whole team should be engaged in developing the standards, and they should be reviewed by the whole team regularly.

# Less is More

Bad coding standards are meandering swamps of useless bloviation. It's not useful to anyone to have fifty pages listing out "Do this, not that" steps. I should be able to skim the standards quite quickly and come away with a solid understanding, if just a feeling, of the way this team wants its code developed.

Focus on what's truly important. You'll have plenty of "Do this, not that" examples, but use those only to disambiguate where necessary. Explain key, high-level concepts. Avoid getting too far into the weeds of specifics. Your goal should be to lay a solid foundation on top of which any number of beautiful houses can be built.

# Focus on "Why"

Prescribing rules for coding is generally useless. They are annoying and can get out of date quickly, so they end up ignored in the long run. They get overlooked on review because they fade into the background when the document is in active use.

What's more important - 1000 times more important - is to document why your team feels a certain way. Suppose your team prefers having a strong emphasis on being able to read your code more vertically than horizontally. You might want one rule to limit the width of the document, one rule to format ternaries on multiple lines, one rule to break conjunctions in conditionals on multiple lines, etc. Rather than stating each of these individually, they can be examples in the broader context of verticality.

```plaintext
## Prefer Verticality

This team prefers writing code in a way that makes it more _vertical_ than _horizontal_. Run-on lines tend to be more difficult to read given our domain and architecture. As a consequence, the team is able to read code faster when it's consistently formatted vertically. We like to limit the horizonal width to 80 characters, and break code into multiple lines in logical places. For example:

**Use multi-line ternaries**

Do not prefer:

var myThing = first >= 50 ? first : second;

Do prefer:

var myThing = 
    first >= 50
    ? first
    : second;

**Break long conditions into multiple lines**

Do not prefer:

if (thingToCheck is SomeThing someThingToCheck && service.CheckSomeThing(someThingToCheck) && businessConditionForNextLogic && featureFlagConditionForNextLogic)

Do prefer:

if (
    thingToCheck is SomeThing someThingToCheck
    && service.CheckSomeThing(someThingToCheck)
    && businessConditionForNextLogic
    && featureFlagConditionForNextLogic
)
```

By explaining why, you're giving everyone an insight as to what the team feels, not just the result of their feelings. You don't need to enumerate every single rule that follows from that feeling since you've (hopefully) articulated the _why_. It's easier to refactor these statements as your team's feelings shift since you've written _those feelings_ down - the consequences might be harder to grasp.

# Continuous Learning, Continuous Improvement

I hope that you and your teammates are learning constantly, and always becoming better engineers. As your team changes, its opinions on how code should be developed will change too. This should be reflected by continuous reviews of the coding standards. First though, you need some way to capture _that_ you're learning and _what_ you've learned.

Retros are the obvious candidate - a "What did you learn in the last two weeks?" question is great. It's less useful for capturing changes in your coding disposition over time though. Monthly or quarterly learning check-ins can be good for these, but whether that works for your team to adequately capture those and translate them into changes in this document are an individual question.

Your team should establish some periodicity by which it reviews all of its documentation, and your coding standards should be part of that. This can be monthly, quarterly, or yearly - just what works for you. Importantly though, consider starting these off by asking everyone on the team what they learned in that time. Did they experience anything cool or interesting, out of their usual development practice? Are they excited by something new they saw they wanted to try?

However you collect it, document learnings and use them to drive the review of the coding standards. This feeds back on explaining the _why_ and not the _what_ in the document itself.

# Coding Standards Affect Culture

If the standards are too nitpicky, you'll start seeing too many nits in code reviews. If the standards are too prescriptive, you'll see fewer creative ideas in the codebase. If the standards don't embrace new features, libraries, or architectures, your codebase will be stuck in the past. Maybe these are actually effects that you want - perhaps you're maintaining a large COBOL platform, and modernization will halve your paycheck. Fair enough.

For the rest of us, I'd venture that we want to work on exciting, collaborative teams that encourage using new ideas to continuously refine the code. If you've got effective standards that the team is actively using, these standards need to be an example of the kind of culture you want to work in.

By developing the standards themselves collaboratively, this will cause the team to develop more collaboratively. By keeping standards focused on the _why_, PR reviews and pair programming sessions will have a focus on _why_. If your standards allow for variance within the parameters your team must work (business requirements, etc) then your team's solution space will be as wide as possible. If you review and improve your standards at intervals where you can incorporate the latest new features of your language or libraries - for example if you're a .NET team and you review standards every November after each .NET/C# release - your team too will incorporate more of the latest features.

# Conclusion

Are coding standards necessary? No, at least not for everyone. If your team isn't going to use the standards, don't create any. If the team isn't engaged in developing the standards, then you'll probably get subpar standards. Remember, unless you've got effective standards it's better to not have any. If your team is in a pinch where they decide they need to align on coding practice, then having concise standards to document its feelings can be beneficial. Develop the standards and review them constantly from a collaborative perspective, and focus on documenting your feelings with a small number of key examples.

But how do we use these standards once we've written them? Well, curiously, you might not. If you're onboarding a new member of the team or explaining yourselves to another engineer in your organization, you should break out these standards to show off how the team feels. But if your team is really aligned on development practice - as it should be after developing a set of standards - then you and your teammates won't need them, not day-to-day. Standards can't be used as a cudgel to enforce conformity, that creates resentment and disfunction on the team.

Don't take this to mean they're not useful at this point - indeed it's the very process of continuously reviewing this document that keeps your team aligned and allows everyone to update and refresh their ideas on the same page. It's curious - the primary utility of the document is not the document itself or the regular use of the document, but the regular and ongoing development of the document as a team.
