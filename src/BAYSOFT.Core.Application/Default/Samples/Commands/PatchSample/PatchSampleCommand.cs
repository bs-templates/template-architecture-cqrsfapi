using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Entities.Default;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.PatchSample
{
    public class PatchSampleCommand : ApplicationRequest<Sample, PatchSampleCommandResponse>
    {
        public PatchSampleCommand()
        {
            ConfigKeys(x => x.Id);

            // Configures supressed properties & response properties
            //ConfigSuppressedProperties(x => x);
            //ConfigSuppressedResponseProperties(x => x);
        }
    }
}
