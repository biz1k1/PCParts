using System.Diagnostics;

namespace PCParts.Shared.Monitoring.Metrics
{
    public interface IAppMetrics
    {
        void IncrementCount(string name, int value, IDictionary<string, object?>? additionalTags = null);
        IDictionary<string, object?> CreateResultTags(bool success);
    }
}
