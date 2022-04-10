using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Infrastructures.Data.Default.EntityMappings;
using Microsoft.EntityFrameworkCore;

namespace BAYSOFT.Infrastructures.Data.Contexts
{
    public class DefaultDbContext : DbContext
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
