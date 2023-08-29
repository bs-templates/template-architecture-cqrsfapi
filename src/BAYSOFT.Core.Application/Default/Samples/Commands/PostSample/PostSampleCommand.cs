using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using FluentValidation;

namespace BAYSOFT.Core.Application.Default.Samples.Commands
{
    public class PostSampleCommand : ApplicationRequest<Sample, PostSampleCommandResponse>
    {
        public PostSampleCommand()
        {
            ConfigKeys(x => x.Id);

            ConfigSuppressedResponseProperties(x => x.Id);

            Validator.RuleFor(x => x.Description).NotEmpty().WithMessage("'{0}' is required!");
        }
    }
}