namespace PCParts.API.Model.ResponseType;

public class ValidationResponseBody
{
    public string type { get; init; }
    public string title { get; init; }
    public int status { get; init; }
    public Dictionary<string, string[]> errors { get; init; }
    public Guid traceId { get; init; }
}