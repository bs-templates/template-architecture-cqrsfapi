using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Middleware.AddServices;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelWrapper.Middleware;
using System;
using System.Reflection;

namespace BAYSOFT.Middleware
{
    public static class Configurations
    {
        public static IServiceCollection AddMiddleware(this IServiceCollection services, IConfiguration configuration, Assembly presentationAssembly)
        {
            services.AddLocalization();

            services.AddDbContexts(configuration, presentationAssembly);
            services.AddSpecifications();
            services.AddEntityValidations();
            services.AddDomainValidations();
            services.AddDomainServices();

            var assemblyApplication = AppDomain.CurrentDomain.Load("BAYSOFT.Core.Application");
            services.AddMediatR(assemblyApplication);
            
            var assemblyDomain = AppDomain.CurrentDomain.Load("BAYSOFT.Core.Domain");
            services.AddMediatR(assemblyDomain);

            services.AddModelWrapper()
                .AddDefaultReturnedCollectionSize(10)
                .AddMinimumReturnedCollectionSize(1)
                .AddMaximumReturnedCollectionSize(100)
                .AddQueryTermsMinimumSize(3)
                .AddSuppressedTerms(new string[] { "the" });
            
            services.AddInheritStringLocalizerFactory();
            
            // YOUR CODE GOES HERE
            return services;
        }

        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app)
        {
            var supportedCultures = new string[] { "en-US", "pt-BR" };

            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            
            app.UseRequestLocalization(localizationOptions);

            //app.UseAuthentication();
            //app.UseAuthorization();
            // YOUR CODE GOES HERE
            return app;
        }
    }
}
