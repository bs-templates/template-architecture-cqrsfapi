using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Entities.Default;

namespace BAYSOFT.Core.Application.Default.Samples.Queries.GetSampleById
{
    public class GetSampleByIdQuery : ApplicationRequest<Sample, GetSampleByIdQueryResponse>
    {
        public GetSampleByIdQuery()
        {
            ConfigKeys(x => x.Id);
            
            // Configures supressed properties & response properties
            //ConfigSuppressedProperties(x => x);
            //ConfigSuppressedResponseProperties(x => x);  
        }
    }
}
