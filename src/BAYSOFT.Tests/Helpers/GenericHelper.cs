using BAYSOFT.Core.Application.Default.Samples.Notifications;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace BAYSOFT.Tests.Helpers
{
    public static class GenericHelper
    {
        public static IStringLocalizer<TService> CreateLocalizer<TService>() where TService : class
        {
            var options = Options.Create(new LocalizationOptions { });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            var localizer = new StringLocalizer<TService>(factory);

            return localizer;
        }

        public static Mock<ILoggerFactory> MockILoggerFactory<TClass>() where TClass : class
        {
            var mockedLogger = new Mock<ILogger<DeleteSampleNotificationHandler>>();
            mockedLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<object, Exception?, string>>()));

            var mockedLoggerFactory = new Mock<ILoggerFactory>();
            mockedLoggerFactory
                .Setup(mock => mock.CreateLogger(It.IsAny<string>()))
                .Returns(mockedLogger.Object);

            return mockedLoggerFactory;
        }
    }
}
