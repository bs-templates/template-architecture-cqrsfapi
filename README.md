# template-architecture-cqrsfapi
Architecture CQRS FullAPI 

cd src/BAYSOFT.Presentations.WebAPI

dotnet ef migrations add InitialDefaultDb -c DefaultDbContext -o Migrations

### WebAPI
> dotnet build src/BAYSOFT.Presentations.WebAPI
> 
> dotnet run --project src/BAYSOFT.Presentations.WebAPI
> 
> cd src/BAYSOFT.Presentations.WebAPI