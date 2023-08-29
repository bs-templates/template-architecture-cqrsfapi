using BAYSOFT.CLI.Models.Bases;
using BAYSOFT.CLI.Templates;
using Spectre.Console;
using System.Text.Json.Serialization;

namespace BAYSOFT.CLI.Models
{
    public class Entity : Promptable
    {
        private readonly string EDIT = "## Edit entity name";
        private readonly string ADD_PROPERTY = "+  Add property";
        private readonly string ADD_ENTITY_RULE = "+  Add entity rule";
        private readonly string ADD_VALIDATION_RULE = "+  Add validation rule";
        private readonly string ADD_SPECIFICATION = "+  Add specification";
        private readonly string ADD_SERVICE = "+  Add service";
        private readonly string ADD_COMMAND = "+  Add command";
        private readonly string PROMPT_PROPERTIES = "-- Properties";
        private readonly string PROMPT_ENTITY_RULES = "-- Entity rules";
        private readonly string PROMPT_VALIDATION_RULES = "-- Validation rules";
        private readonly string PROMPT_SPECIFICATIONS = "-- Specifications";
        private readonly string PROMPT_SERVICES = "-- Services";
        private readonly string PROMPT_COMMANDS = "-- Commands";
        private readonly string PROMPT_QUERIES = "-- Queries";
        private readonly string BACK = "<- Back";
        [JsonIgnore]
        public List<string> GetMessages
        {
            get
            {

                List<string> messages = new List<string>();
                messages.Add(this.Name);
                messages.AddRange(this.Properties.Select(x => x.Name).ToList());
                messages.AddRange(this.EntityRules.Select(x => x.Message).ToList());
                messages.AddRange(this.Specifications.Select(x => x.Message).ToList());
                return messages
                    .Where(message => !this.Context.GetMessages.Any(m => m == message))
                    .Where(message => !this.Context.Project.GetMessages.Any(m => m == message))
                    .ToList();
            }
        }
        public string Name { get; set; }
        public bool GetById { get; set; }
        public bool GetByFilter { get; set; }
        [JsonIgnore]
        public Context Context { get; set; }
        public ICollection<Property> Properties { get; set; }
        public ICollection<EntityRule> EntityRules { get; set; }
        public ICollection<EntityRule> ValidationRules { get; set; }
        public ICollection<Specification> Specifications { get; set; }
        public ICollection<Service> Services { get; set; }
        public ICollection<Command> Commands { get; set; }
        public Entity(Context context)
        {
            Context = context;
            Properties = new List<Property>();
            EntityRules = new List<EntityRule>();
            Specifications = new List<Specification>();
            Services = new List<Service>();
            Commands = new List<Command>();
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
                options.Add(PROMPT_PROPERTIES);
                options.Add(PROMPT_ENTITY_RULES);
                options.Add(PROMPT_VALIDATION_RULES);
                options.Add(PROMPT_SPECIFICATIONS);
                options.Add(PROMPT_SERVICES);
                options.Add(PROMPT_COMMANDS);
                options.Add(PROMPT_QUERIES);
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

                if (selectedOption.Equals(PROMPT_PROPERTIES))
                {
                    PromptProperties();
                }

                if (selectedOption.Equals(PROMPT_ENTITY_RULES))
                {
                    PromptEntityRules();
                }

                if (selectedOption.Equals(PROMPT_VALIDATION_RULES))
                {
                    PromptValidationRules();
                }

                if (selectedOption.Equals(PROMPT_SPECIFICATIONS))
                {
                    PromptSpecifications();
                }

                if (selectedOption.Equals(PROMPT_SERVICES))
                {
                    PromptServices();
                }

                if (selectedOption.Equals(PROMPT_COMMANDS))
                {
                    PromptCommands();
                }

                if (selectedOption.Equals(PROMPT_QUERIES))
                {
                    PromptQueries();
                }
            } while (!selectedOption.Equals(BACK));
        }

