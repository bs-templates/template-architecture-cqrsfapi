using BAYSOFT.Presentations.CommandConsole.Interfaces;

namespace BAYSOFT.Presentations.CommandConsole.Commands
{
    public class ExitCommand : ICommand
    {
        public ExitCommand()
        {
            Name = "Exit";
            CommandLine = "exit";
        }
        private bool ExitWhenComplete { get { return true; } }

        public string Name { get; private set; }

        public string CommandLine { get; private set; }

        public Task<bool> Run(string[] args)
        {
            return Task.FromResult(ExitWhenComplete);
        }
    }
}
