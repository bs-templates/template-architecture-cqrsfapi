using BAYSOFT.Abstractions.Core.Domain.Entities.Services;
using BAYSOFT.Core.Domain.Default.Samples.Entities;

namespace BAYSOFT.Core.Domain.Default.Samples.Services
{
    public class CreateSampleServiceRequest : DomainServiceRequest<Sample>
    {
        public CreateSampleServiceRequest(Sample payload) : base(payload)
        {
        }
    }
}