using BAYSOFT.Core.Domain.Models;
using BAYSOFT.Infrastructures.Services.RabbitMQ;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Notifications
{
    public class PostSampleNotificationHandler : INotificationHandler<PostSampleNotification>
    {
        private ILoggerFactory Logger { get; set; }
        private IMediator Mediator { get; set; }
        public PostSampleNotificationHandler(
            ILoggerFactory logger,
            IMediator mediator)
        {
            Logger = logger;
            Mediator = mediator;
        }
        public Task Handle(PostSampleNotification notification, CancellationToken cancellationToken)
        {
            Logger.CreateLogger<PostSampleNotificationHandler>().Log(LogLevel.Information, $"Sample posted! - Event Created At: {notification.CreatedAt:yyyy-MM-dd HH:mm:ss} Payload: {JsonConvert.SerializeObject(notification.Payload)}");

            //var message = new Message<PostSampleNotification>(notification);

            //Mediator.Send(new RabbitMQServiceRequest("BAYSOFT_EVENTS", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))));

            return Task.CompletedTask;
        }
    }
}