# CH10 - Working with Data Using Entity Framework Core (PDF 478 - 532)

***   
- EF Core 7 targets .NET 6 or later. This means that you can use all the new features of EF 
Core 7 with either .NET 6 or .NET 7

- When deleting entities You can remove individual entities with the Remove method. RemoveRange is more efficient when you 
want to delete multiple entities.

- UPDATE/DELETE - EF Core 7 introduces two methods that can make updates and deletes more efficient because they 
do not require entities to be loaded into memory and have their changes tracked. The methods are 
named ExecuteDelete and ExecuteUpdate (and their Async equivalents). They are called on a LINQ 
query and affect the entities in the query result, although the query is not used to retrieve entities so 
no entities are loaded into the data context