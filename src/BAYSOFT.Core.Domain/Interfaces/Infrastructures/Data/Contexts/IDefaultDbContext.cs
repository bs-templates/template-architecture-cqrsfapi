using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts
{
    public interface IDefaultDbContext : IDefaultDbContextQuery
    {
        public int SaveChanges(bool acceptAllChangesOnSuccess);
        public int SaveChanges();
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
