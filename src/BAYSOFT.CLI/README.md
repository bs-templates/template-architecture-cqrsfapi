# BAYSOFT.CLI ![Nuget](https://img.shields.io/nuget/v/BAYSOFT.CLI)

´´´dotnet tool install --global --add-source ./nupkg baysoft.cli

´´´dotnet tool update --global --add-source ./nupkg baysoft.cli

Aplication
	Commands								- OK
	Queries									- OK
	Notifications							- OK

Domain
	Context
		Interfaces
			Infrastructures
				Data
					IDefaultDbContextReader	- OK
					IDefaultDbContextWriter	- OK
		Resources							- OK
		Collection
			Entity							- OK
			Resources						- OK
			Services						- OK
			Specifications					- OK
			DomainValidations				- OK
			EntityValidations				- OK
	Resources								- OK

Infrastructures
	Data
		Context
			EntityMappings					- OK
			Migrations						
				InitialMigrationDbContext	- OK
				DbContextModelSnapshot		- OK
			DefaultDbContext				- OK
			DefaultDbContextReader			- OK
			DefaultDbContextWriter			- OK
Middleware
	AddServices
		AddDbContextConfigurations			- OK
		AddDomainServicesConfigurations		- OK
		AddValidationsConfigurations		- OK

Presentations
	Resources
		Controllers							- OK
	React
		Organisms
			Form							- OK
			Table							- OK
			Tab								- OK (Refatorar: Panels estão apontando para a entidade agregadora e não os agregados
		Pages
			index							- OK
			Index							- OK
			Create							- OK
			Edit							- OK

TODO: Criar arquivos dos projetos ou renomea-los.