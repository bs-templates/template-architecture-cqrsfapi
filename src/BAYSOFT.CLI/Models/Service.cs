using BAYSOFT.CLI.Models.Bases;
using BAYSOFT.CLI.Templates;
using Spectre.Console;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace BAYSOFT.CLI.Models
{
    public class Service : Promptable
    {
        public string Name { get; set; }
        [JsonIgnore]
        public Entity Entity { get; set; }
        public ICollection<Specification> Specifications { get; set; }
        public Service(Entity entity)
        {
            Entity = entity;
            Specifications = new List<Specification>();
        }

        public override void Prompt()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine($"[blue]{Entity.Name}[/]");

            Name = AnsiConsole.Ask<string>("Enter service name?");

            var selectedOptions = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>()
                        .Title($"Service [blue]{Name}[/] key options - Choose what to do?")
                        .PageSize(10)
                        .Required(false)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(Entity.Specifications.Select(x=>x.Name).ToArray()));

            Entity.Specifications
                .Where(specification => selectedOptions.Contains(specification.Name))
                .ToList()
                .ForEach(specification => {
                    Specifications.Add(specification);
                });
        }

        public override void Generate()
        {
            this.GenerateSpecificationsValidatorFile();
            this.GenerateServiceRequestFile();
            this.GenerateServiceRequestHandlerFile();
        }
    }
}
