using PCParts.Notifications.Common.MessagesResult;
using PCParts.Notifications.Common.Models;
using PCParts.Notifications.Common.Services.EmailService;
using PCParts.Notifications.Common.Services.NotificationConsumerService;
using PCParts.Shared.Monitoring.Logs;
using PCParts.Shared.Monitoring.Metrics;

namespace PCParts.Notifications;

public class NotificationBackground : BackgroundService
{
    private readonly INotificationConsumerService _notificationConsumerService;
    private readonly INotificationSenderService _notificationSenderervice;
    private readonly ILogger<NotificationBackground> _logger;

    public NotificationBackground(
        INotificationConsumerService notificationConsumerService,
        INotificationSenderService notificationSenderervice,
        ILogger<NotificationBackground> logger
        )
    {
        _notificationConsumerService = notificationConsumerService;
        _notificationSenderervice = notificationSenderervice;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        try
        {
            await _notificationConsumerService.StartConsuming(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                Message message = await _notificationConsumerService.GetNotification(stoppingToken);
                if (!message.Result.IsSuccess)
                {
                    await _notificationConsumerService.ConfirmMessageProcessingAsync(message.Result,
                        message.DeliveryTag,
                        stoppingToken);
                    continue;
                }

                MessageResult emailResult = await _notificationSenderervice.Send(message);
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
        }

        catch (Exception ex)
        {
            _logger.LogCriticalException(nameof(NotificationBackground), ex.Message, ex);
        }
    }
}
