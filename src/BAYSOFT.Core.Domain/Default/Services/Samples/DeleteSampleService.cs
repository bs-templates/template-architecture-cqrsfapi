using BAYSOFT.Abstractions.Core.Domain.Services;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Interfaces.Services.Samples;
using BAYSOFT.Core.Domain.Default.Validations.DomainValidations.Samples;
using BAYSOFT.Core.Domain.Default.Validations.EntityValidations;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Default.Services.Samples
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
