using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Default;
using Moq;

namespace BAYSOFT.Core.Domain.Services.Tests.Default
{
    public static class MockDefaultHelper
    {
        internal static Mock<IDefaultDbContextWriter> GetMockedDefaultDbContextWriter()
        {
            var mockedSampleWriter = new Mock<IDefaultDbContextWriter>();

            return mockedSampleWriter;
        }
        internal static Mock<IDefaultDbContextReader> GetMockedDefaultDbContextReader()
        {
            var mockedSampleReader = new Mock<IDefaultDbContextReader>();

            return mockedSampleReader;
        }
    }
}
