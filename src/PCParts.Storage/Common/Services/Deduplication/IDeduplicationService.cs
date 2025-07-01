namespace PCParts.Storage.Common.Services.Deduplication;

public interface IDeduplicationService
{
    bool IsDuplicate(string messageId);
}