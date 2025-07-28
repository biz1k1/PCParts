using System.Data;
using Npgsql;

namespace PCParts.Storage.Common.Services.DbConnectionProvider;

public class NpgsqlConnectionProvider : IDbConnectionProvider<NpgsqlConnection>, IAsyncDisposable
{
    private readonly string _connectionString;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private NpgsqlConnection? _listeningConnection;
    private NpgsqlConnection? _queryConnection;

    public NpgsqlConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }


    public async ValueTask DisposeAsync()
    {
        if (_listeningConnection != null)
        {
            await _listeningConnection.CloseAsync();
            await _listeningConnection.DisposeAsync();
            _listeningConnection = null;

            GC.SuppressFinalize(this);
        }
    }

    public async Task<NpgsqlConnection> GetOpenConnection(bool forListenOnly)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (forListenOnly)
            {
                if (_listeningConnection == null)
                    _listeningConnection = new NpgsqlConnection(_connectionString);

                if (_listeningConnection.State != ConnectionState.Open)
                    await _listeningConnection.OpenAsync();

                return _listeningConnection;
            }

            if (_queryConnection == null)
                _queryConnection = new NpgsqlConnection(_connectionString);

            if (_queryConnection.State != ConnectionState.Open)
            {
                await _queryConnection.OpenAsync();
            }

            return _queryConnection;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}