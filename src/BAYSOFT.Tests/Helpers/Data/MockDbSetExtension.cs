using Moq;
using System.Linq;

namespace BAYSOFT.Tests.Helpers.Data
{
    internal static class MockIQueryableExtension
    {
        internal static Mock<IQueryable<T>> Mock<T>(this IQueryable<T> source)
           where T : class
        {
            var mock = new Mock<IQueryable<T>>();

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
        internal static Mock<IQueryable<T>> MockQueryable<T>(this IQueryable<T> source)
           where T : class
        {
            var mock = new Mock<IQueryable<T>>();

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
