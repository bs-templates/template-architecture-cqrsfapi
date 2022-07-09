using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Entities;
using FluentValidation;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.DeleteSample
{
    public class DeleteSampleCommand : ApplicationRequest<Sample, DeleteSampleCommandResponse>
    {
        public DeleteSampleCommand()
        {
            ConfigKeys(x => x.Id);

            ConfigSuppressedProperties(x => x.Description);

            Validator.RuleFor(x => x.Id).NotEqual(0).WithMessage("{0} is required!");
        }
    }
}
