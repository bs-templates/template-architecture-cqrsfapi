using BAYSOFT.CLI.Models.Bases;
using Spectre.Console;
using System.Text.Json.Serialization;

namespace BAYSOFT.CLI.Models
{
    public class Property : Promptable
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public string DbName { get; set; }
        public string DbType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsDbGenerated { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public string RelatedEntityName { get; set; }
        [JsonIgnore]
        public Entity RelatedEntity { get { return Entity.Context.Entities.Where(e => e.Name == RelatedEntityName).SingleOrDefault(); } }
        [JsonIgnore]
        public Entity Entity { get; set; }
        public Property(Entity entity)
        {
            Entity = entity;
        }

        public override void Prompt()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine($"[blue]{Entity.Name}[/]");

            Name = AnsiConsole.Ask<string>("Enter property name?");
            
            DisplayName = AnsiConsole.Ask<string>("Enter property display name?");

            Type = AnsiConsole.Ask<string>("Enter property type?");

            DbName = AnsiConsole.Ask<string>("Enter BD column name?");

            DbType = AnsiConsole.Ask<string>("Enter DB column type?");

            var selectedOptions = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>()
                        .Title($"Property [blue]{Name}[/] key options - Choose what to do?")
                        .PageSize(10)
                        .Required(false)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(new string[] { "Nullable", "Database generated", "Primary Key", "Foreign Key" }));

            IsNullable = selectedOptions.Contains("Nullable");
            IsDbGenerated = selectedOptions.Contains("Database generated");
            IsPrimaryKey = selectedOptions.Contains("Primary Key");
            IsForeignKey = selectedOptions.Contains("Foreign Key");

            if (IsForeignKey)
            {
                var selectedRelatedEntity= AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title($"Property [blue]{Name}[/] key options - Choose what to do?")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                            .AddChoices(Entity.Context.Entities.Select(x=>x.Name)));

                RelatedEntityName = selectedRelatedEntity;
            }
        }

        public override void Generate()
        {
            throw new NotImplementedException();
        }
    }
}
