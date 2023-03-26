using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Services;
using BAYSOFT.Infrastructures.Data.Contexts;
using BAYSOFT.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace BAYSOFT.Tests.IntegrationTests.Samples
{
    [TestClass]
    public class SamplesScenarios
    {
        [TestMethod]
        public async Task GET_Samples_Should_Return_Ok()
        {
            var contextData = new List<Sample>{
                new Sample { Id = 1, Description = "Sample 01" },
                new Sample { Id = 2, Description = "Sample 02" }
            };

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var response = await client.GetAsync($"/api/samples");

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
        [TestMethod]
        public async Task GET_Sample_By_Id_Should_Return_Ok()
        {
            var contextData = new List<Sample>{
                new Sample { Id = 1, Description = "Sample 01" },
                new Sample { Id = 2, Description = "Sample 02" }
            };

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var response = await client.GetAsync($"/api/samples/1");

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
        [TestMethod]
        public async Task POST_Samples_Should_Return_Ok()
        {
            var contextData = new List<Sample>{
                new Sample { Id = 1, Description = "Sample 01" },
                new Sample { Id = 2, Description = "Sample 02" }
            };

            var data = new Sample { Description = "Sample 03" };

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
            var contextData = new List<Sample>{
                new Sample { Id = 1, Description = "Sample 01" },
                new Sample { Id = 2, Description = "Sample 02" }
            };

            var data = new Sample { Description = "Sample 02" };

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
        [TestMethod]
        public async Task PUT_Samples_Should_Return_Ok()
        {
            var contextData = new List<Sample>{
                new Sample { Id = 1, Description = "Sample 01" },
                new Sample { Id = 2, Description = "Sample 02" }
            };

            var data = new Sample { Id = 2, Description = "Sample 02 [alt]" };

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
            var contextData = new List<Sample>{
                new Sample { Id = 1, Description = "Sample 01" },
                new Sample { Id = 2, Description = "Sample 02" }
            };

            var data = new Sample { Id = 2, Description = "Sample 01" };

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var json = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(json);

                var response = await client.PutAsync($"/api/samples/{data.Id}", content);

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
        [TestMethod]
        public async Task PATCH_Samples_Should_Return_Ok()
        {
            var contextData = new List<Sample>{
                new Sample { Id = 1, Description = "Sample 01" },
                new Sample { Id = 2, Description = "Sample 02" }
            };

            var data = new Sample { Id = 2, Description = "Sample 02 [alt]" };

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var json = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(json);

                var response = await client.PatchAsync($"/api/samples/{data.Id}", content);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
        [TestMethod]
        public async Task PATCH_Samples_With_Same_Description_Should_Return_BadRequest()
        {
            var contextData = new List<Sample>{
                new Sample { Id = 1, Description = "Sample 01" },
                new Sample { Id = 2, Description = "Sample 02" }
            };

            var data = new Sample { Id = 2, Description = "Sample 01" };

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var json = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(json);

                var response = await client.PatchAsync($"/api/samples/{data.Id}", content);

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
        [TestMethod]
        public async Task DELETE_Samples_Should_Return_Ok()
        {
            var contextData = new List<Sample>{
                new Sample { Id = 1, Description = "Sample 01" },
                new Sample { Id = 2, Description = "Sample 02" }
            };

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var response = await client.DeleteAsync($"/api/samples/2");

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
        [TestMethod]
        public async Task DELETE_Samples_Where_Sample_Doesnt_Exists_Should_Return_NotFound()
        {
            var contextData = new List<Sample>{
                new Sample { Id = 1, Description = "Sample 01" },
            };

            using (var client = ServerHelper.Create().SetupData<DefaultDbContext, Sample>(contextData).CreateClient())
            {
                var response = await client.DeleteAsync($"/api/samples/2");

                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
