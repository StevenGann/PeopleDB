# PeopleDB
A fuzzy database to supplement my poor memory of people.

## Why?

Anyone who spends more than a few minutes with me will quickly become aware of my greatest weakness. I simply cannot remember most people, or at least not anything useful about them until I have spent a great deal of time with them.
It is so bad, that many times I will forget a person's name before I have even told them mine. Birthdays, likes, dislikes, and other details are just as hard to retain.

Seeking a solution to my problem, I devised the concept of PeopleDB while still in college. Unfortunately, I ran into limitations with existing database solutions. SQL and similar table-based solutions simply could not be reasonably applied to a database of arbitrary and loosely structured knowledge.

## How?

With my contributions to ListFile (in colaboration with Damen500), I ended up shaping the ideal backing data structure for my loosely structured database needs. For the most basic of functions, PeopleDB needs only to be a light wrapper around ListFile.

At the lowest physical level, every person has their own XML file containing all the information about them.

## Status and Plans

PeopleDB is highly experimental. I have no formal training in database design, data mining, or other areas of study that this project stumbles around.
As such, I am learning as I go along and adding features as I learn.

Besides myself, I feel that PeopleDB could benefit many others, especially those suffering from face blindness. Coupled with facial recognition and a HMD like Google Glass, PeopleDB could identify people a user is looking at and has met before, pick keywords from conversation, and scroll relevant details to jog the user's memory.
