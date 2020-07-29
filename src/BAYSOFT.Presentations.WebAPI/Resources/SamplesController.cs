using BAYSOFT.Core.Application.Default.Samples.Queries.GetSampleByID;
using BAYSOFT.Core.Application.Default.Samples.Queries.GetSamplesByFilter;
using BAYSOFT.Presentations.WebAPI.Abstractions.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Resources
{
    public class SamplesController : ResourceController
    {
        [HttpGet]
        public async Task<ActionResult<GetSamplesByFilterQueryResponse>> Get(GetSamplesByFilterQuery request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Mediator.Send(request, cancellationToken);
        }

        [HttpGet("{sampleid}")]
        public async Task<ActionResult<GetSampleByIDQueryResponse>> Get(GetSampleByIDQuery request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Mediator.Send(request, cancellationToken);
        }
    }
}
