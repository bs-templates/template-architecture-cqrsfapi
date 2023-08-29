using System;

namespace BAYSOFT.Core.Domain.Models
{
    public class Message<TPayload>
        where TPayload : class
    {
        public Guid Id { get; private set; } 
        public string MessageType { get; private set; }
        public TPayload Payload { get; private set; }
        public Message(TPayload payload)
        {
            Id = Guid.NewGuid();
            MessageType = typeof(TPayload).FullName;
            Payload = payload;
        }
    }
}
