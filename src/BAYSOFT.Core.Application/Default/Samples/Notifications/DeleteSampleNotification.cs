using BAYSOFT.Core.Domain.Default.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Notifications
{
    public class DeleteSampleNotification : INotification
    {
        public Sample Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public DeleteSampleNotification(Sample payload)
        {
            Payload = payload;
            CreatedAt = DateTime.UtcNow;
        }
    }
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
