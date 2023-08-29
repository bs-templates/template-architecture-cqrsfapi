using BAYSOFT.Core.Application.Default.Samples.Queries;
using BAYSOFT.Infrastructures.Data.Default;
using BAYSOFT.Tests.Helpers;
using BAYSOFT.Tests.Helpers.Data.Default;
using BAYSOFT.Tests.Helpers.Data.Default.Samples;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;

namespace BAYSOFT.Tests.UnitTests.Application.Default.Samples.Queries
{
    [TestClass]
    public class GetSamplesByFilterQueryScenarios
    {
        [TestMethod]
        public async Task GET_Samples_By_Filter_Should_Return_Ok()
        {
            var contextData = SamplesCollections.GetDefaultCollection();

            using (var context = DefaultDbContextExtensions.GetInMemoryDefaultDbContext().SetupSamples(contextData))
            {
                var reader = new DefaultDbContextReader(context);

                var mockedLogger = new Mock<ILoggerFactory>();

                var mockedMediator = new Mock<IMediator>();

                var localizer = GenericHelper.CreateLocalizer<GetSamplesByFilterQueryHandler>();

                var handler = new GetSamplesByFilterQueryHandler(
                    mockedLogger.Object,
                    mockedMediator.Object,
                    localizer,
                    reader);

                var command = new GetSamplesByFilterQuery();

                var result = await handler.Handle(command, default);

                Assert.AreEqual((long)HttpStatusCode.OK, result.StatusCode);
                Assert.AreEqual(contextData.Count, result.ResultCount);
            }
        }
    }
}
