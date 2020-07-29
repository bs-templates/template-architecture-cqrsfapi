using MediatR;
using Microsoft.EntityFrameworkCore;
using ModelWrapper.Extensions.Patch;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using System;
using System.Threading;
using System.Threading.Tasks;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Services.Default.Samples;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.PatchSample
{
    public class PatchSampleCommandHandler : ApplicationRequestHandler<Sample, PatchSampleCommand, PatchSampleCommandResponse>
    {
        public IDefaultDbContext Context { get; set; }
        private IPatchSampleService PatchService { get; set; }
        public PatchSampleCommandHandler(
            IDefaultDbContext context,
            IPatchSampleService patchService)
        {
            Context = context;
            PatchService = patchService;
        }
        public override async Task<PatchSampleCommandResponse> Handle(PatchSampleCommand request, CancellationToken cancellationToken)
        {
            var id = request.Project(x => x.SampleID);

            var data = await Context.Samples.SingleOrDefaultAsync(x => x.SampleID == id);

            if (data == null)
            {
                throw new Exception("Sample not found!");
            }

            request.Patch(data);

            await PatchService.Run(data);

            await Context.SaveChangesAsync();

            return new PatchSampleCommandResponse(request, data, "Successful operation!", 1);
        }
    }
}
