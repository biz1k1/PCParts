using PCParts.Notifications.Common.MessagesResult;
using PCParts.Notifications.Common.Services.EmailService;
using PCParts.Notifications.Common.Services.NotificationConsumerService;

namespace PCParts.Notifications;

public class NotificationBackground : BackgroundService
{
    private readonly INotificationConsumerService _notificationConsumerService;
    private readonly INotificationSenderService _notificationSenderervice;

    public NotificationBackground(
        INotificationConsumerService notificationConsumerService,
        INotificationSenderService notificationSenderervice)
    {
        _notificationConsumerService = notificationConsumerService;
        _notificationSenderervice = notificationSenderervice;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        await _notificationConsumerService.StartConsuming(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = await _notificationConsumerService.GetNotification(stoppingToken);
                if (!message.Result.IsSuccess)
                {
                    await _notificationConsumerService.ConfirmMessageProcessingAsync(message.Result,
                        message.DeliveryTag,
                        stoppingToken);
                    continue;
                }

                var emailResult = await _notificationSenderervice.Send(message);
                if (!emailResult.IsSuccess)
                {
                    await _notificationConsumerService.ConfirmMessageProcessingAsync(emailResult, message.DeliveryTag,
                        stoppingToken);
                    continue;
                }

                await _notificationConsumerService.ConfirmMessageProcessingAsync(MessageResult.Success(),
                    message.DeliveryTag,
                    stoppingToken);

                await Task.Delay(10, stoppingToken);
            }
            catch (Exception)
            {
            }
        }
    }
}