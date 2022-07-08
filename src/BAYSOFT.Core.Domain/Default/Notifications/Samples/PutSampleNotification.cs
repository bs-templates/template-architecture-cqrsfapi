using BAYSOFT.Core.Domain.Default.Entities;
using MediatR;
using System;

namespace BAYSOFT.Core.Domain.Default.Notifications.Samples
{
    public class PutSampleNotification : INotification
    {
        public Sample Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public PutSampleNotification(Sample payload)
        {
            Payload = payload;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
