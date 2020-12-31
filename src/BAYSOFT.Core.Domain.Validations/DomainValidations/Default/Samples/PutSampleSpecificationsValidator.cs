using BAYSOFT.Abstractions.Core.Domain.Validations;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Validations.Specifications.Default.Samples;
using NetDevPack.Specification;

namespace BAYSOFT.Core.Domain.Validations.DomainValidations.Default.Samples
{
    public class PutSampleSpecificationsValidator : DomainValidator<Sample>
    {
        public PutSampleSpecificationsValidator(
            SampleDescriptionAlreadyExistsSpecification sampleDescriptionAlreadyExistsSpecification
        )
        {
            base.Add("SanpleMustBeUnique", new Rule<Sample>(sampleDescriptionAlreadyExistsSpecification.Not(), "A register with this description already exists!"));
        }
    }
}
