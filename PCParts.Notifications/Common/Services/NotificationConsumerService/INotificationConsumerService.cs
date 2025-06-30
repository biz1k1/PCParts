using PCParts.Notifications.Common.MessagesResult;
using PCParts.Notifications.Common.Models;

namespace PCParts.Notifications.Common.Services.NotificationConsumerService
{
    public interface INotificationConsumerService
    {
        public Task<Message> GetNotification(CancellationToken stoppingToken);
        public Task StartConsuming(CancellationToken stoppingToken);
        public Task ConfirmMessageProcessingAsync(MessageResult result, ulong deliveryTag,
            CancellationToken stoppingToken);
    }
}
