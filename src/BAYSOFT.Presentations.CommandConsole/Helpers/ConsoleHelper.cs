using Microsoft.VisualBasic;

namespace BAYSOFT.Presentations.CommandConsole.Helpers
{
    public static class ConsoleHelper
    {
        public static string COMMAND_EXIT = "exit";
        public static string COMMAND_QUIT = "quit";
        public static T? RequestInformation<T>(string requestMessage)
        {
            object? response = null;
            string? information = null;
            bool responseIsValid = false;

            do
            {
                responseIsValid = false;
                Console.WriteLine(requestMessage);
                information = Console.ReadLine();

                if (information == null) { Console.WriteLine("Request information failed, retry."); continue; }

                try
                {
                    response = Convert.ChangeType(information, typeof(T));
                    responseIsValid = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Request information failed, retry. message: {ex.Message}"); continue;
                }

            } while (!responseIsValid && information != null && !information.ToLower().Equals(COMMAND_QUIT));

            return response == null ? default : (T)response;
        }
    }
}
