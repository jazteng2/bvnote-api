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
- /api/v1/books/abbreviations (GET)

# Tools & Resources
- Minimal API
- EF-Core
- MariaDB
- Bible data from HolyBooks
