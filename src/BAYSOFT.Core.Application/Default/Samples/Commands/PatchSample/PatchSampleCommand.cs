using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Entities;
using FluentValidation;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.PatchSample
{
    public class PatchSampleCommand : ApplicationRequest<Sample, PatchSampleCommandResponse>
    {
        public PatchSampleCommand()
        {
            ConfigKeys(x => x.Id);

            Validator.RuleFor(x => x.Id).NotEqual(0).WithMessage("{0} is required!");
            Validator.RuleFor(x => x.Description).NotEmpty().WithMessage("{0} is required!");
        }
    }
}