        private void PromptProperties()
        {
            var selectedOption = string.Empty;
            do
            {
                var options = new List<string> { };

                options.AddRange(Properties.Select(x => x.Name));
                options.Add(ADD_PROPERTY);
                options.Add(BACK);

                selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Entity [blue]{Name}[/] - Choose what to do?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(options.ToArray()));

                if (selectedOption.Equals(ADD_PROPERTY))
                {
                    AddPropertyPrompt();
                }

                if (Properties.Any(property => selectedOption.Equals(property.Name)))
                {
                    var property = Properties.Where(c => selectedOption.Equals(c.Name)).SingleOrDefault();

                    property.Prompt();
                }
            } while (!selectedOption.Equals(BACK));
        }

        private void AddPropertyPrompt()
        {
            var property = new Property(this);

            Properties.Add(property);

            property.Prompt();
        }

        private void PromptEntityRules()
        {
            var selectedOption = string.Empty;
            do
            {
                var options = new List<string> { };

                options.AddRange(EntityRules.Select(x => $"RuleFor(x => x.{x.Property.Name}).{x.Rule}.WithMessage(\"{x.Message}\");"));
                options.Add(ADD_ENTITY_RULE);
                options.Add(BACK);

                selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Entity [blue]{Name}[/] - Choose what to do?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(options.ToArray()));

                if (selectedOption.Equals(ADD_ENTITY_RULE))
                {
                    AddEntityRulePrompt();
                }

                if (EntityRules.Any(propertyRule => selectedOption.Equals($"RuleFor(x => x.{propertyRule.Property.Name}).{propertyRule.Rule}.WithMessage(\"{propertyRule.Message}\");")))
                {
                    var rule = EntityRules.Where(c => selectedOption.Equals($"RuleFor(x => x.{c.Property.Name}).{c.Rule}.WithMessage(\"{c.Message}\");")).SingleOrDefault();

                    rule.Prompt();
                }
            } while (!selectedOption.Equals(BACK));
        }

        private void AddEntityRulePrompt()
        {
            var rule = new EntityRule(this);

            EntityRules.Add(rule);

            rule.Prompt();
        }

        private void PromptValidationRules()
        {
            var selectedOption = new List<string> { };
            var options = new List<string> { };

            options.AddRange(EntityRules.Select(x => x.GenerateRule()));

            selectedOption = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title($"Entity [blue]{Name}[/] - Choose what to do?")
                    .PageSize(10)
                    .Required(false)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(options.ToArray()));

            ValidationRules = EntityRules.Where(rule => selectedOption.Contains(rule.GenerateRule())).ToList();
        }

        private void PromptSpecifications()
        {
            var selectedOption = string.Empty;
            do
            {
                var options = new List<string> { };

                options.AddRange(Specifications.Select(x => x.Name));
                options.Add(ADD_SPECIFICATION);
                options.Add(BACK);

                selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Entity [blue]{Name}[/] - Choose what to do?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(options.ToArray()));

                if (selectedOption.Equals(ADD_SPECIFICATION))
                {
                    AddSpecificationPrompt();
                }

                if (Specifications.Any(specification => selectedOption.Equals(specification.Name)))
                {
                    var specification = Specifications.Where(c => selectedOption.Equals(c.Name)).SingleOrDefault();

                    specification.Prompt();
                }
            } while (!selectedOption.Equals(BACK));
        }

        private void AddSpecificationPrompt()
        {
            var specification = new Specification(this);

            Specifications.Add(specification);

            specification.Prompt();
        }

        private void PromptServices()
        {
            var selectedOption = string.Empty;
            do
            {
                var options = new List<string> { };

                options.AddRange(Services.Select(x => x.Name));
                options.Add(ADD_SERVICE);
                options.Add(BACK);

                selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Entity [blue]{Name}[/] - Choose what to do?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(options.ToArray()));

                if (selectedOption.Equals(ADD_SERVICE))
                {
                    AddServicePrompt();
                }

                if (Services.Any(service => selectedOption.Equals(service.Name)))
                {
                    var service = Services.Where(c => selectedOption.Equals(c.Name)).SingleOrDefault();

                    service.Prompt();
                }
            } while (!selectedOption.Equals(BACK));
        }

        private void AddServicePrompt()
        {
            var service = new Service(this);

            Services.Add(service);

            service.Prompt();
        }

        private void PromptCommands()
        {
            var selectedOption = string.Empty;
            do
            {
                var options = new List<string> { };

                options.AddRange(Commands.Select(x => x.Name));
                options.Add(ADD_COMMAND);
                options.Add(BACK);

                selectedOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Entity [blue]{Name}[/] - Choose what to do?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                        .AddChoices(options.ToArray()));

                if (selectedOption.Equals(ADD_COMMAND))
                {
                    AddCommandPrompt();
                }

                if (Commands.Any(command => selectedOption.Equals(command.Name)))
                {
                    var command = Commands.Where(c => selectedOption.Equals(c.Name)).SingleOrDefault();

                    command.Prompt();
                }
            } while (!selectedOption.Equals(BACK));
        }

        private void AddCommandPrompt()
        {
            var command = new Command(this);

            Commands.Add(command);

            command.Prompt();
        }

        private void PromptQueries()
        {
            var getById = $"Get{Name}ById";
            var getByFilter = $"Get{Name.Pluralize()}ByFilter";
            var options = new List<string> { };

            options.Add(getById);
            options.Add(getByFilter);

            var selectedOption = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title($"Entity [blue]{Name}[/] - Choose what to do?")
                    .Required(false)
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(options.ToArray()));

            GetById = selectedOption.Contains(getById);
            GetByFilter = selectedOption.Contains(getByFilter);
        }

        private void PromptEdit()
        {
            Name = AnsiConsole.Ask<string>("Enter entity name?");
        }

        public override void Generate()
        {
            this.GenerateEntityFile();
            this.GenerateEntityValidationFile();
            this.GenerateEntityMapping();
            this.GenerateEntityResourceXML();
            this.GenerateEntityResourceClass();
            this.GenerateControllerFile();
            foreach (var specification in Specifications)
            {
                specification.Generate();
            }
            foreach (var service in Services)
            {
                service.Generate();
            }
            foreach (var command in Commands)
            {
                command.Generate();
            }
            if (GetById)
            {
                this.GenerateGetByIdQuery();
                this.GenerateGetByIdQueryHandler();
                this.GenerateGetByIdQueryResponse();
            }
            if (GetByFilter)
            {
                this.GenerateGetByFilterQuery();
                this.GenerateGetByFilterQueryHandler();
                this.GenerateGetByFilterQueryResponse();
            }
        }
    }
}
