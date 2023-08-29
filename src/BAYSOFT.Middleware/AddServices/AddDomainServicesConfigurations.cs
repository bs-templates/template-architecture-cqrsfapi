using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Services;
using BAYSOFT.Infrastructures.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BAYSOFT.Middleware.AddServices
{
    public static class AddDomainServicesConfigurations
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
