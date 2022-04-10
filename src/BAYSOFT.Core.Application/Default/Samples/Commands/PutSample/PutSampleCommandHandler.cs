using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Default;
using BAYSOFT.Core.Domain.Interfaces.Services.Default.Samples;
using BAYSOFT.Core.Domain.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModelWrapper.Extensions.Put;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.PutSample
{
    public class PutSampleCommandHandler : ApplicationRequestHandler<Sample, PutSampleCommand, PutSampleCommandResponse>
    {
        private IStringLocalizer MessagesLocalizer { get; set; }
        private IStringLocalizer EntitiesDefaultLocalizer { get; set; }
        public IDefaultDbContextWriter Writer { get; set; }
        private IPutSampleService PutService { get; set; }
        public PutSampleCommandHandler(
            IStringLocalizer<Messages> messagesLocalizer,
            IStringLocalizer<EntitiesDefault> entitiesDefaultLocalizer,
            IDefaultDbContextWriter writer,
            IPutSampleService putService)
        {
            MessagesLocalizer = messagesLocalizer;
            EntitiesDefaultLocalizer = entitiesDefaultLocalizer;
            Writer = writer;
            PutService = putService;
        }
        public override async Task<PutSampleCommandResponse> Handle(PutSampleCommand request, CancellationToken cancellationToken)
        {
            var id = request.Project(x => x.Id);
            var data = await Writer.Query<Sample>().SingleOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                throw new Exception(string.Format(MessagesLocalizer["{0} not found!"], EntitiesDefaultLocalizer[nameof(Sample)]));
            }

            request.Put(data);

            await PutService.Run(data);

            await Writer.CommitAsync(cancellationToken);

            return new PutSampleCommandResponse(request, data, MessagesLocalizer["Successful operation!"], 1);
        }
    }
}
