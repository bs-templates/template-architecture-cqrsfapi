using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using Moq;

namespace BAYSOFT.Core.Domain.Services.Tests.Default
{
    public static class MockDefaultHelper
    {
        internal static Mock<IDefaultDbContext> GetMockedDbContext()
        {
            var mockedDbContext = new Mock<IDefaultDbContext>();

            return mockedDbContext;
        }
        internal static Mock<IDefaultDbContextQuery> GetMockedDbContextQuery()
        {
            var mockedDbContextQuery = new Mock<IDefaultDbContextQuery>();

            return mockedDbContextQuery;
        }
    }
}
