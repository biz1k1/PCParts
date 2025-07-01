using RabbitMQ.Client;

namespace PCParts.Notifications.Common.Initializer.Connection;

public interface IRabbitMqConnection
{
    Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken);
}