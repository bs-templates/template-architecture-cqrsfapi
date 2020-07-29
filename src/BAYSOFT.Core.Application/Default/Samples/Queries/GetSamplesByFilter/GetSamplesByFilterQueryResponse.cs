using ModelWrapper;
using BAYSOFT.Core.Domain.Entities.Default;

namespace BAYSOFT.Core.Application.Default.Samples.Queries.GetSamplesByFilter
{
    public class GetSamplesByFilterQueryResponse : ApplicationResponse<Sample>
    {
        public GetSamplesByFilterQueryResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
}
