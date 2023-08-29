using BAYSOFT.Middleware;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BAYSOFT.Presentations.CommandConsole
{
    public class CommandConsoleContext
    {
        private static IConfiguration configuration;
        private static ServiceProvider serviceProvider;
        public static ServiceProvider GetServiceProvider()
        {
            if(serviceProvider == null)
            {
                serviceProvider = new ServiceCollection()
                    .AddMiddleware(GetConfiguration(), typeof(Program).GetTypeInfo().Assembly)
                    .BuildServiceProvider();
            }

            return serviceProvider;
        }

        public static IConfiguration GetConfiguration()
        {
            if (configuration == null)
            {
                var customEnviroment = "Development";
                #if TEST
                    customEnviroment = "Test";
                #elif RELEASE
                    customEnviroment = "Production";
                #endif

                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{customEnviroment}.json", true, true)
                    .AddEnvironmentVariables();
                
                configuration = builder.Build(); 
            }

            return configuration;
        }

        public static IMediator GetMediator()
        {
            return GetServiceProvider().GetService<IMediator>();
        }
    }
}
