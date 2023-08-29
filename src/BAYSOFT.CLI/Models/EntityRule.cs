using BAYSOFT.CLI.Models.Bases;
using Spectre.Console;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace BAYSOFT.CLI.Models
{
    public class EntityRule : Promptable
    {
        public Property Property { get; set; }
        public string Rule { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public Entity Entity { get; set; }
        public EntityRule(Entity entity)
        {
            Entity = entity;
        }

        public override void Prompt()
        {
            AnsiConsole.Clear();

            var selectedOptions = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Entity [blue]{Entity.Name}[/] key options - Choose what to do?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(Entity.Properties.Select(x=>x.Name).ToArray()));

            Property = Entity.Properties.Where(x => selectedOptions.Equals(x.Name)).SingleOrDefault();

            Rule = AnsiConsole.Ask<string>("Enter property rule?");

            Message = AnsiConsole.Ask<string>("Enter property rule message?");
        }

        public override void Generate()
        {
            throw new NotImplementedException();
        }
    }
}
