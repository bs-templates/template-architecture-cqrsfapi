using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModelWrapper.Extensions.FullSearch;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Queries.GetSamplesByFilter
{
    public class GetSamplesByFilterQueryHandler : ApplicationRequestHandler<Sample, GetSamplesByFilterQuery, GetSamplesByFilterQueryResponse>
    {
        private IStringLocalizer StringLocalizer { get; set; }
        private IDefaultDbContextReader Reader { get; set; }
        public GetSamplesByFilterQueryHandler(
            IStringLocalizer<Messages> stringLocalizer,
            IDefaultDbContextReader reader)
        {
            StringLocalizer = stringLocalizer;
            Reader = reader;
        }
        public override async Task<GetSamplesByFilterQueryResponse> Handle(GetSamplesByFilterQuery request, CancellationToken cancellationToken)
        {
            long resultCount = 0;
            
            var data =  await Reader.Query<Sample>()
                .FullSearch(request, out resultCount)
                .ToListAsync(cancellationToken);
            
            return new GetSamplesByFilterQueryResponse(request, data, StringLocalizer["Successful operation!"], resultCount);
        }
    }
}
