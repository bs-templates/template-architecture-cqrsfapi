using BAYSOFT.Abstractions.Core.Domain.Entities.Services;
using BAYSOFT.Core.Domain.Default.Samples.Entities;

namespace BAYSOFT.Core.Domain.Default.Samples.Services
{
    public class DeleteSampleServiceRequest : DomainServiceRequest<Sample>
    {
        public DeleteSampleServiceRequest(Sample payload) : base(payload)
        {
        }
    }
}