using MediatR;
using Microsoft.EntityFrameworkCore;
using ModelWrapper.Extensions.FullSearch;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Queries.GetSamplesByFilter
{
    public class GetSamplesByFilterQueryHandler : IRequestHandler<GetSamplesByFilterQuery, GetSamplesByFilterQueryResponse>
    {
        private IDefaultDbContext Context { get; set; }
        public GetSamplesByFilterQueryHandler(IDefaultDbContext context)
        {
            Context = context;
        }
        public async Task<GetSamplesByFilterQueryResponse> Handle(GetSamplesByFilterQuery request, CancellationToken cancellationToken)
        {
            long resultCount = 0;
            
            var data =  await Context.Samples
                .FullSearch(request, out resultCount)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            return new GetSamplesByFilterQueryResponse(request, data, resultCount: resultCount);
        }
    }
}
