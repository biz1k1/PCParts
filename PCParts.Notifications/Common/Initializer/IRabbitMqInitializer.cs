using RabbitMQ.Client;

namespace PCParts.Notifications.Common.Initializer
{
    public interface IRabbitMqInitializer
    {
        Task<IChannel> GetNotificationChannel(CancellationToken stoppingToken);
    }
}
