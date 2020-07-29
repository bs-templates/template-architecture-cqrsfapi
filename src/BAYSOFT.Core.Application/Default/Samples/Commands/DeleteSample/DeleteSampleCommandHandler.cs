using MediatR;
using Microsoft.EntityFrameworkCore;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using System;
using System.Threading;
using System.Threading.Tasks;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Services.Default.Samples;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.DeleteSample
{
    public class DeleteSampleCommandHandler : ApplicationRequestHandler<Sample, DeleteSampleCommand, DeleteSampleCommandResponse>
    {
        public IDefaultDbContext Context { get; set; }
        private IDeleteSampleService DeleteService { get; set; }
        public DeleteSampleCommandHandler(
            IDefaultDbContext context,
            IDeleteSampleService deleteService)
        {
            Context = context;
            DeleteService = deleteService;
        }
        public override async Task<DeleteSampleCommandResponse> Handle(DeleteSampleCommand request, CancellationToken cancellationToken)
        {
            var id = request.Project(x => x.SampleID);

            var data = await Context.Samples.SingleOrDefaultAsync(x => x.SampleID == id);

            if (data == null)
                throw new Exception("Sample not found!");

            await DeleteService.Run(data);

            await Context.SaveChangesAsync();

            return new DeleteSampleCommandResponse(request, data, "Successful operation!", 1);
        }
    }
}
