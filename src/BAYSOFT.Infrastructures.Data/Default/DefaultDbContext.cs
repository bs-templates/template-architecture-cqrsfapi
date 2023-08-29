using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Infrastructures.Data.Default.EntityMappings;
using Microsoft.EntityFrameworkCore;

namespace BAYSOFT.Infrastructures.Data.Default
{
    public class DefaultDbContext : DbContext
    {
        public DbSet<Sample> Samples { get; set; }

        protected DefaultDbContext()
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }

        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.ApplyConfiguration(new SampleMap());
        }
    }
}
