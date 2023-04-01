using BAYSOFT.Core.Domain.Default.Entities;
using MediatR;

namespace BAYSOFT.Core.Domain.Default.Services.Samples
{
    public class CreateSampleRequest : IRequest<Sample>
    {
        public Sample Payload { get; set; }
        public CreateSampleRequest(Sample payload)
        {
            Payload = payload;
        }
    }
}