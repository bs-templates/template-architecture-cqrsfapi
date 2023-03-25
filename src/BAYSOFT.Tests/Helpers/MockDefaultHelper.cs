using BAYSOFT.Abstractions.Core.Domain.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Infrastructures.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace BAYSOFT.Tests.Helpers
{
    public static class MockDefaultHelper
    {
        internal static Mock<IDefaultDbContextWriter> GetMockedDefaultDbContextWriter()
        {
            var mockedDefaultDbContextWriter = new Mock<IDefaultDbContextWriter>();

            return mockedDefaultDbContextWriter;
        }
        internal static Mock<IDefaultDbContextReader> GetMockedDefaultDbContextReader()
        {
            var mockedDefaultDbContextReader = new Mock<IDefaultDbContextReader>();

            return mockedDefaultDbContextReader;
        }
        internal static DefaultDbContext GetInMemoryDefaultDbContext()
        {
            var options = new DbContextOptionsBuilder<DefaultDbContext>()
                .UseInMemoryDatabase("BSINM01T")
                .ConfigureWarnings(cw => cw.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            var context = new DefaultDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        internal static DefaultDbContext SetEntities<TEntity>(this DefaultDbContext context, List<TEntity> entities)
             where TEntity : DomainEntity
        {
            context.Set<TEntity>().AddRange(entities);
            context.SaveChanges();

            return context;
        }
    }
}
