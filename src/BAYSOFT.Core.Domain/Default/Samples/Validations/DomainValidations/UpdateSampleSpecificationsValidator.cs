using BAYSOFT.Abstractions.Core.Domain.Entities.Validations;
using BAYSOFT.Core.Domain.Default.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Samples.Specifications;
using NetDevPack.Specification;

namespace BAYSOFT.Core.Domain.Default.Samples.Validations.DomainValidations
{
    public class UpdateSampleSpecificationsValidator : DomainValidator<Sample>
    {
        public UpdateSampleSpecificationsValidator(
            SampleDescriptionAlreadyExistsSpecification sampleDescriptionAlreadyExistsSpecification
        )
        {
            Add("sampleDescriptionAlreadyExistsSpecification", new DomainRule<Sample>(sampleDescriptionAlreadyExistsSpecification.Not(), sampleDescriptionAlreadyExistsSpecification.ToString()));
        }
    }
}
