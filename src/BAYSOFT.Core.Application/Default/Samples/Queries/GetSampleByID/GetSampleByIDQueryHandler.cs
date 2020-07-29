using MediatR;
using Microsoft.EntityFrameworkCore;
using ModelWrapper.Extensions.Select;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Queries.GetSampleByID
{
    public class GetSampleByIDQueryHandler : IRequestHandler<GetSampleByIDQuery, GetSampleByIDQueryResponse>
    {
        private IDefaultDbContext Context { get; set; }
        public GetSampleByIDQueryHandler(IDefaultDbContext context)
        {
            Context = context;
        }
        public async Task<GetSampleByIDQueryResponse> Handle(GetSampleByIDQuery request, CancellationToken cancellationToken)
        {
            var id = request.Project(x => x.SampleID);

            var data = await Context.Samples
                .Where(x => x.SampleID == id)
                .Select(request)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (data == null)
            {
                throw new Exception("Sample not found!");
            }

            return new GetSampleByIDQueryResponse(request, data, resultCount: 1);
        }
    }
}
