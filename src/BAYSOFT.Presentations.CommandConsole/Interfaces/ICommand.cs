namespace BAYSOFT.Presentations.CommandConsole.Interfaces
{
    interface ICommand
    {
        string Name { get; }
        string CommandLine { get; }
        Task<bool> Run(string[] args);
    }
}
