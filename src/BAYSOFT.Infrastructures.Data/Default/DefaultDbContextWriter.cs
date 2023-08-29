using BAYSOFT.Abstractions.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;

namespace BAYSOFT.Infrastructures.Data.Default
{
    public class DefaultDbContextWriter : Writer, IDefaultDbContextWriter
    {
        public DefaultDbContextWriter(DefaultDbContext context) : base(context)
        {
        }
    }
}
