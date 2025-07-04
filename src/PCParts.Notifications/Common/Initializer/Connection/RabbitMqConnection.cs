﻿using RabbitMQ.Client;

namespace PCParts.Notifications.Common.Initializer.Connection;

public class RabbitMqConnection : IRabbitMqConnection, IAsyncDisposable
{
    private readonly IConnectionFactory _connectionFactory;
    private Task? _initializationTask;

    public RabbitMqConnection(
        IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    private IConnection? Connection { get; set; }

    public async ValueTask DisposeAsync()
    {
        if (Connection != null)
        {
            await Connection.DisposeAsync();
        }
    }

    public async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken)
    {
        if (Connection != null && Connection.IsOpen)
        {
            return Connection;
        }

        await InitializeConnectionAsync(cancellationToken);

        if (Connection == null || !Connection.IsOpen)
        {
            throw new InvalidOperationException("RabbitMQ connection was not successfully established.");
        }

        return Connection;
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