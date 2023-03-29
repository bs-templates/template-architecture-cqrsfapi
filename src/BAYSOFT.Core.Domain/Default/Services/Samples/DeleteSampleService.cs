using BAYSOFT.Abstractions.Core.Domain.Interfaces.Services;
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
    public class DeleteSampleServiceRequest : IRequest<Sample>
    {
        public Sample Payload { get; set; }
        public DeleteSampleServiceRequest(Sample payload)
        {
            Payload = payload;
        }
    }

    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class DeleteSampleServiceRequestHandler : DomainService<Sample, DeleteSampleServiceRequest>, IDomainService<Sample, DeleteSampleServiceRequest>
    {
        private IDefaultDbContextWriter Writer { get; set; }
        public DeleteSampleServiceRequestHandler(
            IDefaultDbContextWriter writer,
            IStringLocalizer<DeleteSampleServiceRequestHandler> localizer,
            SampleValidator entityValidator,
            DeleteSampleSpecificationsValidator domainValidator
        ) : base(localizer, entityValidator, domainValidator)
        {
            Writer = writer;
        }

        public override async Task<Sample> Handle(DeleteSampleServiceRequest request, CancellationToken cancellationToken)
        {
            ValidateEntity(request.Payload);

            ValidateDomain(request.Payload);

            Writer.Remove(request.Payload);

            return request.Payload;
        }
    }
}
