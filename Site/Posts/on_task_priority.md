;;;
{
    "title": "On Task Priority",
    "description": "Some thoughts on assigning priority to our tasks.",
    "date": "3 March 2024",
    "contents": false,
    "hero": "photo-1623045832976-664b045bfea3",
    "related": [
        { "title": "Reclaim Your Agile: The One Clever Trick Agile Coaches Don't Want You to Know", "description": "What if I told you there's one trick to being able to reshape your team's development process without your company knowing it? What if I told you that you can achieve actual Agile even though you work in a Scrum firm?", "fileName": "reclaim_your_agile" },
        { "title": "Clean Meetings: A Software Engineer's Guide", "description": "If being in meetings all day isn't bad enough, spending more time thinking about them seems horrible. Here's a simple guide on making sure you're getting the most out of your meetings.", "fileName": "clean_meetings_a_software_engineers_guide" },
        { "title": "Develop Effective Coding Standards", "description": "Bad coding standards are worse than no standards, and even good standards are sometimes unnecessary. What's the utility in coding standards, and what makes a good one?", "fileName": "develop_effective_coding_standards" },
    ]
}
;;;

I don't know of a lot of successful software projects that don't have some sort of list of tasks defined. Unless you've found some magical, outlying case, we need to define our work before we can do it. This usually takes the form of a kanban board where tasks are "cards", or at the very least tasks are defined in rows in a spreadsheet.

Naturally, these tasks have priority. This isn't just useful so that I know which task to pick up next when I'm done with my current one - we usually have milestones or objectives we need to meet in our development and organizing that gives priority to the tasks. Tasks and priorities are absolutely necessary, so we slap a tshirt size on our cards, keep them ordered with higher priority at the top of the list, and consistently reevaluate priorities to keep everyone aligned, right? Right?

The trouble, as with most things in software engineering, comes from the fact that this field is filled with software engineers. Maybe one day we'll be able to do something about that root cause, but until then we'll have to deal with it. In this case it's that we software engineers can't help but organize, systematize, and insert-your-word-here-ize every problem we find. We do tend to over-engineer, and boy is task priority the sort of thing we tend to over-engineer. I want to take a look here at the best practices.

# Maintain Alignment

The priority of a task is directly tied to the definition of the task and the goals of the project, and none of the variables are static. They change by the day, and sometimes by the hour. Each of these three are essential for an entire team to understand about a project, and everyone needs to be on the same page. This is the most important thing to recognize about task priority.

## Reevaluate Often

Every single time we meet with stakeholders, the project changes. Sometimes dramatically. This is a feature, not a bug; we want to have the tightest loop we can between delivering value and incorporating feedback. This feedback always changes priorities. Features get reconsidered, bugs get discovered, milestones move.

The ramification is that priorities change to adapt to this, and as long as priorities are being documented they need to be revisited and revised when these parameters change. This is an opportunity to make sure the stakeholders and the team are aligned. Ask questions in terms of priority, and document and clarify _why_ tasks have the priority they do.

## Organize Tasks by Priority

This is as simple as keeping them at the top of whatever shared list the team has. It seems obvious to read, but it's often overlooked. How many times have we found an errant "high" priority task that's been at the bottom of a backlog for twenty sprints?

Reorganization should be part of reevaluation. Often times tasks will be assigned one of a small number of priority options; in this case there are several tasks with "high", "medium", or whichever priority labels you have. This doesn't tell the whole story though - the team might have different goals this week or sprint. Maybe some milestone is more pressing and its related tasks should be considered first. These fine-grained priority assignments are best encoded and documented by keeping the task list in order of priority.

# Assign Meaningful Priority

Priority has meaning to everyone - the stakeholders, team members, and even users sometimes. All of these people need to be kept in alignment about priority, and so some kind of common language is needed. The priority needs to be meaningful to everyone involved, and the meaning needs to be the same for everyone across the board. This is extremely difficult to achieve since very few of us have developed adequate skills in telepathy.

