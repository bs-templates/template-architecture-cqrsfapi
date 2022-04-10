using BAYSOFT.Abstractions.Core.Domain.Entities;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Default;
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
