# H360_Helpdesk
Ticketing System
# Angular & ASP.NET Core WebAPI Project

This project is a demonstration of a full-stack web application using Angular as the frontend and ASP.NET Core WebAPI as the backend. It's perfect for understanding the fundamentals of these technologies, including RESTful API design, state management, and best practices for modern web development.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

Before you begin, ensure you have met the following requirements:

- [Node.js](https://nodejs.org/), which can be downloaded and installed from the official website.
- [Angular CLI](https://cli.angular.io/), which can be installed via npm (comes with Node.js) using the command `npm install -g @angular/cli`.
- [.NET Core](https://dotnet.microsoft.com/download), which can be downloaded and installed from the official website.

### Setting Up and Running the Project

Follow these steps to set up and run the project:

```bash
# Clone the repository
git clone https://github.com/VCL-MEA/H360_Helpdesk.git

# Navigate to the repository directory
cd /H360_Helpdesk

# Navigate to the Angular project directory and install dependencies
cd AngularApp
npm install

# Serve the Angular app
ng serve

# Open a new terminal, navigate to the ASP.NET Core project directory
cd /H360_Helpdesk/webapi

# Restore the .NET Core packages and run the project
dotnet restore
dotnet run
Running the Tests
cd AngularApp
ng test

# Running tests in ASP.NET Core
cd path/to/AspNetCoreWebAPI
dotnet test
