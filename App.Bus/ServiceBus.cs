using App.Application.Contracts.ServiceBus;
using App.Domain.Events;
using MassTransit;

namespace App.Bus
{
    public class ServiceBus : IServiceBus
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public ServiceBus(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
        {
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IMessage, IEvent
        {
           await _publishEndpoint.Publish(@event, cancellationToken);
        }

        public async Task SendAsync<T>(T message, string queueName, CancellationToken cancellationToken = default) where T : IMessage, IEvent
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:${queueName}"));

            await endpoint.Send(message, cancellationToken);
        }
    }
}
