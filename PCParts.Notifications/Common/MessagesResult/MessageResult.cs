namespace PCParts.Notifications.Common.MessagesResult;

public class MessageResult
{
    public ProcessingResult Status { get; init; }
    public string? ErrorMessage { get; init; }

    public bool ShouldRetry => Status == ProcessingResult.TransientFailure;
    public bool ShouldSuccess => Status == ProcessingResult.Success;
    public bool ShouldDeadLetter => Status == ProcessingResult.PermanentFailure;

    public static MessageResult Success() =>
        new() { Status = ProcessingResult.Success };

    public static MessageResult TransientFailure(string? message = null) =>
        new() { Status = ProcessingResult.TransientFailure, ErrorMessage = message };

    public static MessageResult PermanentFailure(string? message = null) =>
        new() { Status = ProcessingResult.PermanentFailure, ErrorMessage = message };
}