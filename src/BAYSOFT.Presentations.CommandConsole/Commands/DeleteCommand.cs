﻿using BAYSOFT.Core.Application.Default.Samples.Commands;
using BAYSOFT.Presentations.CommandConsole.Helpers;
using BAYSOFT.Presentations.CommandConsole.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModelWrapper.Extensions.GetModel;

namespace BAYSOFT.Presentations.CommandConsole.Commands
{
    public class DeleteCommand : ICommand
    {
        public DeleteCommand()
        {
            Name = "Delete";
            CommandLine = "delete";
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
                case "sample": await DeleteSample(); break;
                default: Console.WriteLine("Unknown argument!"); Console.ReadLine(); break;
            }

            return ExitWhenComplete;
        }

        private async Task DeleteSample()
        {
            Console.Clear();
            var startedAt = DateTime.UtcNow;
            Console.WriteLine($" - {Name} - Sample - Started");
            var cancellationToken = new CancellationToken();

            int sampleId = ConsoleHelper.RequestInformation<int>("Sample id: ");

            using (var scope = CommandConsoleContext.GetServiceProvider()?.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                DeleteSampleCommandResponse? response = null;
                var command = new DeleteSampleCommand();
                var mediator = scope?.ServiceProvider.GetService<IMediator>();
                if (mediator != null)
                {
                    command.Project(c => { c.Id = sampleId; });
                    response = await mediator.Send(command, cancellationToken);
                }

                if (response?.ResultCount > 0)
                {
                    var sample = response.GetModel();

                    if (sample != null)
                        Console.WriteLine($"{sample.Id} - {sample.Description}");
                }
            }

            Console.WriteLine($" - {Name} - Finished - {DateTime.UtcNow.Subtract(startedAt).TotalSeconds} seconds");
            Console.ReadLine();
        }
    }
}
