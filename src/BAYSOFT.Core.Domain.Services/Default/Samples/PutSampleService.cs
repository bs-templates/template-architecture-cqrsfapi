using BAYSOFT.Abstractions.Core.Domain.Services;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Default;
using BAYSOFT.Core.Domain.Interfaces.Services.Default.Samples;
using BAYSOFT.Core.Domain.Validations.DomainValidations.Default.Samples;
using BAYSOFT.Core.Domain.Validations.EntityValidations.Default;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Services.Default.Samples
{
    public class PutSampleService : DomainService<Sample>, IPutSampleService
    {
        private IDefaultDbContextWriter Writer { get; set; }
        public PutSampleService(
            IDefaultDbContextWriter writer,
            SampleValidator entityValidator,
            PutSampleSpecificationsValidator domainValidator
        ) : base(entityValidator, domainValidator)
        {
            Writer = writer;
        }

        public override Task Run(Sample entity)
        {
            ValidateEntity(entity);

            ValidateDomain(entity);

            return Task.CompletedTask;
        }
    }
}
