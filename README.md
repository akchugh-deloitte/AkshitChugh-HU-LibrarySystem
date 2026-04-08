Akshit Chugh – Assessment B (Debug & Refactor) 
 
Bugs Identification 

LibraryService contructor -> parameter not assigned to field _context 

IssueBook method -> missing db save 

Book Entity -> need to track book status, added IsAvailable 

BookIssue Entity > we need user and book mapping to be able to know which book was issued to whom 

GetBookFromCache -> incorrect logic, it doesnt check key exist and return 

Static cache -> never clears, leakage of memory 

Book available -> need transaction to avoid db corruption or race condition 

Solid Principles Applied  

Single Responsibility -> One service handles all the related to that domain 

Open Closed -> Added interfaces, made classes extendable reusable 

Liskov substn -> all dependencies via interface 

Interface segregation -> Seperated IBookService and BookService 

Dependency inversion -> DI container in program.cs 

New architecture -> 

Library.API -> Controllers (LibraryController (writes), BookController (reads)) 

Services -> Business logic (Book Service), Cache service, Logging Service 

Services for Reading via Dapper queries 

Libarary.Core -> Entities (Book, BookIssue), Interfaces 

Library.Infra -> Data layer repository 

Memory Leak fixing  

It had static dictionary, never clears for app lifetime. No expiration, no size limit. 

Used IMemoryCache with constraints 

Transaction implementation for handling race conditions while writing or updating record. 

Added rate limits, api key handling and correlation id for api gateway in program.cs 
