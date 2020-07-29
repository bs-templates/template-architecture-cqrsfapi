using BAYSOFT.Core.Domain.Entities;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Interfaces.Services
{
    public interface IDomainService<TEntity>
        where TEntity : DomainEntity
    {
        Task Run(TEntity entity);
    }
}
