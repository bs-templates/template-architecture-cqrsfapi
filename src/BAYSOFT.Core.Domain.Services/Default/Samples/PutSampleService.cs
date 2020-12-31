using BAYSOFT.Abstractions.Core.Domain.Services;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using BAYSOFT.Core.Domain.Interfaces.Services.Default.Samples;
using BAYSOFT.Core.Domain.Validations.DomainValidations.Default.Samples;
using BAYSOFT.Core.Domain.Validations.EntityValidations.Default;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Services.Default.Samples
{
    public class PutSampleService : DomainService<Sample>, IPutSampleService
    {
        private IDefaultDbContext Context { get; set; }
        public PutSampleService(
            IDefaultDbContext context,
            SampleValidator entityValidator,
            PutSampleSpecificationsValidator domainValidator
        ) : base(entityValidator, domainValidator)
        {
            Context = context;
        }

        public override Task Run(Sample entity)
        {
            ValidateEntity(entity);

            ValidateDomain(entity);

            return Task.CompletedTask;
        }
    }
}
