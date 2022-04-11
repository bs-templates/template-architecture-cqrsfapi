using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Services.Samples;
using BAYSOFT.Core.Domain.Default.Validations.DomainValidations.Samples;
using BAYSOFT.Core.Domain.Default.Validations.EntityValidations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Tests.Default.Samples
{
    [TestClass]
    public class DeleteSampleServiceTest
    {
        private DeleteSampleService GetMockedDeleteSampleService()
        {
            var mockedWriter = MockDefaultHelper
                .GetMockedDefaultDbContextWriter()
                .AddMockedSamples();

            var mockedSampleValidator = new SampleValidator();

            var mockedDeleteSampleSpecificationsValidator = new DeleteSampleSpecificationsValidator();

            var mockedDeleteSampleService = new DeleteSampleService(
                mockedWriter.Object,
                mockedSampleValidator,
                mockedDeleteSampleSpecificationsValidator
                );

            return mockedDeleteSampleService;
        }

        [TestMethod]
        public async Task TestDeleteSampleValidModelAsync()
        {
            var mockedDeleteSampleService = GetMockedDeleteSampleService();

            var mockedSample = new Sample
            {
                Id = 1,
                Description = "Sample - 001"
            };

            await mockedDeleteSampleService.Run(mockedSample);
        }
    }
}