The most important thing is to keep priority limited to a very small number of generic words. 3-5 is ideal. If you're able to get away with just 2, that's spectacular. Consider that each additional priority category _doubles_ the chance for confusion for each individual that needs to be aligned on priority. If you have 15 priority categories ranging from "no priority" to "EVERYONE SWARM NOW" then nobody is going to understand anything, math being what it is (trust me, I ran the numbers).

## "Highest" and "Lowest" Have no Meaning

Taking the common tshirt-size priority list of "high", "medium", and "low", it's quite common to want to tack on "highest" for a catastrophic situation and "lowest" for a backburner want-to-have. I understand the inclination, but there really is no meaning here.

For "lowest" priority items, why even have it on your set of tasks? if this is something that your team _is_ going to do, then it should be understood in the context of the whole project and have a proper priority. If it's a wishful thought or a pipedream of one of the engineers (what this category is typically used for) then it's not a part of the project and it's wasting valuable space. Get rid of it.

Similarly, if I really get an incident that requires immediate intervention, I'm not going to create a "highest" priority card for it. I'm getting everyone on a Zoom call, starting an incident document, and fixing the issue. "Highest" priority cards lag around because some individual needed to be placated that we're definitely considering this one as important. This is a misalignment, and it should be addressed as such. Everyone should have the same idea as to what we're marching towards.

## Don't use "Medium"

No, really! Just like "highest" and "lowest", this one ends up having no meaning. Using tshirt sizes, "medium" becomes the priority for 90% of all tasks - when someone creates a task they don't want to upset the boat by choosing "high", but they definitely want it done so they're not going to select "low". Instead, they use "medium" as the default "pffft, don't know don't care" option, tell the PM, then the PM doesn't reassign priority because they don't understand the full scope of why refactoring the `IDoSomething` interface is really needed at this stage.

A "medium" priority becomes a dumping ground for all tasks and leads to a situation where the _true_ priority never gets documented because everything is "medium". This is a major source for misalignment acorss the entire team. Instead, my preferred set of priorities is "high", "higher", and "lower". This forces us to consider whether it's more or less important, and recognizes that most tasks on the board are there because they have some heightened priority. For ideas or far-off things, I would suggest keeping a separate, non-prioritized list for documentation purposes.

![Assigning Priority](https://raw.githubusercontent.com/IanWold/ianwold.github.io/master/Static/images/priority.png)

# Beyond Priority

Priority is, for most of our projects, a simplified capture of many different complicated factors about our tasks - the perceived need of different groups of stakeholders, the hopes and wants of the development team, and the unkowns of new technologies or complicated use cases. In some projects however, things are more clear-cut. There might only be a single, contiguous group of users, or the focus might be so constrained as to be relatively obvious to all the parties involved.

In these cases it could be that priority is actually too ambiguous, and measuring other factors might serve us better in terms of understanding task order and importance.

## Impact

If the project as a whole is addressing a specific use case or need, it might be useful to measure tasks in terms of *customer impact* and *business impact*. You can use the modified tshirt sizes of "high", "higher", and "lower", or you can just use "high" and "low" for each. By focusing on the different aspects of priority, this can greatly simplify the priority assigning process and make the documentation more meaningful.

The types of impact can be varied depending on the project type as well - if the project is a heavy refactor then *team impact* or *codebase impact* might be good inclusions.

## Necessity

When the project is narrowly defined but has several different and/or disparate groups of stakeholders, defining the priority of a task instead by its necessity for different kinds of stakeholders can be the best option. Again, the modified tshirt sizes or just "high" and "low" can be used here.

The broader idea is to think about what kinds of divisions the definition of _your_ project has, and to consider breaking priority out along those lines. By making "priority" a more concrete concept, it will be easier to assign and reevaluate priority, and it will be more meaningful for everyone who needs to be aligned on it.
