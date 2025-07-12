using System.Data.Common;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using PCParts.Domain.Entities;
using PCParts.Storage.Common.Extensions;
using PCParts.Storage.Common.Services.DbConnectionProvider;

namespace PCParts.Storage.Common.Services.DomainEventReaderNotify
{
    public class DomainEventReaderNotify : IDomainEventReaderNotify
    {
        private readonly IDbConnectionProvider<NpgsqlConnection> _connectionProvider;
        private readonly AsyncSignal _eventSignal = new();
        public AsyncSignal EventSignal => _eventSignal;

        public DomainEventReaderNotify(
            IDbConnectionProvider<NpgsqlConnection> connectionProvider
            )
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<List<DomainEvents>> GetAllNonActivatedEventsAsync(CancellationToken cancellationToken)
        {
            var connection = await _connectionProvider.GetOpenConnection(false);
            const string nonActivatedEventsQuery = 
                @"SELECT * FROM ""DomainEvents"" WHERE ""ActivityAt"" IS NULL;";

            var list = new List<DomainEvents>();

            await using var cmd = new NpgsqlCommand(nonActivatedEventsQuery, connection);
            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                list.Add(new DomainEvents
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    Type = reader.GetString(reader.GetOrdinal("Type")),
                    Content = reader.GetString(reader.GetOrdinal("Content")),
                    ActivityAt = reader.IsDBNull(reader.GetOrdinal("ActivityAt"))
                        ? (DateTime?)null
                        : reader.GetDateTime(reader.GetOrdinal("ActivityAt"))
                });
            }

            return list;
        }

        public async Task MarkEventsAsActivatedAsync(IEnumerable<Guid> eventIds, CancellationToken cancellationToken)
        {
            if (!eventIds.Any())
            {
                return;
            }

            var connection = await _connectionProvider.GetOpenConnection(false);

            const string query =
                @"UPDATE ""DomainEvents"" SET ""ActivityAt"" = NOW()  WHERE ""Id"" = ANY(@ids); ";

            await using var cmd = new NpgsqlCommand(query, connection);
            cmd.Parameters.AddWithValue("ids", eventIds.ToArray());

            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task StartListeningNotifyAsync(CancellationToken cancellationToken)
        {
            var _listenerConnection = await _connectionProvider.GetOpenConnection(true);

            var listenCommand = _listenerConnection.CreateCommand();
            listenCommand.CommandText = "LISTEN new_event;";
            await listenCommand.ExecuteNonQueryAsync(cancellationToken);

            _listenerConnection.Notification += (_, _) =>
            {
                EventSignal.Set();
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                await _listenerConnection.WaitAsync(cancellationToken);
            }
        }
    }
}
