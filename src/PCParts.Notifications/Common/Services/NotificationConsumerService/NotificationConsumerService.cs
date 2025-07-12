using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using PCParts.Notifications.Common.Initializer;
using PCParts.Notifications.Common.MessagesResult;
using PCParts.Notifications.Common.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace PCParts.Notifications.Common.Services.NotificationConsumerService;

public class NotificationConsumerService : INotificationConsumerService
{
    private const int RETRY_COUNT = 5;
    private readonly Channel<Message> _messageChannel;
    private readonly IRabbitMqInitializer _rabbitMqInitializer;
    private IChannel _channel;

    public NotificationConsumerService(
        IRabbitMqInitializer rabbitMqInitializer)
    {
        _rabbitMqInitializer = rabbitMqInitializer;
        _messageChannel = Channel.CreateBounded<Message>(new BoundedChannelOptions(capacity: 1000)
        {
            FullMode = BoundedChannelFullMode.Wait
        });
    }

    public async Task StartConsuming(CancellationToken stoppingToken)
    {
        _channel = await _rabbitMqInitializer.GetNotificationChannel(stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var message = new Message();
            try
            {
                var body = ea.Body.ToArray();
                message.Body = Encoding.UTF8.GetString(body);
                message.DeliveryTag = ea.DeliveryTag;

                var headers = ea.BasicProperties?.Headers;
                if (headers != null && headers.TryGetValue("x-death", out var xDeathRaw)
                                    && xDeathRaw is List<object> xDeathList
                                    && xDeathList.FirstOrDefault() is Dictionary<string, object> deathInfo
                                    && deathInfo.TryGetValue("count", out var countObj))
                {
                    var retryCount = 0;
                    retryCount = Convert.ToInt32(countObj);

                    if (retryCount > RETRY_COUNT)
                    {
                        await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false,
                            cancellationToken: stoppingToken);
                        return;
                    }
                }

                message.Result = MessageResult.Success();
                await _messageChannel.Writer.WriteAsync(message, stoppingToken);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is SocketException)
            {
                message.Result = MessageResult.TransientFailure(ex.ToString());
                await _messageChannel.Writer.WriteAsync(message, stoppingToken);
            }
            catch (Exception ex) when (ex is AlreadyClosedException || ex is OperationInterruptedException)
            {
                message.Result = MessageResult.TransientFailure(ex.ToString());
                await _messageChannel.Writer.WriteAsync(message, stoppingToken);
            }
            catch (Exception ex)
            {
                message.Result = MessageResult.PermanentFailure(ex.ToString());
                await _messageChannel.Writer.WriteAsync(message, stoppingToken);
            }
        };
        await _channel.BasicConsumeAsync(queue: "Notification.sms.events", autoAck: false,
            consumer: consumer, cancellationToken: stoppingToken);
    }


    public async Task<Message> GetNotification(CancellationToken stoppingToken)
    {
        return await _messageChannel.Reader.ReadAsync(stoppingToken);
    }

    public async Task ConfirmMessageProcessingAsync(MessageResult result, ulong deliveryTag,
        CancellationToken stoppingToken)
    {
        switch (result.Status)
        {
            case Result.Success:
                await _channel.BasicAckAsync(deliveryTag, false, stoppingToken);
                break;

            case Result.TransientFailure:
                await _channel.BasicNackAsync(deliveryTag, false, true, stoppingToken);
                break;

            case Result.PermanentFailure:
                await _channel.BasicNackAsync(deliveryTag, false, false, stoppingToken);
                break;
        }
    }
}