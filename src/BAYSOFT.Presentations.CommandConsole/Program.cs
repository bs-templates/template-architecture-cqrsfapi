using BAYSOFT.Presentations.CommandConsole.Interfaces;
using System.Reflection;

namespace BAYSOFT.Presentations.CommandConsole
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var assembly = typeof(Program).GetTypeInfo().Assembly;

            var commandInterface = typeof(ICommand);

            var commands = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && commandInterface.IsAssignableFrom(type))
                .Select(type => Activator.CreateInstance(type) as ICommand)
                .ToList();

            bool exit = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Store - Command Console");
                Console.WriteLine("----");
                Console.Write("cmd: ");
                
                var commandAndArgs = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(commandAndArgs)) continue;

                var commandArgs = commandAndArgs.Split(' ');

                var command = commandArgs[0];

                var selectedCommand = commands.FirstOrDefault(c => c != null && c.CommandLine.ToLower().Equals(command.ToLower()));

                if (selectedCommand != null)
                {
                    exit = await selectedCommand.Run(commandArgs.Skip(1).ToArray());
                }
            } while (!exit);
        }
    }
}