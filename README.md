# bvnote-api
Basic (maybe REST) WebAPI for requesting resources from a MysqlDatabase.
It is for storing and retrieving bible resources from user input.
This is the main api for the web and mobile client applications in the future. 

# Status
In development

# Available Endpoints
- /api/v1/books (GET)
- /api/v1/books/{bookId} (GET)
- /api/v1/books/{bookId}/verses (GET)
- /api/v1/books/verses?abbrev={param}&chapterNo={param} (GET)

# Tools & Resources
- Minimal API
- EF-Core
- MariaDB
- Bible data from HolyBooks

# Simple system design of the bvnote system
![image](https://github.com/jazteng2/bvnote-api/assets/36156694/4e76e235-4df6-45a3-97f0-d708d88e5c82)
