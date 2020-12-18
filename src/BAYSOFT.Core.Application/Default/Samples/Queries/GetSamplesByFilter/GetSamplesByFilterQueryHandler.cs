using MediatR;
using Microsoft.EntityFrameworkCore;
using ModelWrapper.Extensions.FullSearch;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using System.Threading;
using System.Threading.Tasks;
using BAYSOFT.Core.Domain.Resources;
using Microsoft.Extensions.Localization;

namespace BAYSOFT.Core.Application.Default.Samples.Queries.GetSamplesByFilter
{
    public class GetSamplesByFilterQueryHandler : IRequestHandler<GetSamplesByFilterQuery, GetSamplesByFilterQueryResponse>
    {
        private IStringLocalizer StringLocalizer { get; set; }
        private IDefaultDbContext Context { get; set; }
        public GetSamplesByFilterQueryHandler(
            IStringLocalizer<Messages> stringLocalizer,
            IDefaultDbContext context)
        {
            StringLocalizer = stringLocalizer;
            Context = context;
        }
        public async Task<GetSamplesByFilterQueryResponse> Handle(GetSamplesByFilterQuery request, CancellationToken cancellationToken)
        {
            long resultCount = 0;
            
            var data =  await Context.Samples
                .FullSearch(request, out resultCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            return new GetSamplesByFilterQueryResponse(request, data, StringLocalizer["Successful operation!"], resultCount);
        }
    }
}
