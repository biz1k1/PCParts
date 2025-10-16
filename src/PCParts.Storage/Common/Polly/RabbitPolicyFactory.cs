using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using Polly;
using Polly.Wrap;
using RabbitMQ.Client.Exceptions;
using PCParts.Shared.Monitoring.Logs;

namespace PCParts.Storage.Common.Polly
{
    public class RabbitMqPolicyFactory : IPolicyFactory
    {
        private readonly ILogger<RabbitMqPolicyFactory> _logger;

        public RabbitMqPolicyFactory(
            ILogger<RabbitMqPolicyFactory> logger)
        {
            _logger = logger;
        }

        public AsyncPolicyWrap<T> GetPolicy<T>()
        {
            var breaker = Policy<T>
                .Handle<BrokerUnreachableException>()
                .Or<AlreadyClosedException>()
                .Or<OperationInterruptedException>()
                .CircuitBreakerAsync(1, TimeSpan.FromMilliseconds(5000));

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

            return Policy.WrapAsync(fallback, breaker);
        }
    }
}
