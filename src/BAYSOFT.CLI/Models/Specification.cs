using BAYSOFT.CLI.Models.Bases;
using BAYSOFT.CLI.Templates;
using Spectre.Console;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace BAYSOFT.CLI.Models
{
    public class Specification : Promptable
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public string Rule { get; set; }
        [JsonIgnore]
        public Entity Entity { get; set; }
        public Specification(Entity entity)
        {
            Entity = entity;
        }

        public override void Prompt()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine($"[blue]{Entity.Name}[/]");

            Name = AnsiConsole.Ask<string>("Enter specification name?");

            Message = AnsiConsole.Ask<string>("Enter specification message?");

            Rule = AnsiConsole.Ask<string>("Enter specification rule?");
        }

        public override void Generate()
        {
            this.GenerateSpecificationFile();
        }
    }
}
