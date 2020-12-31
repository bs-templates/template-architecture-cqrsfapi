using ModelWrapper;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Abstractions.Core.Application;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.DeleteSample
{
    public class DeleteSampleCommandResponse : ApplicationResponse<Sample>
    {
        public DeleteSampleCommandResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
}
