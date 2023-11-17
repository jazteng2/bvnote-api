# bvnote-api
As of now, a RestAPI for requesting resources from a database.
It is for storing and retrieving centralized related bible resources.
This is the main api, see below for system design. 
## Status
In development

# Simple system design of the bvnote system
![image](https://github.com/jazteng2/bvnote-api/assets/36156694/4e76e235-4df6-45a3-97f0-d708d88e5c82)

# DB Schema (Not Finished)
![image](https://github.com/jazteng2/bvnote-api/assets/36156694/d722f39d-ddb3-4e89-ba8e-2cb175c22626)

# Not Implemented Yet
- filtering
- pagination

# API Endpoints
Endpoints are group based on database schema sections and the type of resources your are fetching. In each section, there will be a group of endpoints grouped by order of (HTTP Methods, Functionality/Relations, didn't think this far).

## Choice of Endpoints
Likelyhood that it will be used and is practical of course. Logging may be implemented to check the usage of endpoints to check if there is a need to throw some endpoints in the bin.

## Version
/api/v1

## Available
### Bible Section
- (GET) /books
- (GET) /books/{bookId}
- (GET) /books/{bookId}/verses?chapterNo={param}
- (GET) /books/verses?abbrev={param}&chapterNo={param}

## Unavailable
### TextEditor Section
- (GET, SearchDoc) /documents/{id}
- (GET, SearchInAll) /documents?find={param}
- (GET, SearchInOne) /documents/{id}?find={param}
- (GET, ListByVerseId) /verses/{id}/documents
- (GET, FirstByVerseId) /verses/{id}/documents/{id}
- (GET, ListByVideoId) /videos/{id}/documents/{id}
- (GET, FirstByVideoId) /videos/{id}/documents/{id}


# Tools & Resources
- Minimal API
- EF-Core
- MariaDB
- Bible data self-create :(
