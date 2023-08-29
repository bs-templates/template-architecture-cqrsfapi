using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Infrastructures.Data.Default;
using BAYSOFT.Tests.Helpers;
using BAYSOFT.Tests.Helpers.Data.Default.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;

namespace BAYSOFT.Tests.IntegrationTests.Samples
{
    [TestClass]
    public class PutSampleScenarios
    {
        [TestMethod]
        public async Task PUT_Samples_Should_Return_Ok()
        {
            var contextData = SamplesCollections.GetDefaultCollection();

            var data = new Sample { Id = 2, Description = "Sample - 002 [alt]" };

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var json = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(json);

                var response = await client.PutAsync($"/api/samples/{data.Id}", content);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
        [TestMethod]
        public async Task PUT_Samples_With_Same_Description_Should_Return_BadRequest()
        {
            var contextData = SamplesCollections.GetDefaultCollection();

            var data = new Sample { Id = 2, Description = "Sample - 001" };

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var json = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(json);

                var response = await client.PutAsync($"/api/samples/{data.Id}", content);

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
    }
}
