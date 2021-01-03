using ModelWrapper;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Abstractions.Core.Application;

namespace BAYSOFT.Core.Application.Default.Samples.Queries.GetSampleById
{
    public class GetSampleByIdQueryResponse : ApplicationResponse<Sample>
    {
        public GetSampleByIdQueryResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
}
