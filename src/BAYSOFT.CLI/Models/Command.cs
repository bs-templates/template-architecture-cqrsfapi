using BAYSOFT.CLI.Models.Bases;
using BAYSOFT.CLI.Templates;
using Spectre.Console;
using System.Text.Json.Serialization;

namespace BAYSOFT.CLI.Models
{
    public class Command : Promptable
    {
        private readonly string EDIT = "## Edit entity name";
        private readonly string PROMPT_VALIDATION_RULES = "-- Validation rules";
        private readonly string BACK = "<- Back";
        public string Name { get; set; }
        public string HttpMethod { get; set; }
        [JsonIgnore]
        public Entity Entity { get; set; }
        public Service Service { get; set; }
        public ICollection<EntityRule> ValidationRules { get; set; }
        public Command(Entity entity)
        {
            Entity = entity;
            ValidationRules = new List<EntityRule>();
        }

        public override void Prompt()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine($"[blue]{Entity.Name}[/]");

            if (string.IsNullOrWhiteSpace(Name))
            {
                PromptEdit();
            }

            var selectedOption = string.Empty;
            do
            {
                var options = new List<string> { };

                options.Add(EDIT);
                options.Add(PROMPT_VALIDATION_RULES);
                options.Add(BACK);

                selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Entity [blue]{Name}[/] - Choose what to do?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(options.ToArray()));

                if (selectedOption.Equals(EDIT))
                {
                    PromptEdit();
                }

                if (selectedOption.Equals(PROMPT_VALIDATION_RULES))
                {
                    PromptValidationRules();
                }
            } while (!selectedOption.Equals(BACK));
        }
        private void PromptEdit()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine($"[blue]{Entity.Name}[/]");

            Name = AnsiConsole.Ask<string>("Enter command name?");


            var httpOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Entity [blue]{Name}[/] - Choose what to do?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(new string[] { "Post", "Put", "Patch", "Delete" }));

            HttpMethod = httpOption;

            var selectedOptions = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Command [blue]{Name}[/] key options - Choose what to do?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(Entity.Services.Select(x => x.Name).ToArray()));

            Service = Entity.Services
                .Where(service => selectedOptions.Equals(service.Name))
                .Single();
        }
        private void PromptValidationRules()
        {
            var selectedOption = new List<string> { };
            var options = new List<string> { };

            options.AddRange(Entity.EntityRules.Select(x => x.GenerateRule()));

            selectedOption = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title($"Entity [blue]{Name}[/] - Choose what to do?")
                    .PageSize(10)
                    .Required(false)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(options.ToArray()));

            ValidationRules = Entity.EntityRules.Where(rule => selectedOption.Contains(rule.GenerateRule())).ToList();
        }

        public override void Generate()
        {
            this.GenerateCommandFile();
            this.GenerateCommandResponseFile();
            this.GenerateCommandHandlerFile();

            this.GenerateNotificationFile();
            this.GenerateNotificationHandlerFile();
        }
    }
}
