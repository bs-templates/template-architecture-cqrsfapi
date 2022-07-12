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
using ModelWrapper.Extensions.Put;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.PutSample
{
    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class PutSampleCommandHandler : ApplicationRequestHandler<Sample, PutSampleCommand, PutSampleCommandResponse>
    {
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        public IDefaultDbContextWriter Writer { get; set; }
        private IPutSampleService PutService { get; set; }
        public PutSampleCommandHandler(
            IMediator mediator,
            IStringLocalizer<PutSampleCommandHandler> localizer,
            IDefaultDbContextWriter writer,
            IPutSampleService putService)
        {
            Mediator = mediator;
            Localizer = localizer;
            Writer = writer;
            PutService = putService;
        }
        public override async Task<PutSampleCommandResponse> Handle(PutSampleCommand request, CancellationToken cancellationToken)
        {
            request.IsValid(true);

            var id = request.Project(x => x.Id);
            var data = await Writer.Query<Sample>().SingleOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                throw new Exception(string.Format(Localizer["{0} not found!"], Localizer[nameof(Sample)]));
            }

            request.Put(data);

            await PutService.Run(data);

            await Mediator.Publish(new PutSampleNotification(data));

            await Writer.CommitAsync(cancellationToken);

            return new PutSampleCommandResponse(request, data, Localizer["Successful operation!"], 1);
        }
    }
}
