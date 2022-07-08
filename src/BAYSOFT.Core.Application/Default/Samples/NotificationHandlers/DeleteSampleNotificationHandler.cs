using BAYSOFT.Core.Domain.Default.Notifications.Samples;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.NotificationHandlers
{
    public class DeleteSampleNotificationHandler : INotificationHandler<DeleteSampleNotification>
    {
        public ILoggerFactory Logger { get; private set; }
        public DeleteSampleNotificationHandler(ILoggerFactory logger)
        {
            Logger = logger;
        }
        public async Task Handle(DeleteSampleNotification notification, CancellationToken cancellationToken)
        {
            Logger.CreateLogger<DeleteSampleNotificationHandler>().Log(LogLevel.Information, $"Sample deleted! - Event Created At: {notification.CreatedAt:yyyy-MM-dd HH:mm:ss} Payload: {JsonSerializer.Serialize(notification.Payload)}");
        }
    }
}
