using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Tests.Helpers;
using BAYSOFT.Tests.Helpers.Data.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BAYSOFT.Tests.UnitTests.Infrastructures.Data.Default
{
    [TestClass]
    public class DefaultDbContextReaderScenarios
    {
        [TestMethod]
        public void DefaultDbContext_Query_ToList_Should_Return_All_Samples()
        {
            var mockDefaultDbContextReader = MockDefaultHelper
                .GetMockedDefaultDbContextReader()
                .AddMockedSamples();

            var samples = mockDefaultDbContextReader.Object.Query<Sample>().ToList();

            Assert.IsNotNull(samples);
            Assert.AreEqual(samples.Count, 2);
        }
        
        [TestMethod]
        public void DefaultDbContext_Query_Where_Id_Should_Return_Only_One()
        {
            var mockDefaultDbContextReader = MockDefaultHelper
                .GetMockedDefaultDbContextReader()
                .AddMockedSamples();

            var samples = mockDefaultDbContextReader.Object.Query<Sample>().Where(x=>x.Id == 1).ToList();

            Assert.IsNotNull(samples);
            Assert.AreEqual(samples.Count, 1);
        }

        [TestMethod]
        public void DefaultDbContext_Query_Where_Id_SingleOrDefault_Should_Return_Only_One()
        {
            var mockDefaultDbContextReader = MockDefaultHelper
                .GetMockedDefaultDbContextReader()
                .AddMockedSamples();

            var samples = mockDefaultDbContextReader.Object.Query<Sample>().Where(x => x.Id == 1).SingleOrDefault();

            Assert.IsNotNull(samples);
        }

        [TestMethod]
        public void DefaultDbContext_Query_Where_Id_SingleOrDefault_Should_Return_Null()
        {
            var mockDefaultDbContextReader = MockDefaultHelper
                .GetMockedDefaultDbContextReader()
                .AddMockedSamples();

            var samples = mockDefaultDbContextReader.Object.Query<Sample>().Where(x => x.Id == 999).SingleOrDefault();

            Assert.IsNull(samples);
        }
    }
}
