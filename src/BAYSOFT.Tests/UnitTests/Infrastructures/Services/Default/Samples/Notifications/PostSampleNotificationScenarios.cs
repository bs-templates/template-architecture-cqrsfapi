using BAYSOFT.Core.Application.Default.Samples.Notifications;
using BAYSOFT.Tests.Helpers;
using BAYSOFT.Tests.Helpers.Data.Default;
using BAYSOFT.Tests.Helpers.Data.Default.Samples;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BAYSOFT.Tests.UnitTests.Infrastructures.Services.Default.Samples.Notifications
{
    [TestClass]
    public class PostSampleNotificationScenarios
    {
        [TestMethod]
        public async Task PostSampleNotification_Should_Not_Return_Exception()
        {
            var contextData = SamplesCollections.GetDefaultCollection();

            using (var context = DefaultDbContextExtensions.GetInMemoryDefaultDbContext().SetupSamples(contextData))
            {
                var mockedLoggerFactory = GenericHelper.MockILoggerFactory<PostSampleNotificationHandler>();

                var mockedMediator = new Mock<IMediator>();

                var handler = new PostSampleNotificationHandler(
                    mockedLoggerFactory.Object,
                    mockedMediator.Object);

                var entity = contextData.First();

                var notification = new PostSampleNotification(entity);

                await handler.Handle(notification, default);
            }
        }
    }
}
