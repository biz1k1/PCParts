namespace PCParts.Application.Monitoring
{
    interface IDomainMetrics
    {
        void IncrementCount(string name, int value, IDictionary<string, object?>? additionalTags = null);
        IDictionary<string, object?> CreateResultTags(bool success);
    }
}
