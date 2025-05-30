using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace PCParts.Storage.BackgroundServices;

public class RabbitMqQueue : BackgroundService
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly IServiceScopeFactory _scopeFactory;

    public RabbitMqQueue(IServiceScopeFactory scopeFactory, IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        await using var connection = await _connectionFactory.CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(
            exchange: "pcparts.events",
            type: ExchangeType.Topic,
            durable: true
        );

        await channel.QueueDeclareAsync(
            queue: "Component-events",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(
            queue: "Component-events",
            exchange: "pcparts.events",
            routingKey: "pcparts.component.created",
            cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var pgContext = scope.ServiceProvider.GetService<PgContext>();

            var eventsComponents = await pgContext.DomainEvents
                .Where(x => x.ActivityAt == null)
                .ToListAsync(stoppingToken);

            foreach (var msg in eventsComponents)
            {
                var payload = JsonSerializer.Serialize(msg);
                var body = Encoding.UTF8.GetBytes(payload);
                await channel.BasicPublishAsync(
                    exchange: "pcparts.events",
                    routingKey: "pcparts.component.created",
                    body: body,
                    cancellationToken: stoppingToken);

                msg.ActivityAt = DateTimeOffset.UtcNow;
            }

            await pgContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}