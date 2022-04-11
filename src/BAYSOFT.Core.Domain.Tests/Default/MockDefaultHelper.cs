using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using Moq;

namespace BAYSOFT.Core.Domain.Tests.Default
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
