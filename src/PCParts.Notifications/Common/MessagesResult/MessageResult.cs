namespace PCParts.Notifications.Common.MessagesResult;

public class MessageResult
{
    public Result Status { get; init; }
    public string? ErrorMessage { get; init; }

    public bool IsRetry => Status == Result.TransientFailure;
    public bool IsSuccess => Status == Result.Success;
    public bool IsDeadLetter => Status == Result.PermanentFailure;

    public static MessageResult Success() =>
        new() { Status = Result.Success };

    public static MessageResult TransientFailure(string? message = null) =>
        new() { Status = Result.TransientFailure, ErrorMessage = message };

    public static MessageResult PermanentFailure(string? message = null) =>
        new() { Status = Result.PermanentFailure, ErrorMessage = message };
}

public enum Result
{
    Success,
    TransientFailure,
    PermanentFailure
}