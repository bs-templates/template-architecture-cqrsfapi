using BAYSOFT.Core.Domain.Default.Entities;
using MediatR;
using System;

namespace BAYSOFT.Core.Domain.Default.Notifications.Samples
{
    public class PatchSampleNotification : INotification
    {
        public Sample Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public PatchSampleNotification(Sample payload)
        {
            Payload = payload;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
