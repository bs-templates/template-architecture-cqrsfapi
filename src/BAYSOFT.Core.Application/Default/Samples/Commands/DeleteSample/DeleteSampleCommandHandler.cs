using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Interfaces.Services.Samples;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.DeleteSample
{
    public class DeleteSampleCommandHandler : ApplicationRequestHandler<Sample, DeleteSampleCommand, DeleteSampleCommandResponse>
    {
        private IStringLocalizer MessagesLocalizer { get; set; }
        private IStringLocalizer EntitiesDefaultLocalizer { get; set; }
        public IDefaultDbContextWriter Writer { get; set; }
        private IDeleteSampleService DeleteService { get; set; }
        public DeleteSampleCommandHandler(
            IStringLocalizer<Messages> messagesLocalizer,
            IStringLocalizer<EntitiesDefault> entitiesDefaultLocalizer,
            IDefaultDbContextWriter writer,
            IDeleteSampleService deleteService)
        {
            MessagesLocalizer = messagesLocalizer;
            EntitiesDefaultLocalizer = entitiesDefaultLocalizer;
            Writer = writer;
            DeleteService = deleteService;
        }
        public override async Task<DeleteSampleCommandResponse> Handle(DeleteSampleCommand request, CancellationToken cancellationToken)
        {
            var id = request.Project(x => x.Id);

            var data = await Writer.Query<Sample>().SingleOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                throw new Exception(string.Format(MessagesLocalizer["{0} not found!"], EntitiesDefaultLocalizer[nameof(Sample)]));
            }

            await DeleteService.Run(data);

            await Writer.CommitAsync(cancellationToken);

            return new DeleteSampleCommandResponse(request, data, MessagesLocalizer["Successful operation!"], 1);
        }
    }
}
