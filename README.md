# bvnote-api
As of now, a RestAPI for requesting resources from a database.
It is for storing and retrieving bible resources from user input.
This is the main api, see below for system design. 

# Status
In development

# Available Endpoints
- (GET) /api/v1/books
- (GET) /api/v1/books/{bookId}
- (GET) /api/v1/books/{bookId}/verses?chapterNo={param}
- (GET) /api/v1/books/verses?abbrev={param}&chapterNo={param}

# Tools & Resources
- Minimal API
- EF-Core
- MariaDB
- Bible data from HolyBooks

# Simple system design of the bvnote system
![image](https://github.com/jazteng2/bvnote-api/assets/36156694/4e76e235-4df6-45a3-97f0-d708d88e5c82)
