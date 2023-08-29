using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using FluentValidation;

namespace BAYSOFT.Core.Application.Default.Samples.Commands
{
    public class PutSampleCommand : ApplicationRequest<Sample, PutSampleCommandResponse>
    {
        public PutSampleCommand()
        {
            ConfigKeys(x => x.Id);

            ConfigSuppressedProperties(x => x.Id);
            // ConfigSuppressedResponseProperties(x => x.Id);

            Validator.RuleFor(x => x.Id).NotEqual(0).WithMessage("'{0}' is required!");
            Validator.RuleFor(x => x.Description).NotEmpty().WithMessage("'{0}' is required!");
        }
    }
}