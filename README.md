# demo-service
Starting a service from scratch to document what I do

# Migrations

Create migration:
```sh
dotnet ef migrations add <MIGRATION NAME> --verbose --project .\Reapit.Services.Demo.Data\ --startup-project .\Reapit.Services.Demo.Api\ -o .\Context\Migrations\
```

Apply migration:
Windows
```cmd
dotnet ef database update --verbose --project .\Reapit.Services.Demo.Data\ --startup-project .\Reapit.Services.Demo.Api\
```
Mac
```sh
dotnet ef database update --verbose --project Reapit.Services.Demo.Data/Reapit.Services.Demo.Data.csproj --startup-project Reapit.Services.Demo.Api/Reapit.Services.Demo.Api.csproj     
```
