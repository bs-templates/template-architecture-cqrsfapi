using BAYSOFT.Abstractions.Core.Domain.Validations;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Validations.Specifications.Default.Samples;

namespace BAYSOFT.Core.Domain.Validations.DomainValidations.Default.Samples
{
    public class PatchSampleSpecificationsValidator : DomainValidator<Sample>
    {
        public PatchSampleSpecificationsValidator(
            SampleDescriptionAlreadyExistsSpecification sampleDescriptionAlreadyExistsSpecification
        )
        {
            base.Add("SanpleMustBeUnique", new DomainRule<Sample>(sampleDescriptionAlreadyExistsSpecification.Not(), "A register with this description already exists!"));
        }
    }
}
