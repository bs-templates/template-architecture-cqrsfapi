using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Entities.Default;
using ModelWrapper;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.PatchSample
{
    public class PatchSampleCommandResponse : ApplicationResponse<Sample>
    {
        public PatchSampleCommandResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
}
