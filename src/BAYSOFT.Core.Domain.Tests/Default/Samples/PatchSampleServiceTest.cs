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
    public class PatchSampleServiceTest
    {
        private PatchSampleService GetMockedPatchSampleService()
        {
            var mockedWriter = MockDefaultHelper
                .GetMockedDefaultDbContextWriter()
                .AddMockedSamples();

            var mockedReader = MockDefaultHelper
                .GetMockedDefaultDbContextReader()
                .AddMockedSamples();

            var mockedSampleValidator = new SampleValidator();

            var sampleDescriptionAlreadyExistsSpecification = new SampleDescriptionAlreadyExistsSpecification(
                mockedReader.Object);

            var mockedPatchSampleSpecificationsValidator = new PatchSampleSpecificationsValidator(
                sampleDescriptionAlreadyExistsSpecification);

            var mockedPatchSampleService = new PatchSampleService(
                mockedWriter.Object,
                mockedSampleValidator,
                mockedPatchSampleSpecificationsValidator
                );

            return mockedPatchSampleService;
        }

        [TestMethod]
        public async Task TestPatchSampleWithEmptyModelAsync()
        {
            var mockedPatchSampleService = GetMockedPatchSampleService();

            var mockedSample = new Sample { };

            await Assert.ThrowsExceptionAsync<BusinessException>(() =>
                mockedPatchSampleService.Run(mockedSample));
        }

        [TestMethod]
        public async Task TestPatchSampleWithDuplicatedDescriptionOnSchoolAsync()
        {
            var mockedPatchSampleService = GetMockedPatchSampleService();

            var mockedSample = new Sample
            {
                Id = 1,
                Description = "Sample - 002"
            };

            await Assert.ThrowsExceptionAsync<BusinessException>(() =>
                mockedPatchSampleService.Run(mockedSample));
        }

        [TestMethod]
        public async Task TestPatchSampleValidModelAsync()
        {
            var mockedPatchSampleService = GetMockedPatchSampleService();

            var mockedSample = new Sample
            {
                Id = 1,
                Description = "Sample - 003"
            };

            await mockedPatchSampleService.Run(mockedSample);
        }
    }
}
