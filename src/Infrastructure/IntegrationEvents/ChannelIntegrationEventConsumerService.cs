using System.Threading.Channels;
using Solution.Application.Abstractions.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Solution.Infrastructure.IntegrationEvents;

public sealed class ChannelIntegrationEventConsumerService(
    Channel<QueuedIntegrationEvent> channel,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<ChannelIntegrationEventConsumerService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var queuedIntegrationEvent in channel.Reader.ReadAllAsync(stoppingToken))
        {
            await DispatchAsync(queuedIntegrationEvent, stoppingToken);
        }
    }

    private async Task DispatchAsync(
        QueuedIntegrationEvent queuedIntegrationEvent,
        CancellationToken cancellationToken)
    {
        var integrationEvent = queuedIntegrationEvent.IntegrationEvent;
        var integrationEventType = integrationEvent.GetType();
        var consumerType = typeof(IIntegrationEventConsumer<>).MakeGenericType(integrationEventType);

        using var scope = serviceScopeFactory.CreateScope();
        var consumer = scope.ServiceProvider.GetService(consumerType) as IIntegrationEventConsumer;

        if (consumer is null)
        {
            logger.LogWarning(
                "No integration event consumer registered for {IntegrationEventType}.",
                integrationEventType.Name);
            return;
        }

        try
        {
            await consumer.ConsumeAsync(integrationEvent, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Integration event consumer {ConsumerType} failed for {IntegrationEventType}.",
                consumer.GetType().Name,
                integrationEventType.Name);
        }
    }
}
