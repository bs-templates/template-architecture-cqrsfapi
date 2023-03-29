# template-architecture-cqrsfapi
Architecture CQRS FullAPI 

### WebAPI
> dotnet build src/BAYSOFT.Presentations.WebAPI
> 
> dotnet run --project src/BAYSOFT.Presentations.WebAPI
> 
> cd src/BAYSOFT.Presentations.WebAPI

#### Migrations

Go to "BAYSOFT.Infrastructures.Data" project folder and open cmd

> dotnet ef --startup-project ../BAYSOFT.Presentations.WebAPI migrations add InitialMigrationDefaultDbContext -c DefaultDbContext -o Default/Migrations