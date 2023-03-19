using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Exceptions;
using BAYSOFT.Core.Domain.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModelWrapper;
using ModelWrapper.Extensions.Select;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Queries
{
    public class GetSampleByIdQueryResponse : ApplicationResponse<Sample>
    {
        public GetSampleByIdQueryResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
    public class GetSampleByIdQuery : ApplicationRequest<Sample, GetSampleByIdQueryResponse>
    {
        public GetSampleByIdQuery()
        {
            ConfigKeys(x => x.Id);
            
            // Configures supressed properties & response properties
            //ConfigSuppressedProperties(x => x);
            //ConfigSuppressedResponseProperties(x => x);  
        }
    }
    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class GetSampleByIdQueryHandler : ApplicationRequestHandler<Sample, GetSampleByIdQuery, GetSampleByIdQueryResponse>
    {
        private IStringLocalizer Localizer { get; set; }
        private IDefaultDbContextReader Reader { get; set; }
        public GetSampleByIdQueryHandler(
            IStringLocalizer<GetSampleByIdQueryHandler> localizer,
            IDefaultDbContextReader reader)
        {
            Localizer = localizer;
            Reader = reader;
        }
        public override async Task<GetSampleByIdQueryResponse> Handle(GetSampleByIdQuery request, CancellationToken cancellationToken)
        {
            var id = request.Project(x => x.Id);

            var data = await Reader
                .Query<Sample>()
                .Where(x => x.Id == id)
                .Select(request)
                .SingleOrDefaultAsync();

            if (data == null)
            {
                throw new EntityNotFoundException<Sample>(Localizer);
            }

            return new GetSampleByIdQueryResponse(request, data, Localizer["Successful operation!"], 1);
        }
    }
}
