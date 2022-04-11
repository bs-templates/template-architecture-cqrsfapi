using BAYSOFT.Core.Domain.Default.Interfaces.Services.Samples;
using BAYSOFT.Core.Domain.Default.Services.Samples;
using Microsoft.Extensions.DependencyInjection;

namespace BAYSOFT.Middleware.AddServices
{
    public static class AddDomainServicesConfigurations
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IPutSampleService, PutSampleService>();
            services.AddTransient<IPostSampleService, PostSampleService>();
            services.AddTransient<IPatchSampleService, PatchSampleService>();
            services.AddTransient<IDeleteSampleService, DeleteSampleService>();

            return services;
        }
    }
}
