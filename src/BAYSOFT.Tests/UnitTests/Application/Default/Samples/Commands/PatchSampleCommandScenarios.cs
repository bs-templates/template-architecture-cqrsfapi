using BAYSOFT.Core.Application.Default.Samples.Commands;
using BAYSOFT.Core.Domain.Default.Samples.Services.CreateSample;
using BAYSOFT.Infrastructures.Data.Default;
using BAYSOFT.Tests.Helpers;
using BAYSOFT.Tests.Helpers.Data.Default;
using BAYSOFT.Tests.Helpers.Data.Default.Samples;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;

namespace BAYSOFT.Tests.UnitTests.Application.Default.Samples.Commands
{
    [TestClass]
    public class PatchSampleCommandScenarios
    {
        [TestMethod]
        public async Task PATCH_Sample_Should_Return_Ok()
        {
            var contextData = SamplesCollections.GetDefaultCollection();

            using (var context = DefaultDbContextExtensions.GetInMemoryDefaultDbContext().SetupSamples(contextData))
            {
                var writer = new DefaultDbContextWriter(context);

                var mockedLogger = new Mock<ILoggerFactory>();

                var mockedMediator = new Mock<IMediator>();

                var localizer = GenericHelper.CreateLocalizer<PatchSampleCommandHandler>();

                var handler = new PatchSampleCommandHandler(
                    mockedLogger.Object,
                    mockedMediator.Object,
                    localizer,
                    writer);

                var command = new PatchSampleCommand();

                command.Project(model =>
                {
                    model.Id = 1;
                    model.Description = "Sample - 001 [patch]";
                });

                var result = await handler.Handle(command, default);

                Assert.AreEqual((long)HttpStatusCode.OK, result.StatusCode);
            }
        }
    }
}
