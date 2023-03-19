using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Application.Default.Samples.Notifications;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Default.Services.Samples;
using BAYSOFT.Core.Domain.Exceptions;
using BAYSOFT.Core.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModelWrapper;
using ModelWrapper.Extensions.Patch;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands
{
    public class PatchSampleCommandResponse : ApplicationResponse<Sample>
    {
        public PatchSampleCommandResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class PatchSampleCommand : ApplicationRequest<Sample, PatchSampleCommandResponse>
    {
        public PatchSampleCommand()
        {
            ConfigKeys(x => x.Id);

            Validator.RuleFor(x => x.Id).NotEqual(0).WithMessage("{0} is required!");
            Validator.RuleFor(x => x.Description).NotEmpty().WithMessage("{0} is required!");
        }
    }
    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class PatchSampleCommandHandler : ApplicationRequestHandler<Sample, PatchSampleCommand, PatchSampleCommandResponse>
    {
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        public IDefaultDbContextWriter Writer { get; set; }
        public PatchSampleCommandHandler(
            IMediator mediator,
            IStringLocalizer<PatchSampleCommandHandler> localizer,
            IDefaultDbContextWriter writer)
        {
            Mediator = mediator;
            Localizer = localizer;
            Writer = writer;
        }

        public override async Task<PatchSampleCommandResponse> Handle(PatchSampleCommand request, CancellationToken cancellationToken)
        {
            request.IsValid(Localizer, true);

            var id = request.Project(x => x.Id);

            var data = await Writer.Query<Sample>().SingleOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                throw new EntityNotFoundException<Sample>(Localizer);
            }

            request.Patch(data);

            await Mediator.Send(new UpdateSampleServiceRequest(data));

            await Mediator.Publish(new PatchSampleNotification(data));

            await Writer.CommitAsync(cancellationToken);

            return new PatchSampleCommandResponse(request, data, Localizer["Successful operation!"], 1);
        }
    }
}
