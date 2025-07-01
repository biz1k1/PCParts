using PCParts.Notifications.Common.Initializer.Connection;
using RabbitMQ.Client;

namespace PCParts.Notifications.Common.Initializer;

public class RabbitMqInitializer : IRabbitMqInitializer, IAsyncDisposable
{
    private readonly IRabbitMqConnection _rabbitMqConnection;
    private IChannel? _channel;
    private IConnection _connection;

    public RabbitMqInitializer(
        IRabbitMqConnection rabbitMqConnection
    )
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.DisposeAsync();
            _channel = null;
        }
    }

    public async Task<IChannel> GetNotificationChannel(CancellationToken stoppingToken)
    {
        _connection = await _rabbitMqConnection.GetConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await SetupDeadLetterInfrastructure(stoppingToken);
        await SetupSmsNotificationInfrastructure(stoppingToken);

        return _channel;
    }

    private async Task SetupDeadLetterInfrastructure(CancellationToken cancellationToken)
    {
        var queueArgs = new Dictionary<string, object>
        {
            { "x-message-ttl", 5000 },
            { "x-dead-letter-exchange", "pcparts.dlx.exchange.retry" },
            { "x-dead-letter-routing-key", "" }
        };

        await _channel!.ExchangeDeclareAsync(exchange: "pcparts.dlx.exchange.fail", type: ExchangeType.Fanout,
            durable: true, cancellationToken: cancellationToken);
        await _channel!.ExchangeDeclareAsync(exchange: "pcparts.dlx.exchange.retry", type: ExchangeType.Fanout,
            durable: true, cancellationToken: cancellationToken);

        await _channel.QueueDeclareAsync(queue: "Notification.sms.events.dlx", durable: true,
            exclusive: false, autoDelete: false,
            arguments: queueArgs, cancellationToken: cancellationToken);

        await _channel.QueueBindAsync(queue: "Notification.sms.events.dlx", exchange: "pcparts.dlx.exchange.fail",
            routingKey: "", cancellationToken: cancellationToken);
        await _channel.QueueBindAsync(queue: "Notification.sms.events", exchange: "pcparts.dlx.exchange.retry",
            routingKey: "", cancellationToken: cancellationToken);
    }

    private async Task SetupSmsNotificationInfrastructure(CancellationToken stoppingToken)
    {
        var queueArgs = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", "pcparts.dlx.exchange.fail" },
            { "x-dead-letter-routing-key", "" }
        };
        await _channel.ExchangeDeclareAsync(exchange: "pcparts.events", type: ExchangeType.Topic,
            durable: true, cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(queue: "Notification.sms.events", durable: true,
            exclusive: false, autoDelete: false,
            arguments: queueArgs, cancellationToken: stoppingToken);

        await _channel.QueueBindAsync(queue: "Notification.sms.events", exchange: "pcparts.events",
            routingKey: "pcparts.component.created", cancellationToken: stoppingToken);
    }
}