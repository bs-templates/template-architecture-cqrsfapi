using BAYSOFT.Abstractions.Core.Domain.Entities;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Infrastructures.Data
{
    public class Writer: IWriter
    {
        public DbContext Context { get; private set; }
        public Writer(DbContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : DomainEntity
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public void Add<TEntity>(TEntity entity) where TEntity : DomainEntity
        {
            Context.Set<TEntity>().Add(entity);
        }

        public Task AddAsync<TEntity>(TEntity entity) where TEntity : DomainEntity
        {
            return Context.Set<TEntity>().AddAsync(entity).AsTask();
        }

        public void AddRange<TEntity>(params TEntity[] entities) where TEntity : DomainEntity
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public Task AddRangeAsync<TEntity>(params TEntity[] entities) where TEntity : DomainEntity
        {
            return Context.Set<TEntity>().AddRangeAsync(entities);
        }
        public void Remove<TEntity>(TEntity entity) where TEntity : DomainEntity
        {
            Context.Set<TEntity>().Remove(entity);
        }
        public void RemoveRange<TEntity>(params TEntity[] entities) where TEntity : DomainEntity
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
        public int Commit()
        {
            return Context.SaveChanges();
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }
    }
}
