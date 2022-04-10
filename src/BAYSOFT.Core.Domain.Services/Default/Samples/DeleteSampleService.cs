using BAYSOFT.Abstractions.Core.Domain.Services;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Default;
using BAYSOFT.Core.Domain.Interfaces.Services.Default.Samples;
using BAYSOFT.Core.Domain.Validations.DomainValidations.Default.Samples;
using BAYSOFT.Core.Domain.Validations.EntityValidations.Default;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Services.Default.Samples
{
    public class DeleteSampleService : DomainService<Sample>,IDeleteSampleService
    {
        private IDefaultDbContextWriter Writer { get; set; }
        public DeleteSampleService(
            IDefaultDbContextWriter writer,
            SampleValidator entityValidator,
            DeleteSampleSpecificationsValidator domainValidator
        ) : base(entityValidator, domainValidator)
        {
            Writer = writer;
        }

        public override Task Run(Sample entity)
        {
            ValidateEntity(entity);

            ValidateDomain(entity);

            Writer.Remove(entity);

            return Task.CompletedTask;
        }
    }
}
