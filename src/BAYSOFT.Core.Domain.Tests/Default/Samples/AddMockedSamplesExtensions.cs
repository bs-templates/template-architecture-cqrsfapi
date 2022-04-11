using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace BAYSOFT.Core.Domain.Tests.Default.Samples
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

        private static Mock<DbSet<Sample>> GetMockedDbSetSamples()
        {
            var collection = GetSamplesCollection();

            var mockedDbSetSamples = collection.MockDbSet();

            return mockedDbSetSamples;
        }

        internal static Mock<IDefaultDbContextWriter> AddMockedSamples(this Mock<IDefaultDbContextWriter> mockedSampleWriter)
        {
            var mockedDbSetSamples = GetMockedDbSetSamples();

            mockedSampleWriter
                .Setup(setup => setup.Query<Sample>())
                .Returns(mockedDbSetSamples.Object);

            return mockedSampleWriter;
        }

        internal static Mock<IDefaultDbContextReader> AddMockedSamples(this Mock<IDefaultDbContextReader> mockedSampleReader)
        {
            var mockedDbSetSamples = GetMockedDbSetSamples();

            mockedSampleReader
                .Setup(setup => setup.Query<Sample>())
                .Returns(mockedDbSetSamples.Object);

            return mockedSampleReader;
        }
    }
}
