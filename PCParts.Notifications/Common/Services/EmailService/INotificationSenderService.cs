using PCParts.Notifications.Common.MessagesResult;
using PCParts.Notifications.Common.Models;

namespace PCParts.Notifications.Common.Services.EmailService;

public interface INotificationSenderService
{
    public Task<MessageResult> Send(Message message);
}