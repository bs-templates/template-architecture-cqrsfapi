﻿using BAYSOFT.Core.Domain.Default.Samples.Entities;
using MediatR;
using System;

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
}