using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using BAYSOFT.Core.Domain.Interfaces.Services.Default.Samples;
using BAYSOFT.Core.Domain.Resources;
using Microsoft.Extensions.Localization;
using ModelWrapper.Extensions.Post;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.PostSample
{
    public class PostSampleCommandHandler : ApplicationRequestHandler<Sample, PostSampleCommand, PostSampleCommandResponse>
    {
        private IStringLocalizer MessagesLocalizer { get; set; }
        private IDefaultDbContext Context { get; set; }
        private IPostSampleService PostService { get; set; }
        public PostSampleCommandHandler(
            IStringLocalizer<Messages> messagesLocalizer,
            IDefaultDbContext context,
            IPostSampleService postService
        )
        {
            MessagesLocalizer = messagesLocalizer;
            Context = context;
            PostService = postService;
        }
        public override async Task<PostSampleCommandResponse> Handle(PostSampleCommand request, CancellationToken cancellationToken)
        {
            var data = request.Post();

            await PostService.Run(data);

            await Context.SaveChangesAsync();

            return new PostSampleCommandResponse(request, data, MessagesLocalizer["Successful operation!"], 1);
        }
    }
}
