using System.Text;
using Microsoft.Extensions.Hosting;
using PCParts.Storage.Common.Helpers;
using PCParts.Storage.Common.Services.Deduplication;
using PCParts.Storage.Common.Services.DomainEventReaderNotify;
using RabbitMQ.Client;

namespace PCParts.Storage.BackgroundServices;

public class NotificationPublisher : BackgroundService
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly IDeduplicationService _deduplicationService;
    private readonly IDomainEventReaderNotify _domainEventReaderNotify;

    private IChannel? _channelRabbitMq;
    private IConnection? _connectionRabbitMq;

    public NotificationPublisher(
        IConnectionFactory connectionFactory,
        IDeduplicationService deduplicationService,
        IDomainEventReaderNotify domainEventReaderNotify)
    {
        _connectionFactory = connectionFactory;
        _deduplicationService = deduplicationService;
        _domainEventReaderNotify = domainEventReaderNotify;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        _connectionRabbitMq = await _connectionFactory.CreateConnectionAsync(stoppingToken);
        _channelRabbitMq = await _connectionRabbitMq.CreateChannelAsync(cancellationToken: stoppingToken);
        _ = Task.Run(() => _domainEventReaderNotify.StartListeningNotifyAsync(stoppingToken));

        await _channelRabbitMq.ExchangeDeclareAsync(exchange: "pcparts.events", type: ExchangeType.Topic,
            durable: true, cancellationToken: stoppingToken);

        await _channelRabbitMq.QueueDeclareAsync(queue: "Notification.sms.events", durable: true,
            exclusive: false, autoDelete: false,
            arguments: new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "pcparts.dlx.exchange.fail" },
                { "x-dead-letter-routing-key", "" }
            }, cancellationToken: stoppingToken);

        await _channelRabbitMq.QueueBindAsync(queue: "Notification.sms.events", exchange: "pcparts.events",
            routingKey: "pcparts.component.created", cancellationToken: stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await _domainEventReaderNotify.EventSignal.WaitAsync(stoppingToken);

            var events = await _domainEventReaderNotify.GetAllNonActivatedEventsAsync(stoppingToken);

            foreach (var msg in events)
            {
                var payload = msg.Content;
                var body = Encoding.UTF8.GetBytes(payload);
                var messageId = HashHelper.ComputeSha256(payload);

                var props = new BasicProperties();
                props.MessageId = $"{messageId}";

                props.Persistent = true;
                props.Headers = new Dictionary<string, object?>
                {
                    ["From"] = "NotificationPublisher-1"
                };

                var duplicate = _deduplicationService.IsDuplicate(messageId);
                if (duplicate)
                {
                    continue;
                }

                await _channelRabbitMq.BasicPublishAsync(exchange: "pcparts.events", routingKey: "pcparts.component.created",
                    body: body, basicProperties: props, mandatory: true, cancellationToken: stoppingToken);
            }

            await _domainEventReaderNotify.MarkEventsAsActivatedAsync(events.Select(x=>x.Id),stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }

    }
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channelRabbitMq != null)
        {
            await _channelRabbitMq.CloseAsync(cancellationToken);
            await _channelRabbitMq.DisposeAsync();
            _channelRabbitMq = null;
        }

        if (_connectionRabbitMq != null)
        {
            await _connectionRabbitMq.CloseAsync(cancellationToken);
            await _connectionRabbitMq.DisposeAsync();
            _connectionRabbitMq = null;
        }

        await base.StopAsync(cancellationToken);
    }
    public new void Dispose()
    {
        base.Dispose();
    }
}