using System.Text.Json;
using Microsoft.Extensions.Options;
using PCParts.Notifications.Common.MessagesResult;
using PCParts.Notifications.Common.Models;

namespace PCParts.Notifications.Common.Services.EmailService;

public class NotificationSenderService : INotificationSenderService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ElasticEmailOptions _options;

    public NotificationSenderService(
        IHttpClientFactory iHttpClientFactory,
        IOptions<ElasticEmailOptions> options)
    {
        _httpClientFactory = iHttpClientFactory;
        _options = options.Value;
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
        catch (HttpRequestException ex)
        {
            return MessageResult.TransientFailure(ex.Message);
        }
        catch (TaskCanceledException ex)
        {
            return MessageResult.TransientFailure(ex.Message);
        }
        catch (JsonException ex)
        {
            return MessageResult.PermanentFailure(ex.Message);
        }
        catch (Exception ex)
        {
            return MessageResult.PermanentFailure(ex.Message);
        }

        return MessageResult.Success();
    }
}