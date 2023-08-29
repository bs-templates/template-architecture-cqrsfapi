using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Infrastructures.Data.Default;
using BAYSOFT.Tests.Helpers;
using BAYSOFT.Tests.Helpers.Data.Default.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace BAYSOFT.Tests.IntegrationTests.Samples
{
    [TestClass]
    public class GetSampleByIdScenarios
    {
        [TestMethod]
        public async Task GET_Sample_By_Id_Should_Return_Ok()
        {
            var contextData = SamplesCollections.GetDefaultCollection();

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var response = await client.GetAsync($"/api/samples/1");

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
