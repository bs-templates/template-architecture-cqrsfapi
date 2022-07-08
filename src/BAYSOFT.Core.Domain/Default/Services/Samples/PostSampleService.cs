﻿using BAYSOFT.Abstractions.Core.Domain.Services;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Interfaces.Services.Samples;
using BAYSOFT.Core.Domain.Default.Validations.DomainValidations.Samples;
using BAYSOFT.Core.Domain.Default.Validations.EntityValidations;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Default.Services.Samples
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
