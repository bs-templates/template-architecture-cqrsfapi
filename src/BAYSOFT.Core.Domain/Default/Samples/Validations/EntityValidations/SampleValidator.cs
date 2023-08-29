using BAYSOFT.Abstractions.Core.Domain.Entities.Validations;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using FluentValidation;

namespace BAYSOFT.Core.Domain.Default.Samples.Validations.EntityValidations
{
    public class SampleValidator : EntityValidator<Sample>
    {
        public SampleValidator()
        {
            RuleFor(x => x.Description).NotNull().WithMessage("'{0}' cannot be null!");
            RuleFor(x => x.Description).NotEmpty().WithMessage("'{0}' cannot be empty!");
            RuleFor(x => x.Description).MinimumLength(3).WithMessage("'{3}' must have at least '{0}' characters!");
            RuleFor(x => x.Description).MaximumLength(512).WithMessage("'{3}' must have a maximum of '{1}' characters!");
        }
    }
}
