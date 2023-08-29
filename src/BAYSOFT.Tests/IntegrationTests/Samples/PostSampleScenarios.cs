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
    public class PostSampleScenarios
    {
        [TestMethod]
        public async Task POST_Samples_Should_Return_Ok()
        {
            var contextData = SamplesCollections.GetDefaultCollection();

            var data = new Sample { Description = "Sample - 001 [new]" };

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var json = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(json);

                var response = await client.PostAsync($"/api/samples", content);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
        [TestMethod]
        public async Task POST_Samples_With_Same_Description_Should_Return_BadRequest()
        {
            var contextData = SamplesCollections.GetDefaultCollection();

            var data = new Sample { Description = "Sample - 002" };

            //var mock = Mock.Of<IEmailService>();
            //Mock.Get(mock)
            //    .Setup(x => x.GetCharacterName())
            //    .ReturnsAsync("Skywalker, Luke");

            //Action<IServiceCollection> services = s => { s.RemoveAll<IEmailService>(); s.TryAddTransient<IEmailService>(sf => mock); };

            //using (var client = ServerHelper.Create(services).SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var json = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(json);

                var response = await client.PostAsync($"/api/samples", content);

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
    }
}
