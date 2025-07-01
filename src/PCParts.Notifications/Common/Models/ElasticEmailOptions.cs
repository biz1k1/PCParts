namespace PCParts.Notifications.Common.Models;

public class ElasticEmailOptions
{
    public string ApiKey { get; set; }
    public string To { get; set; }
    public string From { get; set; }
    public string Template { get; set; }
}