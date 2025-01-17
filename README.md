# ChatApp
## Installation:

### SendGrid API Key:
Obtain an API key from SendGrid and enter it in the relevant section of the appsettings.json file.

### Enter Your Mail:
Enter the email address from the API key you obtained from SendGrid into the line ```var from = new EmailAddress("Add Here");``` in the EmailSenderService.cs file of the ChatApp.Front project.

### Database Connection:
Write the required connection string for MSSQL in the SqlServer section of the appsettings.json file.
```
"ConnectionStrings": {
  "SqlServer": "Add Here"
},
```

### Migration:
Open package manager console and write codes.
```
add-migration InitialCreate -OutputDir Persistence/Migrations
update-database
```
