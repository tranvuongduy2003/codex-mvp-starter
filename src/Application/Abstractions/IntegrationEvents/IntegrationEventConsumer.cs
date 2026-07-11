namespace Solution.Application.Abstractions.IntegrationEvents;

public abstract class IntegrationEventConsumer<TIntegrationEvent> : IIntegrationEventConsumer<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    public Type IntegrationEventType => typeof(TIntegrationEvent);

    public Task ConsumeAsync(object integrationEvent, CancellationToken cancellationToken = default)
    {
        if (integrationEvent is not TIntegrationEvent typedIntegrationEvent)
        {
            throw new InvalidOperationException(
                $"Integration event type '{integrationEvent.GetType().Name}' cannot be handled by '{GetType().Name}'.");
        }

        return ConsumeAsync(typedIntegrationEvent, cancellationToken);
    }

    public abstract Task ConsumeAsync(
        TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default);
}
