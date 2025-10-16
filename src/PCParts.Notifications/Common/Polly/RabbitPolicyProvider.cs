using Polly.CircuitBreaker;
using Polly;
using Polly.Wrap;
using RabbitMQ.Client.Exceptions;
using PCParts.Shared.Monitoring.Logs;

namespace PCParts.Notifications.Common.Polly
{
    public class RabbitMqPolicyFactory : IPolicyFactory
    {
        private readonly ILogger<RabbitMqPolicyFactory> _logger;

        private const int RETRY_COUNT = 3;
        private const int BREAKER_FAILURES = 5;
        private static readonly TimeSpan RETRY_DELAY = TimeSpan.FromMilliseconds(200);
        private static readonly TimeSpan BREAKER_DURATION = TimeSpan.FromSeconds(10);

        public RabbitMqPolicyFactory(
            ILogger<RabbitMqPolicyFactory> logger)
        {
            _logger = logger;
        }
        public AsyncPolicyWrap<T> GetPolicy<T>()
        {
            var retry = Policy<T>
                .Handle<BrokerUnreachableException>()
                .Or<AlreadyClosedException>()
                .Or<OperationInterruptedException>()
                .WaitAndRetryAsync(RETRY_COUNT, attempt => RETRY_DELAY);

            var breaker = Policy<T>
                .Handle<BrokerUnreachableException>()
                .Or<AlreadyClosedException>()
                .Or<OperationInterruptedException>()
                .CircuitBreakerAsync(BREAKER_FAILURES, BREAKER_DURATION);

            var fallback = Policy<T>
                .Handle<BrokerUnreachableException>()
                .Or<AlreadyClosedException>()
                .Or<BrokenCircuitException>()
                .Or<OperationInterruptedException>()
                .Or<NullReferenceException>()
                .FallbackAsync(
                    fallbackValue: default!,
                    onFallbackAsync: async (outcome, context) =>
                    {
                        _logger.LogErrorException(nameof(RabbitMqPolicyFactory), outcome.Exception.Message);
                        await Task.CompletedTask;
                    }
                );

            return Policy.WrapAsync(fallback, retry, breaker);
        }
    }
}
