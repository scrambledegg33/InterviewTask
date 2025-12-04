## Task Overview

You will be working with a Fines Management system that consists of a .NET API backend and a React frontend. Your goal is to implement filtering functionality across both layers.

## Requirements

### What You Need to Do

1. **Fork and Clone**
   - Fork this repository to your own GitHub account
   - Clone your fork locally to begin work

2. **Frontend Implementation** (React/TypeScript)
   - Implement filter components/controls (e.g., filter by fine type, date range, customer name, etc.)
   - React to filter changes, make a request to the API and update the UI accordingly

3. **Backend Implementation** (.NET/C#)
   - Implement API endpoint(s) to accept filter parameters
   - Query the Entity Framework repository with the filters
   - Ensure proper response structure and status codes

4. **Submit Your Work**
   - Push your completed solution to your forked repository
   - Ensure all code is committed and the repository is public (or accessible to reviewers)

## Your Task
Extend or modify the API and frontend, to support filtering based on the below critera:

### Filters

1. Fine Date
2. Fine Type
3. Vehicle Registration

### Bonus Points

If you have time, feel free to add one or two small improvements that you think are worthwhile. 

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or later)
- [Git](https://git-scm.com/)
- Code editor ([Visual Studio/Visual Studio Community](https://visualstudio.microsoft.com/vs/) or [VS Code](https://code.visualstudio.com/))

### Running the API/Front End

Each application has a respective readme, Please see the Fines.Api and Fines.Client folders. 

## Technologies Used

### Backend
- **[ASP.NET Core 8.0](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)** - Web API framework
- **[Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)** - ORM and data access
- **[In-Memory Database](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/)** - For development/testing
- **[Swashbuckle](https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger)** - API documentation (Swagger)
- **[xUnit](https://xunit.net/)** - Testing framework

### Frontend
- **[Mantine Documentation](https://mantine.dev/)**
- **[Vite Documentation](https://vitejs.dev/)**
- **[React Documentation](https://react.dev/)**

## Submission

Once you've completed the task:

1. Ensure all your changes are committed

2. Push to your forked repository

3. Share the link to your repository with the interviewer

## Questions?

If you have any questions about the task requirements or encounter issues with the setup, please reach out to your recruitment contact.