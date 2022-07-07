using BAYSOFT.Core.Domain.Default.Notifications.Samples;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.NotificationHandlers
{
    public class PostSampleNotificationHandler : INotificationHandler<PostSampleNotification>
    {
        public ILoggerFactory Logger { get; private set; }
        public PostSampleNotificationHandler(ILoggerFactory logger)
        {
            Logger = logger;
        }
        public async Task Handle(PostSampleNotification notification, CancellationToken cancellationToken)
        {
            Logger.CreateLogger<PostSampleNotificationHandler>().Log(LogLevel.Information, $"Sample posted! - Event Created At: {notification.CreatedAt:yyyy-MM-dd HH:mm:ss} Payload: {JsonSerializer.Serialize(notification.Payload)}");

            notification.Payload.Description = $"Sample {notification.CreatedAt:yyyy-MM-dd HH:mm:ss}";

            await Task.Delay(1000);
        }
    }
}
