using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Interfaces.Services.Samples;
using BAYSOFT.Core.Domain.Default.Notifications.Samples;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModelWrapper.Extensions.Patch;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.PatchSample
{
    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class PatchSampleCommandHandler : ApplicationRequestHandler<Sample, PatchSampleCommand, PatchSampleCommandResponse>
    {
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        public IDefaultDbContextWriter Writer { get; set; }
        private IPatchSampleService PatchService { get; set; }
        public PatchSampleCommandHandler(
            IMediator mediator,
            IStringLocalizer<PatchSampleCommandHandler> localizer,
            IDefaultDbContextWriter writer,
            IPatchSampleService patchService)
        {
            Mediator = mediator;
            Localizer = localizer;
            Writer = writer;
            PatchService = patchService;
        }

        public override async Task<PatchSampleCommandResponse> Handle(PatchSampleCommand request, CancellationToken cancellationToken)
        {
            request.IsValid(true);

            var id = request.Project(x => x.Id);

            var data = await Writer.Query<Sample>().SingleOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                throw new Exception(string.Format(Localizer["{0} not found!"], Localizer[nameof(Sample)]));
            }

            request.Patch(data);

            await PatchService.Run(data);

            await Mediator.Publish(new PatchSampleNotification(data));

            await Writer.CommitAsync(cancellationToken);

            return new PatchSampleCommandResponse(request, data, Localizer["Successful operation!"], 1);
        }
    }
}
