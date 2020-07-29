using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using NetDevPack.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Validations.Specifications.Default.Samples
{
    public class SampleDescriptionAlreadyExistsSpecification : Specification<Sample>
    {
        private IDefaultDbContextQuery Context { get; set; }
        public SampleDescriptionAlreadyExistsSpecification(IDefaultDbContextQuery context)
        {
            Context = context;
        }

        public override Expression<Func<Sample, bool>> ToExpression()
        {
            return sample => Context.Samples.Any(x => x.Description == sample.Description && x.SampleID != sample.SampleID);
        }
    }
}
