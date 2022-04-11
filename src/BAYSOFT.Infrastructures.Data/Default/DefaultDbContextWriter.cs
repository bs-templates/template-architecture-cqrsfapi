using BAYSOFT.Abstractions.Core.Domain.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Infrastructures.Data.Contexts;

namespace BAYSOFT.Infrastructures.Data.Default
{
    public class DefaultDbContextWriter : Writer, IDefaultDbContextWriter
    {
        public DefaultDbContextWriter(DefaultDbContext context) : base(context)
        {
        }
    }
}
