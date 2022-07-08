using BAYSOFT.Core.Domain.Default.Notifications.Samples;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.NotificationHandlers
{
    public class PatchSampleNotificationHandler : INotificationHandler<PatchSampleNotification>
    {
        public ILoggerFactory Logger { get; private set; }
        public PatchSampleNotificationHandler(ILoggerFactory logger)
        {
            Logger = logger;
        }
        public async Task Handle(PatchSampleNotification notification, CancellationToken cancellationToken)
        {
            Logger.CreateLogger<PatchSampleNotificationHandler>().Log(LogLevel.Information, $"Sample patched! - Event Created At: {notification.CreatedAt:yyyy-MM-dd HH:mm:ss} Payload: {JsonSerializer.Serialize(notification.Payload)}");
        }
    }
}
