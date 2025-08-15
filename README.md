Notes Application

A secure, full-featured note management application built with ASP.NET MVC, featuring JWT authentication and responsive design.
Project Overview
The Notes Application allows users to create, read, update, and delete personal notes in a secure environment. Built as a technical examination project for Thurston by Joshua Laude, it demonstrates proficiency in ASP.NET MVC, Entity Framework, JWT authentication, and responsive design.

Features

- **User Authentication**
	- Secure JWT-based authentication
	- User registration and login
	- Session management with token-based security
- **Note Management**
	- Create, read, update, and delete personal notes
	- Advanced filtering and sorting options
	- Responsive card-based interface
- **Search & Filter**
	- Filter by title and content
	- Sort by creation or update date
	- Ascending/descending order options

Technologies Used
| Technology | Version/Description |
|------------|---------------------|
| .NET Framework | 4.8 |
| ASP.NET MVC | 5.x |
| Entity Framework | 6.x (Code First) |
| JWT Authentication | System.IdentityModel.Tokens.Jwt |
| Bootstrap | 4.x |
| jQuery | 3.7.0 |
| MS SQL Server | Database backend |
| Unity | Dependency Injection |

Database Schema


**Users Table**
- Id - Guid (Primary Key)
- Username - nvarchar(50) (Unique)
- Email - nvarchar(100) (Unique)
- PasswordHash - nvarchar(256)
- CreatedAt - datetime

**Notes Table**
- Id - Guid (Primary Key)
- Title - nvarchar(200) (Required)
- Content - nvarchar(max)
- CreatedAt - datetime
- UpdatedAt - datetime (Nullable)
- UserId - Guid (Foreign Key)

git clone https://github.com/Josh070301/NotesApplication.git

## Setup and Installation

1. **Prerequisites**
	- Visual Studio 2022 (or compatible)
	- .NET Framework 4.8
	- MS SQL Server

2. **Clone the Repository**
	```sh
	git clone https://github.com/Josh070301/NotesApplication.git
	```

3. **Database Setup**
	- Update connection string in Web.config
	- The application uses Code First migrations
	- Database will initialize with seed data automatically
	- Modify dotenv to use your server

4. **NuGet Packages**
	The following packages are required:
	- Install-Package EntityFramework
	- Install-Package System.IdentityModel.Tokens.Jwt
	- Install-Package Microsoft.Owin.Host.SystemWeb
	- Install-Package Microsoft.Owin.Security.Jwt
	- Install-Package Microsoft.Owin.Security.OAuth
	- Install-Package Unity
	- Install-Package Unity.Mvc5
	- Install-Package Newtonsoft.Json
	- Install-Package DotNetEnv

5. **Build and Run**
	- Build the solution
	- Run the application using IIS Express


## Future Implementations
- Data Insights: Visual dashboards showing note creation patterns
- Analytics: Statistics on note length, frequency, and topic distribution
- Content Analysis: Automatic spell checking and readability scoring
- Tagging System: Organization with custom tags and color coding
- Sharing Options: Secure note sharing capabilities
- Reminder System: Due dates and notifications for action-oriented notes


## Project Development
- IDE: Visual Studio Community 2022
- Database: MS SQL Server Management Studio
- Project Template: ASP.NET Web Application (.NET Framework) - MVC
- Authentication: Custom JWT implementation