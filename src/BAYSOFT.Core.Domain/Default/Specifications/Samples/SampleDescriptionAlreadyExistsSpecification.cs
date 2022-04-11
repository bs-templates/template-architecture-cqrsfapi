using BAYSOFT.Abstractions.Core.Domain.Validations;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BAYSOFT.Core.Domain.Default.Specifications.Samples
{
    public class SampleDescriptionAlreadyExistsSpecification : DomainSpecification<Sample>
    {
        private IDefaultDbContextReader Reader { get; set; }
        public SampleDescriptionAlreadyExistsSpecification(IDefaultDbContextReader reader)
        {
            Reader = reader;
        }

        public override Expression<Func<Sample, bool>> ToExpression()
        {
            return sample => Reader.Query<Sample>().Any(x => x.Description == sample.Description && x.Id != sample.Id);
        }
    }
}
