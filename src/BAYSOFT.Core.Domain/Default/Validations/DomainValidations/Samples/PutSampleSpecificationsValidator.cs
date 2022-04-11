using BAYSOFT.Abstractions.Core.Domain.Validations;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Specifications.Samples;
using NetDevPack.Specification;

namespace BAYSOFT.Core.Domain.Default.Validations.DomainValidations.Samples
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
