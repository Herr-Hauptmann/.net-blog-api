# Blog API
A simple API made with .NET 6

## Prerequisites
Programs you will need in order to run this solution include:
- Visual Studio
- Microsoft SQL Server Express
- SQL Server Mannagement Studio (SSMS)
- Git
## Install instructions
1. Clone the repository via git
2. Open the solution file (Blog.sln)
3. Check if all NuGet packages are installed for both projects (Right click on the project -> Manage NuGet packeges...) and check if all of the packages from the [list](##packages). are there, if not, install them
4. Open terminal in Visual Studio (CTRL + ` or search for Terminal)
5. Install .NET EF globally with ```dotnet tool install --global dotnet-ef```
6. Change directory to rubicon-blog project ```cd ./rubicon-blog```
7. Migrate the database with ```dotnet ef database update```
8. Run the project

NOTE: Database seeding is done automatically when there are no posts in your database

## Packages
### Blog
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer
- Slugify.Core
- Swashbuckle.AspNetCore

### BlogTests
- FluentAssertions
- Microsoft.NET.Test.Sdk
- Moq
- xunit
- xunit.runner.visualstudio
- coverlet.collector
