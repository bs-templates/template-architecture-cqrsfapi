using BAYSOFT.Core.Application.Default.Samples.Commands;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Samples.Services;
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
    public class PostSampleCommandScenarios
    {
        [TestMethod]
        public async Task POST_Sample_Should_Return_Ok()
        {
            var contextData = SamplesCollections.GetDefaultCollection();

            using (var context = DefaultDbContextExtensions.GetInMemoryDefaultDbContext().SetupSamples(contextData))
            {
                var writer = new DefaultDbContextWriter(context);

                var mockedLogger = new Mock<ILoggerFactory>();

                var mockedMediator = new Mock<IMediator>();

                var localizer = GenericHelper.CreateLocalizer<PostSampleCommandHandler>();

                var handler = new PostSampleCommandHandler(
                    mockedLogger.Object,
                    mockedMediator.Object,
                    localizer,
                    writer);

                var command = new PostSampleCommand();

                command.Project(model =>
                {
                    model.Description = "Sample - 001";
                });

                var result = await handler.Handle(command, default);

                Assert.AreEqual((long)HttpStatusCode.OK, result.StatusCode);
            }
        }
    }
}
