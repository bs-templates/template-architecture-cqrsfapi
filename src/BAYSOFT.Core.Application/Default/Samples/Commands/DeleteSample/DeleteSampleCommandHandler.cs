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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.DeleteSample
{
    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class DeleteSampleCommandHandler : ApplicationRequestHandler<Sample, DeleteSampleCommand, DeleteSampleCommandResponse>
    {
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        public IDefaultDbContextWriter Writer { get; set; }
        private IDeleteSampleService DeleteService { get; set; }
        public DeleteSampleCommandHandler(
            IMediator mediator,
            IStringLocalizer<DeleteSampleCommandHandler> localizer,
            IDefaultDbContextWriter writer,
            IDeleteSampleService deleteService)
        {
            Mediator = mediator;
            Localizer = localizer;
            Writer = writer;
            DeleteService = deleteService;
        }
        public override async Task<DeleteSampleCommandResponse> Handle(DeleteSampleCommand request, CancellationToken cancellationToken)
        {
            request.IsValid(true);

            var id = request.Project(x => x.Id);

            var data = await Writer.Query<Sample>().SingleOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                throw new Exception(string.Format(Localizer["{0} not found!"], Localizer[nameof(Sample)]));
            }

            await DeleteService.Run(data);

            await Mediator.Publish(new DeleteSampleNotification(data));

            await Writer.CommitAsync(cancellationToken);

            return new DeleteSampleCommandResponse(request, data, Localizer["Successful operation!"], 1);
        }
    }
}
