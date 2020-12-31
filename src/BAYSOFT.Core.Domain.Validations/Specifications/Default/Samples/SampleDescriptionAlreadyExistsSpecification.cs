using BAYSOFT.Abstractions.Core.Domain.Validations;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BAYSOFT.Core.Domain.Validations.Specifications.Default.Samples
{
    public class SampleDescriptionAlreadyExistsSpecification : DomainSpecification<Sample>
    {
        private IDefaultDbContextQuery Context { get; set; }
        public SampleDescriptionAlreadyExistsSpecification(IDefaultDbContextQuery context)
        {
            Context = context;
        }

        public override Expression<Func<Sample, bool>> ToExpression()
        {
            return sample => Context.Samples.Any(x => x.Description == sample.Description && x.Id != sample.Id);
        }
    }
}
