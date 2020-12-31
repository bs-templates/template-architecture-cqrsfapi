using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using BAYSOFT.Infrastructures.Data.EntityMappings.Default;

namespace BAYSOFT.Infrastructures.Data.Contexts
{
    public class DefaultDbContext : DbContext, IDefaultDbContext
    {
        public DbSet<Sample> Samples { get; set; }

        protected DefaultDbContext()
        {
            Database.Migrate();
        }

        public DefaultDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SampleMap());
        }
    }
}
