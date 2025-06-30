using PCParts.Notifications.Common.MessagesResult;

namespace PCParts.Notifications.Common.Models;

public class Message
{
    public string Body { get; set; }
    public MessageResult Result { get; set; }
    public ulong DeliveryTag { get; set; }
}