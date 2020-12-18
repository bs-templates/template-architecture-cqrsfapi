using BAYSOFT.Core.Application.Default.Samples.Commands.DeleteSample;
using BAYSOFT.Core.Application.Default.Samples.Commands.PatchSample;
using BAYSOFT.Core.Application.Default.Samples.Commands.PostSample;
using BAYSOFT.Core.Application.Default.Samples.Commands.PutSample;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<GetSampleByIDQueryResponse>> Get(GetSampleByIDQuery request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Mediator.Send(request, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<PostSampleCommandResponse>> Post(PostSampleCommand request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Mediator.Send(request, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PutSampleCommandResponse>> Put(PutSampleCommand request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Mediator.Send(request, cancellationToken);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<PatchSampleCommandResponse>> Patch(PatchSampleCommand request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Mediator.Send(request, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteSampleCommandResponse>> Delete(DeleteSampleCommand request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Mediator.Send(request, cancellationToken);
        }
    }
}
