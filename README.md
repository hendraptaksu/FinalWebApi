# FinalWebApi
FinalWebApi is a REST API project that is built using .NET 6 Framework generated using `dotnet new webapi` command.
This repo contains 2 .NET projects:
- FinalWebApi.API: contains REST API code.
- FinalWebApi.Tests: contains testing code for the FinalWebApi.API project.

## Running Migration
To run migration i.e generating tables, use command `dotnet-ef database update`.

## Run Project
To run the project. Either use available C# IDE (JB Rider, Visual Studio, VS Code). Or using command `dotnet run`. But using command you'll lose debugger capability.

## Run Test
To run testing use command `dotnet test` or use your favorite IDE.

## Seeing available endpoints
Swagger page will automatically launced on the browser. You can use swagger to play around with the endpoint.