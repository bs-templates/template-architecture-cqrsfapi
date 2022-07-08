using BAYSOFT.Core.Domain.Default.Entities;
using MediatR;
using System;

namespace BAYSOFT.Core.Domain.Default.Notifications.Samples
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
}
