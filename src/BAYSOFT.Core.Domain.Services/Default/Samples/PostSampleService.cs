using BAYSOFT.Abstractions.Core.Domain.Services;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Default;
using BAYSOFT.Core.Domain.Interfaces.Services.Default.Samples;
using BAYSOFT.Core.Domain.Validations.DomainValidations.Default.Samples;
using BAYSOFT.Core.Domain.Validations.EntityValidations.Default;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Services.Default.Samples
{
    public class PostSampleService : DomainService<Sample>, IPostSampleService
    {
        private IDefaultDbContextWriter Writer { get; set; }
        public PostSampleService(
            IDefaultDbContextWriter writer,
            SampleValidator entityValidator,
            PostSampleSpecificationsValidator domainValidator
        ) : base(entityValidator, domainValidator)
        {
            Writer = writer;
        }
        public override async Task Run(Sample entity)
        {
            ValidateEntity(entity);

            ValidateDomain(entity);

            await Writer.AddAsync(entity);
        }
    }
}
