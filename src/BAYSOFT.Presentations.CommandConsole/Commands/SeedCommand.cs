using BAYSOFT.Core.Application.Default.Samples.Commands;
using BAYSOFT.Core.Application.Default.Samples.Queries;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Presentations.CommandConsole.Extensions;
using BAYSOFT.Presentations.CommandConsole.Helpers;
using BAYSOFT.Presentations.CommandConsole.Interfaces;
using ChanceNET;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModelWrapper.Extensions.GetModel;
using System;

namespace BAYSOFT.Presentations.CommandConsole.Commands
{
    public class SeedCommand : ICommand
    {
        public SeedCommand()
        {
            Name = "Seed";
            CommandLine = "seed";
        }
        private bool ExitWhenComplete { get { return false; } }
        public string Name { get; private set; }

        public string CommandLine { get; private set; }


        public async Task<bool> Run(string[] args)
        {
            if(args.Count() == 0)
            {
                Console.WriteLine("No argument supplied!");
                Console.ReadLine();
                return ExitWhenComplete;
            }

            switch (args.First().ToLower())
            {
                case "samples": await SeedSamples(); break;
                default: Console.WriteLine("Unknown argument!"); Console.ReadLine(); break;
            }

            return ExitWhenComplete;
        }

        private async Task SeedSamples()
        {
            Console.Clear();
            var startedAt = DateTime.UtcNow;
            Console.WriteLine($" - {Name} - Samples - Started");
            var cancellationToken = new CancellationToken();
            int nonRegisters = ConsoleHelper.RequestInformation<int>("Enter the number of new registers: ");
            Console.WriteLine($"{nonRegisters} new registers.");

            Chance chance = new Chance(DateTime.UtcNow.Ticks.ToString());

            var delegates = new List<Func<Chance, Task<PostSampleCommandResponse?>>>();

            for (int i = 0; i < nonRegisters; i++)
            {
                delegates.Add((chance) =>
                {
                    var sample = new Sample { Description = $"Sample: {chance.FullName()}; Genereted at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}." };
                    var task = Task.Run(async () =>
                    {
                        using (var scope = CommandConsoleContext.GetServiceProvider()?.GetService<IServiceScopeFactory>()?.CreateScope())
                        {
                            PostSampleCommandResponse? response = null;
                            var command = new PostSampleCommand();
                            var mediator = scope?.ServiceProvider.GetService<IMediator>();
                            if (mediator != null)
                            {
                                command.Project(c => { c.Description = sample.Description; });
                                response = await mediator.Send(command, cancellationToken);
                            }

                            return response;
                        }
                    });

                    return task;
                });
            }
            var processCount = 0;
            var processTotal = delegates.Count();
            long processedCount = 0;
            foreach (var batch in delegates.Batch(100))
            {
                Console.WriteLine($" - batch of {batch.Count()} in progress - {processCount} to {processCount + batch.Count()} out of {processTotal}");
                var responses = batch.Select(item => item(chance)).ToList();

                await Task.WhenAll(responses);

                Console.WriteLine($"{responses.Where(x => x.IsCompleted).Count()} registers saved.");

                responses.ForEach(response =>
                {
                    if (response.IsCompleted && response.Result?.ResultCount > 0)
                    {
                        var sample = response.Result.GetModel();

                        if (sample != null)
                            Console.WriteLine($"{sample.Id} - {sample.Description}");
                    }
                });
                processCount = processCount + batch.Count();
                var processedOnBatch = responses.Select(x => x.Result?.ResultCount).Sum();
                processedCount = processedCount + (processedOnBatch.HasValue ? processedOnBatch.Value : 0);
            }

            Console.WriteLine($" - {Name} - Finished - {delegates.Count} registers  - {processedCount} processed - {DateTime.UtcNow.Subtract(startedAt).TotalSeconds} seconds");
            Console.ReadLine();
        }
    }
}
