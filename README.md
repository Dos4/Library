## Name
Library

## Description
The database was created code-first on the entity framework core. It adds information about books from a file 
to the database and has the ability to sort by filter.

## Usage
It is a flexible database that can be used as a basis for other projects and continue the logic of the library. 
The program creates a local database based on the code-first principle and a ready-made pattern for filtering and 
searching by the main parameters of a book. Another feature of this program is that it does not need a separate 
parameter in the repository that is an IQueriable, because the repository itself can be an IQueriable, so a 
database query can be written directly to the repository through the unit of work.

## Support
Email: dos4vidanie@gmail.com
Telegram: @BuenasNo4es

## Roadmap
The prospect of this program is the transfer of the database from the local to the network, as well as a 
user-friendly interface and visuals.

## Contributing
 When accepting data from a file, the program first compares this data with the data in the database to 
avoid duplicates, and if there are any, the file is rejected. After that, the program maps the data to the 
BookFileModel template to create a ready-made entity, converts centuries to years, if any, and then adds a 
new ready-made Book entity to the list, eliminating duplicates in the file itself. After the books have been 
added to the database, a filter is triggered that takes the values for comparison from the json file and selects 
only those books that fit all the specified criteria. The names of these books are displayed on the console, after 
which a file of the same extension as the input file is created, and all information about the books that fall into 
the filtering categories is added to it.

## Authors and acknowledgment
Nikita Shynkar. Foxminded student


## Project status
In progress
