using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace PCParts.Application.Monitoring;

public class DomainMetrics(IMeterFactory meterFactory): IDomainMetrics
{
    private const string ApplicationName = "PCParts.Applicatio";

    private readonly Meter meter = meterFactory.Create(ApplicationName);
    private readonly ConcurrentDictionary<string, Counter<int>> counters = new();

    internal static readonly ActivitySource ActivitySource = new(ApplicationName);

    public void IncrementCount(string name, int value, IDictionary<string, object?>? additionalTags = null)
    {
        var counter = counters.GetOrAdd(name, _ => meter.CreateCounter<int>(name));
        counter.Add(value, additionalTags?.ToArray() ?? ReadOnlySpan<KeyValuePair<string, object?>>.Empty);
    }

    public IDictionary<string, object?> CreateResultTags(bool success) =>
        new Dictionary<string, object?> { ["success"] = success };

    public static IDictionary<string, object?> ResultTags(bool success) => new Dictionary<string, object?>
    {
        ["success"] = success
    };
}
