using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Interfaces.Services.Samples;
using BAYSOFT.Core.Domain.Default.Notifications.Samples;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Resources;
using MediatR;
using Microsoft.Extensions.Localization;
using ModelWrapper.Extensions.Post;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.PostSample
{
    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class PostSampleCommandHandler : ApplicationRequestHandler<Sample, PostSampleCommand, PostSampleCommandResponse>
    {
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        private IPostSampleService PostService { get; set; }
        private IDefaultDbContextWriter Writer { get; set; }
        public PostSampleCommandHandler(
            IMediator mediator,
            IStringLocalizer<PostSampleCommandHandler> localizer,
            IDefaultDbContextWriter writer,
            IPostSampleService postService
        )
        {
            Mediator = mediator;
            Localizer = localizer;
            Writer = writer;
            PostService = postService;
        }
        public override async Task<PostSampleCommandResponse> Handle(PostSampleCommand request, CancellationToken cancellationToken)
        {
            request.IsValid(true);

            var data = request.Post();

            await PostService.Run(data);

            await Mediator.Publish(new PostSampleNotification(data));

            await Writer.CommitAsync(cancellationToken);

            return new PostSampleCommandResponse(request, data, Localizer["Successful operation!"], 1);
        }
    }
}
