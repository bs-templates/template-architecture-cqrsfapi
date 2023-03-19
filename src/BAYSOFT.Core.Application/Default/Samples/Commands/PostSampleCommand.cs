using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Application.Default.Samples.Notifications;
using BAYSOFT.Core.Domain.Default.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Default.Services.Samples;
using BAYSOFT.Core.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using ModelWrapper;
using ModelWrapper.Extensions.Post;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands
{
    public class PostSampleCommandResponse : ApplicationResponse<Sample>
    {
        public PostSampleCommandResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }
    public class PostSampleCommand : ApplicationRequest<Sample, PostSampleCommandResponse>
    {
        public PostSampleCommand()
        {
            ConfigKeys(x => x.Id);

            ConfigSuppressedProperties(x => x.Id);

            Validator.RuleFor(x => x.Description).NotEmpty().WithMessage("{0} is required!");
        }
    }

    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    public class PostSampleCommandHandler : ApplicationRequestHandler<Sample, PostSampleCommand, PostSampleCommandResponse>
    {
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        private IDefaultDbContextWriter Writer { get; set; }
        public PostSampleCommandHandler(
            IMediator mediator,
            IStringLocalizer<PostSampleCommandHandler> localizer,
            IDefaultDbContextWriter writer
        )
        {
            Mediator = mediator;
            Localizer = localizer;
            Writer = writer;
        }
        public override async Task<PostSampleCommandResponse> Handle(PostSampleCommand request, CancellationToken cancellationToken)
        {
            request.IsValid(Localizer, true);

            var data = request.Post();

            await Mediator.Send(new CreateSampleServiceRequest(data));

            await Mediator.Publish(new PostSampleNotification(data));

            await Writer.CommitAsync(cancellationToken);

            return new PostSampleCommandResponse(request, data, Localizer["Successful operation!"], 1);
        }
    }
}
