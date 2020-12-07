using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace BAYSOFT.Core.Domain.Services.Tests.Default
{
    public static class MockDefaultHelper
    {
        private static Mock<DbSet<T>> BuildMockDbSet<T>(this IQueryable<T> source)
           where T : class
        {
            var mock = new Mock<DbSet<T>>();

            mock.As<IQueryable<T>>()
                .Setup(x => x.Provider)
                .Returns(source.Provider);

            mock.As<IQueryable<T>>()
                .Setup(x => x.Expression)
                .Returns(source.Expression);

            mock.As<IQueryable<T>>()
                .Setup(x => x.ElementType)
                .Returns(source.ElementType);

            mock.As<IQueryable<T>>()
                .Setup(x => x.GetEnumerator())
                .Returns(source.GetEnumerator());

            return mock;
        }
        private static IQueryable<Sample> GetSchoolsCollection()
        {
            return new List<Sample> {
                new Sample { Id = 1, Description = "Sample - 001" },
                new Sample { Id = 2, Description = "Sample - 002" },
            }.AsQueryable();
        }
        
        private static Mock<DbSet<Sample>> GetMockedDbSetSamples()
        {
            var schoolsCollection = GetSchoolsCollection();

            var mockedDbSetSchools = schoolsCollection.BuildMockDbSet();

            return mockedDbSetSchools;
        }
        
        internal static Mock<IDefaultDbContext> GetMockedDefaultDbContext()
        {
            var mockedDbSetSamples = GetMockedDbSetSamples();

            var mockedDeafultDbContext = new Mock<IDefaultDbContext>();

            mockedDeafultDbContext
                .Setup(setup => setup.Samples)
                .Returns(mockedDbSetSamples.Object);

            return mockedDeafultDbContext;
        }
        internal static Mock<IDefaultDbContextQuery> GetMockedDefaultDbContextQuery()
        {
            var mockedDbSetSamples = GetMockedDbSetSamples();

            var mockedDeafultDbContextQuery = new Mock<IDefaultDbContextQuery>();

            mockedDeafultDbContextQuery
                .Setup(setup => setup.Samples)
                .Returns(mockedDbSetSamples.Object);

            return mockedDeafultDbContextQuery;
        }
    }
}
