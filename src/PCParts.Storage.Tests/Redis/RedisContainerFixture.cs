using Microsoft.Extensions.Options;
using PCParts.Storage.Common.Polly;
using PCParts.Storage.Redis;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace PCParts.Storage.Tests.Redis
{
    public class RedisContainerFixture : IAsyncLifetime
    {
        private readonly RedisContainer _container = new RedisBuilder().Build();
        private ConnectionMultiplexer? _connection;
        private readonly IEnumerable<IPolicyFactory> policies;

        public RedisCacheService? Service { get; private set; }
        public async Task InitializeAsync()
        {
            await _container.StartAsync();

            _connection = await ConnectionMultiplexer.ConnectAsync(
                _container.GetConnectionString()
            );


            var options = Options.Create(new RedisCacheOptions
            {
                DefaultTtl = TimeSpan.FromMinutes(10)
            });

            Service = new RedisCacheService(_connection, options, policies);
        }

        public async Task DisposeAsync()
        {
            await (_connection?.DisposeAsync() ?? default);
            await _container.DisposeAsync();
        }
    }
}
