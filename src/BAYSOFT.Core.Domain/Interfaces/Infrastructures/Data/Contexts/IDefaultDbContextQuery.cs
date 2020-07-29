using BAYSOFT.Core.Domain.Entities.Default;
using Microsoft.EntityFrameworkCore;

namespace BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts
{
    public interface IDefaultDbContextQuery
    {
        public DbSet<Sample> Samples { get; set; }
    }
}
