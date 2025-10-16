using System.Text.Json;
using Microsoft.Extensions.Options;
using PCParts.Notifications.Common.MessagesResult;
using PCParts.Notifications.Common.Models;
using PCParts.Shared.Monitoring.Logs;

namespace PCParts.Notifications.Common.Services.EmailService;

public class NotificationSenderService : INotificationSenderService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ElasticEmailOptions _options;
    private readonly ILogger<NotificationSenderService> _logger;

    public NotificationSenderService(
        IHttpClientFactory iHttpClientFactory,
        IOptions<ElasticEmailOptions> options,
        ILogger<NotificationSenderService> logger)
    {
        _httpClientFactory = iHttpClientFactory;
        _options = options.Value;
        _logger = logger;
        _httpClient = _httpClientFactory.CreateClient();
    }

    public async Task<MessageResult> Send(Message message)
    {
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "apikey", _options.ApiKey },
            { "from", _options.From },
            { "to", message.Body },
            { "template", _options.Template }
        });

        try
        {
            var response = await _httpClient.PostAsync("", content);
            if (!response.IsSuccessStatusCode)
            {
                return MessageResult.PermanentFailure($"API error: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
        }
        catch(Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            _logger.LogErrorException(nameof(NotificationSenderService), ex.Message, ex);
            return MessageResult.TransientFailure(ex.Message);
        }
        catch (Exception ex) when (ex is JsonException or Exception)
        {
            _logger.LogCriticalException(nameof(NotificationSenderService), ex.Message, ex);
            return MessageResult.PermanentFailure(ex.Message);
        }

        return MessageResult.Success();
    }
}
