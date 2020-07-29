using ModelWrapper;
using BAYSOFT.Core.Domain.Entities.Default;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.PostSample
{
    public class PostSampleCommandResponse : ApplicationResponse<Sample>
    {
        public PostSampleCommandResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
}
