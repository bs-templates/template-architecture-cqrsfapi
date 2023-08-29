using BAYSOFT.Abstractions.Core.Domain.Entities.Services;
using BAYSOFT.Core.Domain.Default.Samples.Entities;

namespace BAYSOFT.Core.Domain.Default.Samples.Services
{
    public class UpdateSampleServiceRequest : DomainServiceRequest<Sample>
    {
        public UpdateSampleServiceRequest(Sample payload) : base(payload)
        {
        }
    }
}