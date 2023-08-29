using BAYSOFT.Core.Application.Default.Samples.Queries;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Presentations.CommandConsole.Helpers;
using BAYSOFT.Presentations.CommandConsole.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModelWrapper.Extensions.Filter;
using ModelWrapper.Extensions.GetModel;
using ModelWrapper.Extensions.GetModels;
using ModelWrapper.Extensions.Ordination;
using ModelWrapper.Extensions.Pagination;
using ModelWrapper.Extensions.Search;
using ModelWrapper.Extensions.Notifications;

namespace BAYSOFT.Presentations.CommandConsole.Commands
{
    public class ReadCommand : ICommand
    {
        public ReadCommand()
        {
            Name = "Read";
            CommandLine = "read";
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
                case "samples": await ReadSamples(); break;
                case "sample": await ReadSample(); break;
                default: Console.WriteLine("Unknown argument!"); Console.ReadLine(); break;
            }

            return ExitWhenComplete;
        }

        private async Task ReadSamples()
        {
            Console.Clear();
            var startedAt = DateTime.UtcNow;
            Console.WriteLine($" - {Name} - Sample - Started");

            string? queryString = ConsoleHelper.RequestInformation<string?>("QueryString: ");

            var cancellationToken = new CancellationToken();

            using (var scope = CommandConsoleContext.GetServiceProvider()?.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                GetSamplesByFilterQueryResponse? response = null;
                var command = new GetSamplesByFilterQuery();

                queryString?
                    .Split("&")
                    .Where(keyPair => !string.IsNullOrWhiteSpace(keyPair))
                    .ToList()
                    .ForEach(keyPair =>
                    {
                        var nameValue = keyPair.Split("=");
                        command.AddProperty(nameValue[0], nameValue[1], ModelWrapper.WrapPropertySource.FromQuery);
                    });

                var mediator = scope?.ServiceProvider.GetService<IMediator>();
                if (mediator != null)
                {
                    response = await mediator.Send(command, cancellationToken);
                }

                if (response?.ResultCount > 0)
                {
                    var samples = response.GetModels();

                    if (samples != null && samples.Count > 0)
                    {
                        samples.ToList().ForEach(sample => Console.WriteLine($"{sample.Id} - {sample.Description}"));

                        Console.WriteLine($"Exibindo {samples.Count} de {response.ResultCount} registros");

                        var pagination = response.Pagination();
                        Console.WriteLine($"Pagina {pagination.Number}");

                        var ordination = response.Ordination();
                        Console.WriteLine($"Order by: {ordination.OrderBy}; Order: {ordination.Order};");

                        response.Filters().ForEach(filter => Console.WriteLine($"{filter.Name}: {filter.Value.ToString()}"));
                    }
                }
            }

            Console.WriteLine($" - {Name} - Finished - {DateTime.UtcNow.Subtract(startedAt).TotalSeconds} seconds");
            Console.ReadLine();
        }

        private async Task ReadSample()
        {
            Console.Clear();
            var startedAt = DateTime.UtcNow;
            Console.WriteLine($" - {Name} - Sample - Started");
            var cancellationToken = new CancellationToken();

            int sampleId = ConsoleHelper.RequestInformation<int>("Sample id: ");

            using (var scope = CommandConsoleContext.GetServiceProvider()?.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                GetSampleByIdQueryResponse? response = null;
                var command = new GetSampleByIdQuery();
                var mediator = scope?.ServiceProvider.GetService<IMediator>();
                if (mediator != null)
                {
                    command.Project(c => { c.Id = sampleId; });
                    command.ClearFilters();
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
