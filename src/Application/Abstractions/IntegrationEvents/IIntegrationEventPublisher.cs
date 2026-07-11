namespace Solution.Application.Abstractions.IntegrationEvents;

public interface IIntegrationEventPublisher
{
    ValueTask PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}
