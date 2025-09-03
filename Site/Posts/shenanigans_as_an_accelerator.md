;;;
{
	"title": "Shenanigans as an Accelerator",
	"description": "A short story about Wordle and scripts",
	"date": "3 September 2025",
	"contents": false,
	"hero": "photo-1502086223501-7ea6ecd79368",
    "topics": ["Processes"],
    "related": [
		{ "title": "Principles for Successful Teams", "description": "We don't need the thousands of several-hundred-page books to understand what makes a team, department, or firm successful.", "fileName": "principles_for_successful_teams" },
		{ "title": "The Best Business Brunch in Minneapolis (So Far)", "description": "Keeping in touch with colleagues is best done over a Business Brunch™; here's where I go in Minneapolis.", "fileName": "best_business_brunch_spots_in_minneapolis" },
		{ "title": "Don't Retro the Same Twice", "description": "Different retrospective formats are mostly the same thing in different flavors - don't argue about them; try all the flavors (at least once)", "fileName": "dont_retro_the_same_twice" }
    ]
}
;;;

If you're a fan of [Wordle](https://www.nytimes.com/games/wordle/index.html)-ing with your work colleagues, you've probably run into a similar problem as I have. The NYTimes app has a feature to compete with friends on Wordle, but you'd (presumably) be signing into the NYTimes app with a personal account. Or, not everyone has the app, or a NYTimes account. Both of these are barriers to friendly office Wordling.

My company uses Slack, which makes setting up workflows very easy. I presume Teams does as well. Crucially, Slack support two actions out of the box: it can paste form submissions into a Google Sheet, and it can set up an endpoint to ingest and parse any JSON string. On the Google side, their [Apps Scripts](https://workspace.google.com/intl/en_uk/products/apps-script/) make interacting with Sheets quite easy, and allows sending HTTP requests. Creating a Wordle challenge Slack channel was all to obvious then: I have a form workflow in which colleagues paste Wordle results that get saved to the spreadsheet, then each morning at 10 an Apps Script posts the results and winners to the Slack channel and clears out that day's submissions.

If you're already familiar with Slack and/or Apps Sctipts then that's nothing but obvious. Maybe a bit annoying to have to read spelled out. However, this ended up being different for me and my colleagues, as we sorted into a cohort defined by two facts: We did not know all that much about Slack workflows and Apps Scripts; and we have a number of scheduley-like things that could greatly benefit from such an automation.

Setting up automations for common scheduley/reporting _things_ can be a huge accelerator for any team. Teams develop processes to make their work flow smoothly, and while those processes evolve over time they generally settle into a static state, with someone on the team tasked with upholding them. Where these processes involve maintaining lists, updating work items, or other manual interventions, would I rather that the process maintainer spend some hours per week performing this work themselves or some (much fewer) hours per year maintaining automations to resolve this work? As with any technological solution, poorly created or architected automations can end up requiring burdensome maintenance, but I want to focus here on the positive result of well-set-up, simple automations with common tools.

What could be more common (for my firm) than Slack and Sheets, I do not know. I do know a couple other things though. For one, having ended up using Slack and Sheets to set up common automations, my team is much better off. One more important thing I know is that, many times, that work done for _work_ is rigid (maybe _rote_), can tend towards over-engineering, but above all is typically non-exploratory. Maybe I'm getting a bit ahead of myself in my own blog post - how do those two relate?

Well, it wasn't the case that we figured out how to set up the Wordle channel after having tasked someone with learning how to set up automations with Slack and Sheets. Rahter, it was the other way around: the automations we developed were a result of having set up the Wordle channel. In fact, we didn't even think of some of the automations we might do until possibilities were revealed to us by the Wordle channel. And, I assert this is the only direction we could have done this work - we could not have assigned out a task to develop the automations.

Only by making something engaging we were able to stimulate the motivation to explore. Only by stepping outside - clearly outside - the boundaries of day-to-day work we were able to get a different perspective on problems, or what problems existed that needed solving. Easy solving, too.

A Wordle competition Slack channel is surely some shenanigans, and thsoe shenanigans have a tangible, positive result on our work. Some might say that's work that was distracted from by play, but there's no denying the benefits realized. I'm resolving to shenanigan again, and I would encourage the same for you!
