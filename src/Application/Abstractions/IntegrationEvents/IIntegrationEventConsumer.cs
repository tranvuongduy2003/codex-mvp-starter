namespace Solution.Application.Abstractions.IntegrationEvents;

public interface IIntegrationEventConsumer
{
    Type IntegrationEventType { get; }

    Task ConsumeAsync(object integrationEvent, CancellationToken cancellationToken = default);
}

public interface IIntegrationEventConsumer<in TIntegrationEvent> : IIntegrationEventConsumer
    where TIntegrationEvent : IIntegrationEvent
{
    Task ConsumeAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}
