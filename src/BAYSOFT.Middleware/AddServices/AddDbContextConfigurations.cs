using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Infrastructures.Data.Contexts;
using BAYSOFT.Infrastructures.Data.Default;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BAYSOFT.Middleware.AddServices
{
    public static class AddDbContextConfigurations
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDefaultDbContextWriter, DefaultDbContextWriter>();
            services.AddTransient<IDefaultDbContextReader, DefaultDbContextReader>();

            services.AddDbContext<DefaultDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly(typeof(DefaultDbContext).Assembly.GetName().Name)));

            return services;
        }
        public static IServiceCollection AddDbContextsTest(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDefaultDbContextWriter, DefaultDbContextWriter>();
            services.AddTransient<IDefaultDbContextReader, DefaultDbContextReader>();

            services.AddDbContext<DefaultDbContext>(options => 
                options.UseInMemoryDatabase(nameof(DefaultDbContext), new InMemoryDatabaseRoot()),
                ServiceLifetime.Singleton);

            return services;
        }
    }
}
