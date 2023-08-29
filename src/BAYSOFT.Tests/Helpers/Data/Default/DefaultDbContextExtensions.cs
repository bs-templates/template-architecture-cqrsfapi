using BAYSOFT.Infrastructures.Data.Default;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BAYSOFT.Tests.Helpers.Data.Default
{
    public static class DefaultDbContextExtensions
    {
        public static DefaultDbContext GetInMemoryDefaultDbContext()
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

        public static DefaultDbContextReader GetDbContextReader(this DefaultDbContext context)
        {
            return new DefaultDbContextReader(context);
        }

        public static DefaultDbContextWriter GetDbContextWriter(this DefaultDbContext context)
        {
            return new DefaultDbContextWriter(context);
        }
    }
}
