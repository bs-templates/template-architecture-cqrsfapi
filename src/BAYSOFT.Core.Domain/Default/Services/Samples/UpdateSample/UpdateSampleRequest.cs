using BAYSOFT.Core.Domain.Default.Entities;
using MediatR;

namespace BAYSOFT.Core.Domain.Default.Services.Samples
{
    public class UpdateSampleRequest : IRequest<Sample>
    {
        public Sample Payload { get; set; }
        public UpdateSampleRequest(Sample payload)
        {
            Payload = payload;
        }
    }
}