using BAYSOFT.CLI.Models.Bases;
using BAYSOFT.CLI.Templates;
using Spectre.Console;
using System.Text.Json.Serialization;

namespace BAYSOFT.CLI.Models
{
    public class Project : Promptable
    {
        private readonly string EDIT = "## Edit project name";
        private readonly string ADD_CONTEXT = "+  Add context";
        private readonly string GENERATE = "$  GENERATE";
        private readonly string BACK = "<- Back";
        [JsonIgnore]
        public List<string> GetMessages
        {
            get
            {

                List<string> messages = new List<string>();
                messages.Add(this.Name);
                messages.AddRange(this.Contexts.Select(x => x.Name).ToList());
                messages.AddRange(this.Contexts.Select(x => x.Name.Pluralize()).ToList());
                messages.AddRange(new List<string> {
                    "'{0}' cannot be empty!",
                    "'{0}' cannot be null!",
                    "'{0}' cannot be 0!",
                    "'{0}' is required!",
                    "'{0}' not found!",
                    "'{3}' must have a maximum of '{1}' characters!",
                    "'{3}' must have at least '{0}' characters!",
                    "Operation failed in domain validation!",
                    "Operation failed in entity validation!",
                    "Operation failed in request validation!",
                    "Successful operation!",
                    "Unsuccessful operation!"
                });
                return messages;
            }
        }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ICollection<Context> Contexts { get; set; }
        public Project()
        {
            Contexts = new List<Context>();
        }

        public override void Prompt()
        {
            AnsiConsole.Clear();

            if (string.IsNullOrWhiteSpace(Name))
            {
                PromptEdit();
            }

            var selectedOption = string.Empty;
            do
            {
                var options = new List<string> { };

                options.Add(EDIT);
                options.AddRange(Contexts.Select(x => x.Name));
                options.Add(ADD_CONTEXT);
                options.Add(GENERATE);
                options.Add(BACK);

                selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Project [blue]{Name}[/] - Choose what to do?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(options.ToArray()));

                if (selectedOption.Equals(EDIT))
                {
                    PromptEdit();
                }

                if (Contexts.Any(context => selectedOption.Equals(context.Name)))
                {
                    var context = Contexts.Where(c => selectedOption.Equals(c.Name)).SingleOrDefault();

                    context.Prompt();
                }

                if (selectedOption.Equals(ADD_CONTEXT))
                {
                    AddContextPrompt();
                }

                if (selectedOption.Equals(GENERATE))
                {
                    Generate();
                }
            } while (!selectedOption.Equals(BACK));
        }

        private void AddContextPrompt()
        {
            var context = new Context(this);

            Contexts.Add(context);

            context.Prompt();
        }

        private void PromptEdit()
        {
            Name = AnsiConsole.Ask<string>("Enter project name?");
            DisplayName = AnsiConsole.Ask<string>("Enter project display name?");
        }

        public override void Generate()
        {
            this.GenerateProjectResourceXML();
            this.GenerateProjectResourceClass();
            this.GenerateAddDbContextConfigurations();
            this.GenerateAddDomainServicesConfigurations();
            this.GenerateAddValidationsConfigurations();
            foreach (var context in Contexts)
            {
                context.Generate();
            }
        }
    }
}
