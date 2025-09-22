using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Wrap;
using RabbitMQ.Client.Exceptions;
using StackExchange.Redis;

namespace PCParts.Storage.Common.Polly
{
    public class RedisPolicyFactory : IPolicyFactory
    {
        private readonly ILogger<RedisPolicyFactory> _logger;

        public RedisPolicyFactory(
            ILogger<RedisPolicyFactory> logger)
        {
            _logger = logger;
        }

        public AsyncPolicyWrap<T> GetPolicy<T>()
        {

            var breaker = Policy<T>
                .Handle<RedisConnectionException>()
                .Or<AlreadyClosedException>()
                .CircuitBreakerAsync(1, TimeSpan.FromMilliseconds(5000));

            var fallback = Policy<T>
                .Handle<RedisConnectionException>()
                .Or<AlreadyClosedException>()
                .Or<BrokenCircuitException>()
                .Or<NullReferenceException>()
                .FallbackAsync(
                    fallbackValue: default(T)!,
                    onFallbackAsync: async (outcome, context) =>
                    {
                        _logger.LogError(outcome.Exception, "Redis call failed, fallback executed");
                        await Task.CompletedTask;
                    }
                );

            return Policy.WrapAsync(fallback, breaker);
        }
    }
}
