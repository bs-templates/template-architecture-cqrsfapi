using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Core.Domain.Exceptions;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Application.Default.Samples.Notifications;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Default.Services.Samples;
using BAYSOFT.Core.Domain.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands
{

    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class DeleteSampleCommandHandler : ApplicationRequestHandler<Sample, DeleteSampleCommand, DeleteSampleCommandResponse>
    {
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        private IDefaultDbContextWriter Writer { get; set; }
        public DeleteSampleCommandHandler(
            IMediator mediator,
            IStringLocalizer<DeleteSampleCommandHandler> localizer,
            IDefaultDbContextWriter writer
        )
        {
            Mediator = mediator;
            Localizer = localizer;
            Writer = writer;
        }
        public override async Task<DeleteSampleCommandResponse> Handle(DeleteSampleCommand request, CancellationToken cancellationToken)
        {
            request.IsValid(Localizer, true);

            var id = request.Project(x => x.Id);

            var data = await Writer
                .Query<Sample>()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                throw new EntityNotFoundException<Sample>(Localizer);
            }

            await Mediator.Send(new DeleteSampleRequest(data));

            await Mediator.Publish(new DeleteSampleNotification(data));

            return new DeleteSampleCommandResponse(request, data, Localizer["Successful operation!"], 1);
        }
    }
}