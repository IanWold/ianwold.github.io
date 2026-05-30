;;;
{
    "title": "The Ontology Doc",
    "description": "A simpleton-level thought on a better way to map domain ontology.",
    "date": "30 May 2026",
    "contents": false,
    "hero": "photo-1562186854-e28962c2ec93",
    "topics": ["Processes", "Learning", "Standards", "DevEx"],
    "related": [
        { "title": "Building a Documentation Habit", "description": "Documentation is great when it's great and terrible when it's not, but a habit to document, review, and improve is a good habit for a team.", "fileName": "building_a_documentation_habit" },
        { "title": "Clean Meetings: A Software Engineer's Guide", "description": "If being in meetings all day isn't bad enough, spending more time thinking about them seems horrible. Here's a simple guide on making sure you're getting the most out of your meetings.", "fileName": "clean_meetings_a_software_engineers_guide" },
        { "title": "Don't Retro the Same Twice", "description": "Different retrospective formats are mostly the same thing in different flavors - don't argue about them; try all the flavors (at least once)", "fileName": "dont_retro_the_same_twice" }
    ]
}
;;;


You know how you're not supposed to start an essay with a dictionary definition? Just as well that this one starts with a question. Anyway, [Webster's defines _ontology_](https://www.merriam-webster.com/dictionary/ontology) as:

> a particular theory about the nature of being or the kinds of things that have existence

Software engineers dael in ontology every day, but we usually use the word "domain" to refer to this concept. "Domain" has become laden with a lot of pieces though, when we discuss domain we're not necessarily just talking about the _things_ that exist within it. A _domain_ will have an _ontology_ - some opinion about the things and _kinds_ of things in the domain.

We inevitably spend a lot of time talking about the ontology of the domain without explicitly naming it. That's not necessarily a problem, but we end up missing a very powerful tool when we don't consider the _ontology_ as an _ontology_; that is, a proper _theory_ about the _kinds_ of _things_ in the domain. I've seen a lot of "domain glossaries," sometimes presented as a plan SQL schema and/or entity diagram, when I attempt to inquire about the objects of a domain. SQL schemas are a bit dry on the "what purpose does this have" side but do a good job of showing relations, whereas bullet point glossaries have the opposite problem. Both approaches lack an overall vision; what is the limit o fthe space these objects exist in, what principle(s) guide their relations, and what principles guide their existences?

Especially for more complicated domains, it's ideal to have a proper ontology developed: not just a view of the particular objects as they exist, but a theory about what the whole domain is and principles for how objects come to be in it and how relations get made. This is approaching a proper ontology, and can be captured in an Ontology Doc. Now, there's a whole practice of [ontology engineering](https://en.wikipedia.org/wiki/Ontology_engineering) that has methods well in excess of what I'm proposing here. It all seems a bit niche to me, and instead I've charted my own course with how I handle domain ontology. With the caveat that I am very much an outsider to formal ontology engineering, here's what I have come to like in an Ontology Doc:

## 1. What is the domain boundary?

We want to answer what world we're modeling and what we're _not_ modeling. We want to define the boundaries of what definitely is or is not the domain. Why does the system exist? What real-world _thing_ is it modeling? What problem is it solving? Does the system serve some process, relation, or object in the real world?

I work in ecommerce presently, I might say something at the broad level like "the domain models the exchange of goods between economic actors." If I drill down to an individual product page, I'm no longer modeling the exchange but the individual product. Taken as a user relation, the domain might be "the interaction between a purchaser and an individual product." For the broader ecommerce context I might say that I am not modeling accounting or legal practices; for the narrower product page context I might say I am not modeling physical delivery.

Alternately, some project management application might be said to model "commitments made by actors toward outcomes over time," or some healthcare startup app might be said to model "observations, interventions, and responsibilities concerning human health." Domains are quite broad at the top level, but being clear about its proper purpoes and boundaries gives us a context to work in.

## 2. What are the assumptions of the domain?

At a bullet level, what characteristics are we assuming as requirements for the domain? Take an identity system as an example, I might assume:

* Every user possesses one or more identities
* Identities may change
* Historical identity state must remain recoverable

And so on. Almost every team has something like this already documented, just in a location separate to the entities. I see these a lot as a preface to a repository of "feature sets."

## 3. What are the fundamental entity categories?

Not specific entities, but categories. In a larger firm these might frequently map onto teams, or in domain-driven-design they might map onto areas. In ecommerce I might map something like:

* User
  * Customer
  * Admin
* Resource
  * Family
  * Item
  * Set
* Order
  * Purchase
  * Refund

For each category, we want to know why it exists based on the assumptions of the domain. We might say the "user" category fulfils some domain assumption that "customers will be able to view their history of exchanges with us," or that the "order" category fulfils some domain assumption that "economic exchanges will be trackable and reportable". We're focused on explaining why and giving rationality for each step: when some new guy comes on and asks why we have the specific divisions we do, we can point to this reasoning.

## 4. What are the existence rules in these categories?

For each _category_, I want to know what makes objects under them come into existence, what makes them disappear, and what makes them the _same_ object over time? Existence and identity are two sides of the same coin, remember!

An object in the _order_ category comes into existence when the user decides to commit to some ordering action (they want to purchase or refund something, for example). An object in the refund category stops existing when the refund is posted to the user, but we might also say that the object in the refund category will transform into an "archive" version of itself; then we'd say "refund archive" objects come into existence on the completion of refund objects.

We might say a user remains the same user in spite of changes to username, email, phone number, and so on.

By keeping rules at a category-level, we're putting more generic constraints on more braod areas of the system. When we need to fashion specific objects, we can more clearly see the rules across the whole system to see where those specific objects should exist. For example, we might not want "refund archive" objects to be under the "refund" category; instead, we might define "archive" as a separate category fulfilling a different domain assumption, with objects under that category more similar to each other.

## 5. What drives relationships?

Intuitively, we know that users and purchases will have a one-to-many relationship. Instead of bulleting that out, we want to define _why_ that is based on the domain assumptions: "users contain many purchases to fulfil the domain assumption that customers will purchase things from us." Contrived a bit, but I think you get my point there.

This is explicitly stating the "why" of relationships instead of _just_ mapping relationships as they exist. Again, we're wanting to move from our domain assumptions and fundamental principles to create constraints for the system more broadly. When I need to consider creating new object relations, I can better see the _whys_ of what presently exists, and what parts of the system are going to be more accommodating to the updates I need to make. When I do make updates, justifying them along the same lines based on the domain assumptions allows me to slot the updates more neatly into the broader system.

## 6. Now, what specifics exist?

Having defined the broad shape and constraints of the system, it should be pretty simple to derive objects and relationships. What's more though is that pretty much any concrete thing we want to define can come from these principles. For example: state change (lifecycle) modeling becomes easy from the existence rules, and domain events are usually derivations of state change points.

## Altering the Ontology

When we get new information about the domain, this can have a ripple effect down the system. If we're adding assumptions, we need to make sure the new assumptions don't clash with the existing ones, and that derived constraints aren't accidentally impeding new development from satisfying the new constraints. If we're altering previous assumptions, we'll want to revisit all the rules we derived from that assumption. There aren't great software tools I know of that can capture every change and ramification, so the Ontology Doc should be kept relatively general and lean; it should be a tool to be able to double check system-wide alterations, and a learning platform for new folks.