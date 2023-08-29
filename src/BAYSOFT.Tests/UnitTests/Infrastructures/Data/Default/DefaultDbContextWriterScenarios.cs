using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Tests.Helpers.Data.Default;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BAYSOFT.Tests.UnitTests.Infrastructures.Data.Default
{
    [TestClass]
    public class DefaultDbContextWriterScenarios
    {
        [TestMethod]
        public void DefaultDbContextReader_Add_Should_Not_Throw_Exception()
        {
            using(var context = DefaultDbContextExtensions.GetInMemoryDefaultDbContext().SetupSamples())
            {
                var defaultDbContextReader = context.GetDbContextReader();
                var defaultDbContextWriter = context.GetDbContextWriter();

                var entity = new Sample { Description = "new sample" };

                defaultDbContextWriter.Add(entity);
                context.SaveChanges();

                Assert.IsTrue(defaultDbContextReader.Query<Sample>().Any(x => x.Description == entity.Description));
                Assert.IsTrue(entity.Id != 0);
            }
        }
    }
}
