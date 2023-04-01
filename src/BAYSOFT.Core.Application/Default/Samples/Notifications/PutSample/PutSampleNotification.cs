using BAYSOFT.Core.Domain.Default.Entities;
using MediatR;
using System;

namespace BAYSOFT.Core.Application.Default.Samples.Notifications
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