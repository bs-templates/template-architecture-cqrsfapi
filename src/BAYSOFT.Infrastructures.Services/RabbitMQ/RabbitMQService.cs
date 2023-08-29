using BAYSOFT.Core.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Infrastructures.Services.RabbitMQ
{

    public class RabbitMQServiceRequest : IRequest<bool>
    {
        public string QueueName { get; private set; }
        public bool QueueDurable { get; private set; }
        public bool QueueExclusive { get; internal set; }
        public bool QueueAutoDelete { get; internal set; }
        public IDictionary<string, object> QueueArguments { get; internal set; }
        public string MessageExchange { get; internal set; }
        public IBasicProperties MessageBasicProperties { get; internal set; }
        public ReadOnlyMemory<byte> MessageBody { get; internal set; }
        public RabbitMQServiceRequest(
            string queueName,
            ReadOnlyMemory<byte> messageBody)
        {
            QueueName = queueName;
            QueueDurable = false;
            QueueExclusive = false;
            QueueAutoDelete = false;
            QueueArguments = null;
            MessageExchange = "";
            MessageBasicProperties = null;
            MessageBody = messageBody;
        }
    }
    public class RabbitMQServiceRequestHandler : IRequestHandler<RabbitMQServiceRequest, bool>
    {
        private readonly ConnectionFactory ConnectionFactory;
        private ILoggerFactory Logger { get; set; }

        public RabbitMQServiceRequestHandler(ILoggerFactory logger)
        {
            Logger = logger;
            ConnectionFactory = new ConnectionFactory
            {
                Uri = new Uri("amqps://ksqqodyh:AzEXj5H6em44fwRTO1LnCEdDVFRJM0Sm@prawn.rmq.cloudamqp.com/ksqqodyh")
            };
        }

        public Task<bool> Handle(RabbitMQServiceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = ConnectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(
                            queue: request.QueueName,
                            durable: request.QueueDurable,
                            exclusive: request.QueueExclusive,
                            autoDelete: request.QueueAutoDelete,
                            arguments: request.QueueArguments
                        );

                        channel.BasicPublish(
                            exchange: request.MessageExchange,
                            routingKey: request.QueueName,
                            basicProperties: request.MessageBasicProperties,
                            body: request.MessageBody
                        );
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.CreateLogger<RabbitMQServiceRequestHandler>().Log(LogLevel.Error, exception, exception.Message);
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
