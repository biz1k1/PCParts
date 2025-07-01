using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PCParts.Storage.Common.Helpers;
using PCParts.Storage.Common.Services.Deduplication;
using RabbitMQ.Client;

namespace PCParts.Storage.BackgroundServices;

public class NotificationPublisher : BackgroundService
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly IDeduplicationService _deduplicationService;
    private readonly IServiceScopeFactory _scopeFactory;
    private IChannel _channel;
    private IConnection _connection;

    public NotificationPublisher(
        IServiceScopeFactory scopeFactory,
        IConnectionFactory connectionFactory,
        IDeduplicationService deduplicationService)
    {
        _connectionFactory = connectionFactory;
        _scopeFactory = scopeFactory;
        _deduplicationService = deduplicationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        _connection = await _connectionFactory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await using var scope = _scopeFactory.CreateAsyncScope();
        var pgContext = scope.ServiceProvider.GetService<PgContext>();

        await _channel.ExchangeDeclareAsync(exchange: "pcparts.events", type: ExchangeType.Topic,
            durable: true, cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(queue: "Notification.sms.events", durable: true,
            exclusive: false, autoDelete: false,
            arguments: new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "pcparts.dlx.exchange.fail" },
                { "x-dead-letter-routing-key", "" }
            }, cancellationToken: stoppingToken);

        await _channel.QueueBindAsync(queue: "Notification.sms.events", exchange: "pcparts.events",
            routingKey: "pcparts.component.created", cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            var eventsComponents = await pgContext.DomainEvents
                .Where(x => x.ActivityAt == null)
                .ToListAsync(stoppingToken);

            foreach (var msg in eventsComponents)
            {
                var payload = msg.Content;
                var body = Encoding.UTF8.GetBytes(payload);
                var messageId = HashHelper.ComputeSha256(payload);

                var props = new BasicProperties();
                props.MessageId = $"{messageId}";

                props.Persistent = true;
                props.Headers = new Dictionary<string, object?>
                {
                    ["From"] = "NotificationPublisher-1",
                    ["x-retry-count"] = "0"
                };

                var duplicate = _deduplicationService.IsDuplicate(messageId);
                if (duplicate is true)
                {
                    continue;
                }

                await _channel.BasicPublishAsync(exchange: "pcparts.events", routingKey: "pcparts.component.created",
                    body: body, basicProperties: props, mandatory: true, cancellationToken: stoppingToken);

                msg.ActivityAt = DateTimeOffset.UtcNow;
            }

            await pgContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    public async override void Dispose()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}