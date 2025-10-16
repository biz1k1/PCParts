using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PCParts.Shared.Monitoring.Metrics;

public class AppMetrics : IAppMetrics, IDisposable
{
    private readonly string _applicationName;
    private readonly Meter _meter;

    private readonly ConcurrentDictionary<string, Counter<int>> counters = new();

    private readonly ActivitySource _activitySource;


    [MethodImpl(MethodImplOptions.NoInlining)]
    public AppMetrics(
        IMeterFactory meterFactory)
    {
        _applicationName = Assembly.GetEntryAssembly().GetName().Name ?? "The Assembly is not defined";

        _meter = meterFactory.Create(_applicationName);
        _activitySource = new(_applicationName);
    }

    public void IncrementCount(string name, int value, IDictionary<string, object?>? additionalTags = null)
    {
        var counter = counters.GetOrAdd(name, _ => _meter.CreateCounter<int>(name));
        counter.Add(value, additionalTags?.ToArray() ?? ReadOnlySpan<KeyValuePair<string, object?>>.Empty);
    }

    public IDictionary<string, object?> CreateResultTags(bool success) =>
        new Dictionary<string, object?> { ["success"] = success };

    public static IDictionary<string, object?> ResultTags(bool success) =>
        new Dictionary<string, object?> { ["success"] = success };

    public void Dispose()
    {
        _activitySource.Dispose();
        GC.SuppressFinalize(this);
    }
}
