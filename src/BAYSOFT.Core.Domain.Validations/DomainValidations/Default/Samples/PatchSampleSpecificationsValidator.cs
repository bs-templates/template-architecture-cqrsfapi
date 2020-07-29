using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Validations.Specifications.Default.Samples;
using NetDevPack.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Validations.DomainValidations.Default.Samples
{
    public class PatchSampleSpecificationsValidator : SpecValidator<Sample>
    {
        public PatchSampleSpecificationsValidator(
            SampleDescriptionAlreadyExistsSpecification sampleDescriptionAlreadyExistsSpecification
        )
        {
            base.Add("SanpleMustBeUnique", new Rule<Sample>(sampleDescriptionAlreadyExistsSpecification.Not(), "A register with this description already exists!"));
        }
    }
}
