using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using BAYSOFT.Core.Domain.Interfaces.Services.Default.Samples;
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
        public IDefaultDbContext Context { get; set; }
        private IDeleteSampleService DeleteService { get; set; }
        public DeleteSampleCommandHandler(
            IStringLocalizer<Messages> messagesLocalizer,
            IStringLocalizer<EntitiesDefault> entitiesDefaultLocalizer,
            IDefaultDbContext context,
            IDeleteSampleService deleteService)
        {
            MessagesLocalizer = messagesLocalizer;
            EntitiesDefaultLocalizer = entitiesDefaultLocalizer;
            Context = context;
            DeleteService = deleteService;
        }
        public override async Task<DeleteSampleCommandResponse> Handle(DeleteSampleCommand request, CancellationToken cancellationToken)
        {
            var id = request.Project(x => x.Id);

            var data = await Context.Samples.SingleOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                throw new Exception(string.Format(MessagesLocalizer["{0} not found!"], EntitiesDefaultLocalizer[nameof(Sample)]));
            }

            await DeleteService.Run(data);

            await Context.SaveChangesAsync();

            return new DeleteSampleCommandResponse(request, data, MessagesLocalizer["Successful operation!"], 1);
        }
    }
}
