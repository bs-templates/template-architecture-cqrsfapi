using BAYSOFT.Abstractions.Core.Domain.Services;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using BAYSOFT.Core.Domain.Interfaces.Services.Default.Samples;
using BAYSOFT.Core.Domain.Validations.DomainValidations.Default.Samples;
using BAYSOFT.Core.Domain.Validations.EntityValidations.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Services.Default.Samples
{
    public class PatchSampleService : DomainService<Sample>, IPatchSampleService
    {
        private IDefaultDbContext Context { get; set; }
        public PatchSampleService(
            IDefaultDbContext context,
            SampleValidator entityValidator,
            PatchSampleSpecificationsValidator domainValidator
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
