using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Entities;
using FluentValidation;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.PutSample
{
    public class PutSampleCommand : ApplicationRequest<Sample, PutSampleCommandResponse>
    {
        public PutSampleCommand()
        {
            ConfigKeys(x => x.Id);

            // Configures supressed properties & response properties
            Validator.RuleFor(x => x.Id).NotEqual(0).WithMessage("{0} is required!");
            Validator.RuleFor(x => x.Description).NotEmpty().WithMessage("{0} is required!");
        }
    }
}
