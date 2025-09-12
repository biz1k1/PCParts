using Polly;
using Polly.CircuitBreaker;
using Polly.Wrap;
using StackExchange.Redis;

namespace PCParts.Storage.Common.Polly
{
    public class RedisPolicyFactory : IPolicyFactory
    {
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
                .FallbackAsync(
                    fallbackValue: default!, onFallbackAsync: _ => Task.CompletedTask
                );

            return Policy.WrapAsync(fallback, retry, breaker);
        }
    }
}
