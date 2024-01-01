# ASP.NET Core Web App

This basic ASP.NET Core Web App is test for a software web developer position in IEA

## Getting Started

To run the application locally please follow this steps.

## Info

- this project was developed with .net 8.0
## 1/ Fast Run Using Bash Commands:
### Create a new folder to host this project and getting into it 
```bash
 mkdir Baha_assignment
```
```bash
 cd Baha_assignment
```
### Clone the Repository
```bash
 git clone https://github.com/bahagh/MiniProjetBooks.git
```
```bash
 cd MiniProjetBooks\MiniProjetBooks
```
### Restore Dependencies
```bash
 dotnet restore
```
### Build the Project
```bash
 dotnet build
```
### Run the Application
running this command below will run the project on: http://localhost:5269/ , check "http://localhost:5269/swagger/index.html" to consult the backend :)
```bash
 dotnet run
```
## 2/ Run from Visual Studio:
- right-click on the solution (WebApplication2) from the solution explorer 
- Rebuild Solution
- Debug > Run (the project is configured to listen on "https://localhost:7264/" for https, and "http://localhost:5269/" for http  , and it will listen on the port 5269 also when the app is runned with the command 'dotnet run' , swagger is configured to test the api's it is possible to be direct it to it via this urls "http://localhost:5269/swagger/index.html" and "http://localhost:7264/swagger/index.html" for http and https respectively")
## 3/ Executing Tests:
- Go to the solution explorer bar right-click on the Tests project under the solution (WebApplication2/Tests)
- Click on Execute tests
- Go to View > Tests Explorer to view their status or behaviour 
