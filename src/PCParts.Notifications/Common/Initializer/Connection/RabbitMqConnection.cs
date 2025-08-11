using RabbitMQ.Client;

namespace PCParts.Notifications.Common.Initializer.Connection;

public class RabbitMqConnection : IRabbitMqConnection, IAsyncDisposable
{
    private readonly IConnectionFactory _connectionFactory;
    private IConnection? Connection { get; set; }
    public RabbitMqConnection(
        IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async ValueTask DisposeAsync()
    {
        if (Connection != null)
        {
            await Connection.DisposeAsync();
            GC.SuppressFinalize(this);

        }
    }

    public async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken)
    {
        if (Connection?.IsOpen is true)
        {
            return Connection;
        }

        await InitializeConnectionAsync(cancellationToken);

        return Connection?.IsOpen is not true
            ? throw new InvalidOperationException("RabbitMQ connection was not successfully established.")
            : Connection;
    }

    private async Task InitializeConnectionAsync(CancellationToken cancellationToken)
    {
        try
        {
            Connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to initialize RabbitMQ connection.", ex);
        }
    }
}
