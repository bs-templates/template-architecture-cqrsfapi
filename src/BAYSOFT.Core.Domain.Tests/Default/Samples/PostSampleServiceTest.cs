using BAYSOFT.Abstractions.Core.Domain.Exceptions;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Services.Samples;
using BAYSOFT.Core.Domain.Default.Validations.DomainValidations.Samples;
using BAYSOFT.Core.Domain.Default.Validations.EntityValidations;
using BAYSOFT.Core.Domain.Default.Specifications.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Tests.Default.Samples
{
    [TestClass]
    public class PostSampleServiceTest
    {
        private PostSampleService GetMockedPostSampleService()
        {
            var mockedWriter = MockDefaultHelper
                .GetMockedDefaultDbContextWriter()
                .AddMockedSamples();

            var mockedReader = MockDefaultHelper
                .GetMockedDefaultDbContextReader()
                .AddMockedSamples();

            var mockedSampleValidator = new SampleValidator();

            var mockedSampleNameAlreadyExistsSpecification = new SampleDescriptionAlreadyExistsSpecification(
                mockedReader.Object);

            var mockedPostSampleSpecificationsValidator = new PostSampleSpecificationsValidator(
                mockedSampleNameAlreadyExistsSpecification);

            var mockedPostSampleService = new PostSampleService(
                mockedWriter.Object,
                mockedSampleValidator,
                mockedPostSampleSpecificationsValidator);

            return mockedPostSampleService;
        }

        [TestMethod]
        public async Task TestPostSampleWithEmptyModelAsync()
        {
            var mockedPostSampleService = GetMockedPostSampleService();

            var mockedSample = new Sample { };

            await Assert.ThrowsExceptionAsync<BusinessException>(() =>
                mockedPostSampleService.Run(mockedSample));
        }

        [TestMethod]
        public async Task TestPostSampleWithDuplicatedDescriptionOnSchoolAsync()
        {
            var mockedPostSampleService = GetMockedPostSampleService();

            var mockedSample = new Sample
            {
                Description = "Sample - 002",
            };

            await Assert.ThrowsExceptionAsync<BusinessException>(() =>
                mockedPostSampleService.Run(mockedSample));
        }

        [TestMethod]
        public async Task TestPostSampleValidModelAsync()
        {
            var mockedPostSampleService = GetMockedPostSampleService();

            var mockedSample = new Sample
            {
                Description = "Sample - 003",
            };

            await mockedPostSampleService.Run(mockedSample);
        }
    }
}
