namespace PCParts.API.Model.ResponseType;

public class ErrorResponseBody
{
    public string type { get; init; }
    public string title { get; init; }
    public int status { get; init; }
    public Guid traceId { get; init; }
}