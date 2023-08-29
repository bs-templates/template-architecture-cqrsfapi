using BAYSOFT.CLI.Models;
using System.Text.Json;

// dotnet tool update --global --add-source ./nupkg baysoft.cli

var filePath = Path.Combine("baysoft.json");

Project project = null;

if (File.Exists(filePath))
{
    var fileContent = File.ReadAllText(filePath);
    project = JsonSerializer.Deserialize<Project>(fileContent);
    project.Contexts
        .ToList()
        .ForEach(context => 
        { 
            context.Project = project;
            context.Entities
                .ToList()
                .ForEach(entity =>
                {
                    entity.Context = context;

                    entity.Properties
                        .ToList()
                        .ForEach(property =>
                        {
                            property.Entity = entity;
                        });

                    entity.EntityRules
                        .ToList()
                        .ForEach(entityRule =>
                        {
                            entityRule.Entity = entity;
                            if(entityRule.Property != null)
                            {
                                entityRule.Property.Entity = entity;
                            }
                        });

                    entity.Specifications
                        .ToList()
                        .ForEach(specification =>
                        {
                            specification.Entity = entity;
                        });

                    entity.Services
                        .ToList()
                        .ForEach(service =>
                        {
                            service.Entity = entity;
                            service.Specifications
                                .ToList()
                                .ForEach(specification => { 
                                    specification.Entity = entity;
                                });
                        });

                    entity.Commands
                        .ToList()
                        .ForEach(command =>
                        {
                            command.Entity = entity;
                            if(command.Service != null)
                            {
                                command.Service.Entity = entity;
                                command.Service.Specifications
                                    .ToList()
                                    .ForEach(specification => 
                                    { 
                                        specification.Entity = entity;
                                    });
                            }
                        });
                });
        });
}
else
{
    project = new Project();
}

project.Prompt();

var json = JsonSerializer.Serialize(project);

File.WriteAllText(filePath, json);