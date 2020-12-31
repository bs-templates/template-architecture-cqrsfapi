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
    public class PutSampleServiceTest
    {
        private PutSampleService GetMockedPutSampleService()
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

            var mockedPutSampleSpecificationsValidator = new PutSampleSpecificationsValidator(
                mockedSampleNameAlreadyExistsSpecification);

            var mockedPutSampleService = new PutSampleService(
                mockedDefaultDbContext.Object,
                mockedSampleValidator,
                mockedPutSampleSpecificationsValidator);

            return mockedPutSampleService;
        }

        [TestMethod]
        public async Task TestPutSampleWithEmptyModelAsync()
        {
            var mockedPutSampleService = GetMockedPutSampleService();

            var mockedSample = new Sample { };

            await Assert.ThrowsExceptionAsync<BusinessException>(() =>
                mockedPutSampleService.Run(mockedSample));
        }

        [TestMethod]
        public async Task TestPutSampleWithDuplicatedDescriptionOnSchoolAsync()
        {
            var mockedPutSampleService = GetMockedPutSampleService();

            var mockedSample = new Sample
            {
                Id = 1,
                Description = "Sample - 002"
            };

            await Assert.ThrowsExceptionAsync<BusinessException>(() =>
                mockedPutSampleService.Run(mockedSample));
        }

        [TestMethod]
        public async Task TestPutSampleValidModelAsync()
        {
            var mockedPutSampleService = GetMockedPutSampleService();

            var mockedSample = new Sample
            {
                Id = 1,
                Description = "Sample - 003",
            };

            await mockedPutSampleService.Run(mockedSample);
        }
    }
}
