using BAYSOFT.CLI.Models.Bases;
using BAYSOFT.CLI.Templates;
using Spectre.Console;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace BAYSOFT.CLI.Models
{
    public class Context : Promptable
    {
        private readonly string EDIT = "## Edit context name";
        private readonly string ADD_ENTITY = "+  Add entity";
        private readonly string BACK = "<- Back";
        [JsonIgnore]
        public List<string> GetMessages
        {
            get
            {

                List<string> messages = new List<string>();
                messages.Add(this.Name);
                messages.AddRange(this.Entities.Select(x => x.Name).ToList());
                messages.AddRange(this.Entities.Select(x => x.Name.Pluralize()).ToList());
                return messages.Where(message=> !this.Project.GetMessages.Any(m => m ==message)).ToList();
            }
        }
        public string Name { get; set; }
        public string Schema { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }
        public ICollection<Entity> Entities { get; set; }
        public Context(Project project)
        {
            Project = project;
            Entities = new List<Entity>();
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
                options.AddRange(Entities.Select(x => x.Name));
                options.Add(ADD_ENTITY);
                options.Add(BACK);

                selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Context [blue]{Name}[/] - Choose what to do?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(options.ToArray()));

                if (selectedOption.Equals(EDIT))
                {
                    PromptEdit();
                }

                if (selectedOption.Equals(ADD_ENTITY))
                {
                    AddEntityPrompt();
                }

                if (Entities.Any(entity => selectedOption.Equals(entity.Name)))
                {
                    var entity = Entities.Where(c => selectedOption.Equals(c.Name)).SingleOrDefault();

                    entity.Prompt();
                }
            } while (!selectedOption.Equals(BACK));
        }

        private void AddEntityPrompt()
        {
            var entity = new Entity(this);

            Entities.Add(entity);

            entity.Prompt();
        }

        private void PromptEdit()
        {
            Name = AnsiConsole.Ask<string>("Enter context name?");

            Schema = AnsiConsole.Ask<string>("Enter context schema?");
        }

        public override void Generate()
        {
            this.GenerateDbContext();
            this.GenerateDbContextReader();
            this.GenerateDbContextWriter();
            this.GenerateIDbContextReader();
            this.GenerateIDbContextWriter();
            this.GenerateContextResourceXML();
            this.GenerateContextResourceClass();
            this.GenerateContextInitialMigration();
            this.GenerateSnapshot();
            foreach (var entity in Entities)
            {
                entity.Generate();
            }
        }
    }
}
