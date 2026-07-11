using System.Threading.Channels;
using Solution.Application.Abstractions.IntegrationEvents;
using Solution.Infrastructure.IntegrationEvents;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Solution.Api.IntegrationTests.Infrastructure;

public sealed class ChannelIntegrationEventConsumerServiceTests
{
    [Fact]
    public async Task ExecuteAsync_ResolvesTypedConsumerFromDependencyInjection()
    {
        var channel = Channel.CreateUnbounded<QueuedIntegrationEvent>();
        var completion = new TaskCompletionSource<TestIntegrationEvent>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        var services = new ServiceCollection()
            .AddLogging()
            .AddSingleton(completion)
            .AddScoped<IIntegrationEventConsumer<TestIntegrationEvent>, TestIntegrationEventConsumer>()
            .BuildServiceProvider();

        var service = new ChannelIntegrationEventConsumerService(
            channel,
            services.GetRequiredService<IServiceScopeFactory>(),
            services.GetRequiredService<ILogger<ChannelIntegrationEventConsumerService>>());
        var publisher = new ChannelIntegrationEventPublisher(channel);
        var integrationEvent = new TestIntegrationEvent(Guid.NewGuid());

        await service.StartAsync(CancellationToken.None);

        try
        {
            await publisher.PublishAsync(integrationEvent);
            var consumedEvent = await completion.Task.WaitAsync(TimeSpan.FromSeconds(5));

            consumedEvent.Should().Be(integrationEvent);
        }
        finally
        {
            await service.StopAsync(CancellationToken.None);
            await services.DisposeAsync();
        }
    }

    private sealed record TestIntegrationEvent(Guid Id) : IIntegrationEvent;

    private sealed class TestIntegrationEventConsumer(
        TaskCompletionSource<TestIntegrationEvent> completion)
        : IntegrationEventConsumer<TestIntegrationEvent>
    {
        public override Task ConsumeAsync(
            TestIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default)
        {
            completion.TrySetResult(integrationEvent);
            return Task.CompletedTask;
        }
    }
}
