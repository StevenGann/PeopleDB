# PeopleDB
A fuzzy, loosely structured database to supplement my poor memory of people.

## Why?

Anyone who spends more than a few minutes with me will quickly become aware of my greatest weakness. I simply cannot remember most people, or at least not anything useful about them until I have spent a great deal of time with them.
It is so bad, that many times I will forget a person's name before I have even told them mine. Birthdays, likes, dislikes, and other details are almost as hard to retain.

Seeking a solution to my problem, I devised the concept of PeopleDB while still in college. Unfortunately, I ran into limitations with existing database solutions. SQL and similar table-based solutions simply could not be reasonably applied to a database of arbitrary and loosely structured knowledge, since I cannot define specific columns or fields in advance. 

## How?

With my contributions to ListFile (in colaboration with Damen500), I ended up shaping the ideal backing data structure for my loosely structured database needs. For the most basic of functions, PeopleDB needs only to be a light wrapper around ListFile with custom search methods and additional file handling procedures.

At the lowest physical level, every person has their own XML file containing all the information about them, in title:text pairs.

## Status and Plans

PeopleDB is highly experimental. I have no formal training in database design, data mining, or other areas of study that this project stumbles around.
As such, I am learning as I go along and adding features as I learn. The first major lesson was that evaluating the relevence of text is a lot harder than I anticipated. I expected it to be difficult, given that it is the subject of cutting-edge data mining and AI research, but I had assumed a simple search over a small data set would be trivial. It was not.

The main search method, FuzzyFind(), is one of the most important parts of PeopleDB and is the fruit of significant trail and error. I iterate over every entry in the database, assigning a relevence score to every entry. The relevence score of an entry is determined by splitting the search string into tokens, splitting the entry into tokens, looking for tokens in the entry that have a Levenshtein Distance below a certain threshhold to a search string token, and then averaging the Levenshtein Distance between the near-hits to avoid favoring very long entries.

Using Levenshtein Distance is no magic bullet, of course, since completely unrelated words like "Steven" and "Shaved" bring up curious matches. However, since the addition of the near-hit averaging the top one or two results are usually very relevent.

Besides myself, I feel that PeopleDB could benefit many others, especially those suffering from face blindness. Coupled with facial recognition and a HMD like Google Glass, PeopleDB could identify people a user is looking at and has met before, pick keywords from conversation, and scroll relevant details to jog the user's memory.
