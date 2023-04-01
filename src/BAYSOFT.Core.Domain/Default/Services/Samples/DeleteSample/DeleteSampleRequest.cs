using BAYSOFT.Core.Domain.Default.Entities;
using MediatR;

namespace BAYSOFT.Core.Domain.Default.Services.Samples
{
    public class DeleteSampleRequest : IRequest<Sample>
    {
        public Sample Payload { get; set; }
        public DeleteSampleRequest(Sample payload)
        {
            Payload = payload;
        }
    }
}