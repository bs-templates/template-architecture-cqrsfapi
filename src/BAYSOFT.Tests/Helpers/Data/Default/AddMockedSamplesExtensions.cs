using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using Moq;

namespace BAYSOFT.Tests.Helpers.Data.Default
{
    internal static class AddMockedSamplesExtensions
    {
        private static IQueryable<Sample> GetSamplesCollection()
        {
            return new List<Sample> {
                new Sample { Id = 1, Description = "Sample - 001" },
                new Sample { Id = 2, Description = "Sample - 002" },
            }.AsQueryable();
        }

        private static Mock<IQueryable<Sample>> GetMockedQueryableSamples()
        {
            var collection = GetSamplesCollection();

            var mockedQueryableSamples = collection.MockQueryable();

            return mockedQueryableSamples;
        }

        internal static Mock<IDefaultDbContextWriter> AddMockedSamples(this Mock<IDefaultDbContextWriter> mockedDefaultDbContextWriter)
        {
            var mockedQueryableSamples = GetMockedQueryableSamples();

            mockedDefaultDbContextWriter
                .Setup(setup => setup.Query<Sample>())
                .Returns(mockedQueryableSamples.Object);

            return mockedDefaultDbContextWriter;
        }

        internal static Mock<IDefaultDbContextReader> AddMockedSamples(this Mock<IDefaultDbContextReader> mockedDefaultDbContextReader)
        {
            var mockedQueryableSamples = GetMockedQueryableSamples();

            mockedDefaultDbContextReader
                .Setup(setup => setup.Query<Sample>())
                .Returns(mockedQueryableSamples.Object);

            return mockedDefaultDbContextReader;
        }
    }
}
