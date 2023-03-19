using BAYSOFT.Abstractions.Core.Domain.Services;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Default.Validations.DomainValidations.Samples;
using BAYSOFT.Core.Domain.Default.Validations.EntityValidations;
using BAYSOFT.Core.Domain.Resources;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Domain.Default.Services.Samples
{
    public class UpdateSampleServiceRequest : IRequest<Sample>
    {
        public Sample Payload { get; set; }
        public UpdateSampleServiceRequest(Sample payload)
        {
            Payload = payload;
        }
    }

    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class UpdateSampleServiceRequestHandler : DomainService<Sample>, IRequestHandler<UpdateSampleServiceRequest, Sample>
    {
        private IDefaultDbContextWriter Writer { get; set; }
        public UpdateSampleServiceRequestHandler(
            IDefaultDbContextWriter writer,
            IStringLocalizer<UpdateSampleServiceRequestHandler> localizer,
            SampleValidator entityValidator,
            UpdateSampleSpecificationsValidator domainValidator
        ) : base(localizer, entityValidator, domainValidator)
        {
            Writer = writer;
        }

        public override Task Run(Sample entity)
        {
            ValidateEntity(entity);

            ValidateDomain(entity);

            return Task.CompletedTask;
        }

        public async Task<Sample> Handle(UpdateSampleServiceRequest request, CancellationToken cancellationToken)
        {
            await Run(request.Payload);

            return request.Payload;
        }
    }
}
