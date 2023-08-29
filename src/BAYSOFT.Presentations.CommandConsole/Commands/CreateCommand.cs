using BAYSOFT.Core.Application.Default.Samples.Commands;
using BAYSOFT.Presentations.CommandConsole.Helpers;
using BAYSOFT.Presentations.CommandConsole.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModelWrapper.Extensions.GetModel;
using ModelWrapper.Extensions.Notifications;

namespace BAYSOFT.Presentations.CommandConsole.Commands
{
    public class CreateCommand : ICommand
    {
        public CreateCommand()
        {
            Name = "Create";
            CommandLine = "create";
        }
        private bool ExitWhenComplete { get { return false; } }
        public string Name { get; private set; }

        public string CommandLine { get; private set; }


        public async Task<bool> Run(string[] args)
        {
            if (args.Count() == 0)
            {
                Console.WriteLine("No argument supplied!");
                Console.ReadLine();
                return ExitWhenComplete;
            }

            switch (args.First().ToLower())
            {
                case "sample": await CreateSample(); break;
                default: Console.WriteLine("Unknown argument!"); Console.ReadLine(); break;
            }

            return ExitWhenComplete;
        }

        private async Task CreateSample()
        {
            Console.Clear();
            var startedAt = DateTime.UtcNow;
            Console.WriteLine($" - {Name} - Sample - Started");
            var cancellationToken = new CancellationToken();

            string? sampleDescription = ConsoleHelper.RequestInformation<string>("Sample description: ");

            using (var scope = CommandConsoleContext.GetServiceProvider()?.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                PostSampleCommandResponse? response = null;
                var command = new PostSampleCommand();
                var mediator = scope?.ServiceProvider.GetService<IMediator>();
                if (mediator != null)
                {
                    command.Project(c => { c.Description = sampleDescription; });
                    command.AddProperty("culture", "pt-br");
                    response = await mediator.Send(command, cancellationToken);
                }

                if (response != null && response.ResultCount > 0)
                {
                    var sample = response.GetModel();

                    if (sample != null)
                        Console.WriteLine($"{sample.Id} - {sample.Description}");
                }

                if (response != null && response.Notifications != null)
                {
                    Console.WriteLine($"Message: {response.Notifications.GetMessage()}");
                    Console.WriteLine($"Notification JSON: '{response.Notifications.GetJson()}'");
                    if (response.Notifications.HasInner())
                    {
                        Console.WriteLine($"InnerMessage: {response.Notifications.GetInner().GetMessage()}");
                    }
                }
            }

            Console.WriteLine($" - {Name} - Finished - {DateTime.UtcNow.Subtract(startedAt).TotalSeconds} seconds");
            Console.ReadLine();
        }
    }
}
