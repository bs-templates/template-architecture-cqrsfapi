using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace BAYSOFT.Core.Domain.Services.Tests.Default.Samples
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

        internal static Mock<IDefaultDbContext> AddMockedSamples(this Mock<IDefaultDbContext> mockedDbContext)
        {
            var mockedDbSetSamples = GetMockedDbSetSamples();

            mockedDbContext
                .Setup(setup => setup.Samples)
                .Returns(mockedDbSetSamples.Object);

            return mockedDbContext;
        }

        internal static Mock<IDefaultDbContextQuery> AddMockedSamples(this Mock<IDefaultDbContextQuery> mockedDbContextQuery)
        {
            var mockedDbSetSamples = GetMockedDbSetSamples();

            mockedDbContextQuery
                .Setup(setup => setup.Samples)
                .Returns(mockedDbSetSamples.Object);

            return mockedDbContextQuery;
        }
    }
}
