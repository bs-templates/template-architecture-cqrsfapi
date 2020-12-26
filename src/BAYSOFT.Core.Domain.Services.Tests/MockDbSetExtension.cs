using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;

namespace BAYSOFT.Core.Domain.Services.Tests
{
    internal static class MockDbSetExtension
    {
        internal static Mock<DbSet<T>> MockDbSet<T>(this IQueryable<T> source)
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
    }
}
