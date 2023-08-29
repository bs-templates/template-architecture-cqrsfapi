using BAYSOFT.Abstractions.Core.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BAYSOFT.Tests.Helpers
{
    public static class ServerHelper
    {
        public static TestServer Create(Action<IServiceCollection>? actionServices = null)
        {
            if (actionServices != null)
                return new TestServer(new WebHostBuilder().ConfigureTestServices(actionServices).UseStartup<TestStartup>());

            return new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
        }

        public static TestServer SetupData<TContext, TEntity>(this TestServer server, IEnumerable<TEntity> entities)
            where TContext : DbContext
            where TEntity : DomainEntity
        {
            var context = server.Services.GetService<TContext>();

            if (context != null)
            {
                context.Set<TEntity>().AddRange(entities);
                context.SaveChanges();
            }

            return server;
        }
    }
}
