using System.Diagnostics.Metrics;

namespace PCParts.Application.Monitoring;

public class DomainMetrics
{
    private readonly Counter<int> _categoryfetched;

    public DomainMetrics()
    {
        var meter = new Meter("PCParts.Domain");
        _categoryfetched = meter.CreateCounter<int>("category_fetched");
    }

    public void CategoryFetched()
    {
        _categoryfetched.Add(1);
    }
}
