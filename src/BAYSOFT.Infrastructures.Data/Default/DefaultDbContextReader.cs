using BAYSOFT.Abstractions.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;

namespace BAYSOFT.Infrastructures.Data.Default
{
    public class DefaultDbContextReader: Reader, IDefaultDbContextReader
    {
        public DefaultDbContextReader(DefaultDbContext context) : base(context)
        {
        }
    }
}
