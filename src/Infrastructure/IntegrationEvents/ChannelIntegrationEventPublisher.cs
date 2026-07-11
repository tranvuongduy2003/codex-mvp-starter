using System.Threading.Channels;
using Solution.Application.Abstractions.IntegrationEvents;

namespace Solution.Infrastructure.IntegrationEvents;

public sealed class ChannelIntegrationEventPublisher(Channel<QueuedIntegrationEvent> channel)
    : IIntegrationEventPublisher
{
    public ValueTask PublishAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default) =>
        channel.Writer.WriteAsync(new QueuedIntegrationEvent(integrationEvent), cancellationToken);
}
