using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Default;
using BAYSOFT.Infrastructures.Data.Contexts;
using BAYSOFT.Infrastructures.Data.Default;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BAYSOFT.Middleware.AddServices
{
    public static class AddDbContextConfigurations
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration, Assembly presentationAssembly)
        {
            services.AddTransient<IDefaultDbContextWriter, DefaultDbContextWriter>();
            services.AddTransient<IDefaultDbContextReader, DefaultDbContextReader>();

            services.AddDbContext<DefaultDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly(presentationAssembly.GetName().Name)));

            services.AddDbContext<DefaultDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly(presentationAssembly.GetName().Name)));

            return services;
        }
    }
}
