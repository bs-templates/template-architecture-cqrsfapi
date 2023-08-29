using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Infrastructures.Data.Default;
using BAYSOFT.Tests.Helpers;
using BAYSOFT.Tests.Helpers.Data.Default.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace BAYSOFT.Tests.IntegrationTests.Samples
{
    [TestClass]
    public class DeleteSampleScenarios
    {
        [TestMethod]
        public async Task DELETE_Samples_Should_Return_Ok()
        {
            var contextData = SamplesCollections.GetDefaultCollection();

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var response = await client.DeleteAsync($"/api/samples/2");

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
        [TestMethod]
        public async Task DELETE_Samples_Where_Sample_Doesnt_Exists_Should_Return_NotFound()
        {
            var contextData = SamplesCollections.GetDefaultCollection().Where(x=>x.Id != 2);

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var response = await client.DeleteAsync($"/api/samples/2");

                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
