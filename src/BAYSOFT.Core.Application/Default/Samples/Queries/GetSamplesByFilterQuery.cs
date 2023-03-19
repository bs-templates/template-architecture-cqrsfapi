using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModelWrapper;
using ModelWrapper.Extensions.FullSearch;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Queries
{
    public class GetSamplesByFilterQueryResponse : ApplicationResponse<Sample>
    {
        public GetSamplesByFilterQueryResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
    public class GetSamplesByFilterQuery : ApplicationRequest<Sample, GetSamplesByFilterQueryResponse>
    {
        public GetSamplesByFilterQuery()
        {
            ConfigKeys(x => x.Id);

            // Configures supressed properties & response properties
            // ConfigSuppressedProperties(x => x);
            // ConfigSuppressedResponseProperties(x => x);  
        }
    }
    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class GetSamplesByFilterQueryHandler : ApplicationRequestHandler<Sample, GetSamplesByFilterQuery, GetSamplesByFilterQueryResponse>
    {
        private IStringLocalizer Localizer { get; set; }
        private IDefaultDbContextReader Reader { get; set; }
        public GetSamplesByFilterQueryHandler(
            IStringLocalizer<GetSamplesByFilterQueryHandler> localizer,
            IDefaultDbContextReader reader)
        {
            Localizer = localizer;
            Reader = reader;
        }
        public override async Task<GetSamplesByFilterQueryResponse> Handle(GetSamplesByFilterQuery request, CancellationToken cancellationToken)
        {
            long resultCount = 0;

            var data = await Reader
                .Query<Sample>()
                .FullSearch(request, out resultCount)
                .ToListAsync(cancellationToken);

            return new GetSamplesByFilterQueryResponse(request, data, Localizer["Successful operation!"], resultCount);
        }
    }
}
