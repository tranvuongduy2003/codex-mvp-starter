namespace Solution.Application.Abstractions.IntegrationEvents;

public sealed record QueuedIntegrationEvent(IIntegrationEvent IntegrationEvent);
