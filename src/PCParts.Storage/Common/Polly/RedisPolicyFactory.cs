using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Wrap;
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
            var retry = Policy<T>
                .Handle<RedisConnectionException>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200));

            var breaker = Policy<T>
                .Handle<RedisConnectionException>()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(10));

            var fallback = Policy<T>
                .Handle<RedisConnectionException>()
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

            return Policy.WrapAsync(fallback, retry, breaker);
        }
    }
}
