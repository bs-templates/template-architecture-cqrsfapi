using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModelWrapper.Extensions.Select;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Queries.GetSampleById
{
    public class GetSampleByIdQueryHandler : ApplicationRequestHandler<Sample, GetSampleByIdQuery, GetSampleByIdQueryResponse>
    {
        private IStringLocalizer MessagesLocalizer { get; set; }
        private IStringLocalizer EntitiesDefaultLocalizer { get; set; }
        private IDefaultDbContextReader Reader { get; set; }
        public GetSampleByIdQueryHandler(
            IStringLocalizer<Messages> messagesLocalizer,
            IStringLocalizer<EntitiesDefault> entitiesDefaultLocalizer,
            IDefaultDbContextReader reader)
        {
            MessagesLocalizer = messagesLocalizer;
            EntitiesDefaultLocalizer = entitiesDefaultLocalizer;
            Reader = reader;
        }
        public override async Task<GetSampleByIdQueryResponse> Handle(GetSampleByIdQuery request, CancellationToken cancellationToken)
        {
            var id = request.Project(x => x.Id);

            var data = await Reader.Query<Sample>()
                .Where(x => x.Id == id)
                .Select(request)
                .SingleOrDefaultAsync();

            if (data == null)
            {
                throw new Exception(string.Format(MessagesLocalizer["{0} not found!"], EntitiesDefaultLocalizer[nameof(Sample)]));
            }

            return new GetSampleByIdQueryResponse(request, data, MessagesLocalizer["Successful operation!"], 1);
        }
    }
}
