using BAYSOFT.Abstractions.Core.Domain.Exceptions;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Services.Default.Samples;
using BAYSOFT.Core.Domain.Validations.DomainValidations.Default.Samples;
using BAYSOFT.Core.Domain.Validations.EntityValidations.Default;
using BAYSOFT.Core.Domain.Validations.Specifications.Default.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Services.Tests.Default.Samples
{
    [TestClass]
    public class PostSampleServiceTest
    {
        private PostSampleService GetMockedPostSampleService()
        {
            var mockedDefaultDbContext = MockDefaultHelper
                .GetMockedDbContext()
                .AddMockedSamples();

            var mockedDefaultDbContextQuery = MockDefaultHelper
                .GetMockedDbContextQuery()
                .AddMockedSamples();

            var mockedSampleValidator = new SampleValidator();

            var mockedSampleNameAlreadyExistsSpecification = new SampleDescriptionAlreadyExistsSpecification(
                mockedDefaultDbContextQuery.Object);

            var mockedPostSampleSpecificationsValidator = new PostSampleSpecificationsValidator(
                mockedSampleNameAlreadyExistsSpecification);

            var mockedPostSampleService = new PostSampleService(
                mockedDefaultDbContext.Object,
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
