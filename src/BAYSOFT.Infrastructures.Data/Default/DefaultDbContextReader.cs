using BAYSOFT.Abstractions.Core.Domain.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Infrastructures.Data.Contexts;

namespace BAYSOFT.Infrastructures.Data.Default
{
    public class DefaultDbContextReader: Reader, IDefaultDbContextReader
    {
        public DefaultDbContextReader(DefaultDbContext context) : base(context)
        {
        }
    }
}
