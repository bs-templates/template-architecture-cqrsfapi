using BAYSOFT.Abstractions.Core.Domain.Entities;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BAYSOFT.Infrastructures.Data
{
    public class Reader: IReader
    {
        public DbContext Context { get; private set; }
        public Reader(DbContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : DomainEntity
        {
            return Context.Set<TEntity>().AsQueryable().AsNoTracking();
        }
    }
}
