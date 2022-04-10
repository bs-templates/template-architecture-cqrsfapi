using BAYSOFT.Abstractions.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data
{
    public interface IReader
    {
        public IQueryable<TEntity> Query<TEntity>() where TEntity : DomainEntity;
    }
}
