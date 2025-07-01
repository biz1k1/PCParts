using OpenTelemetry.Metrics;

namespace PCParts.API.Monitoring;

internal static class MetricServiceCollectionExtensions
{
    public static IServiceCollection AddApiMetrics(this IServiceCollection service)
    {
        service
            .AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddPrometheusExporter()
                .AddConsoleExporter()
                .AddView("http.server.request.duration", new ExplicitBucketHistogramConfiguration
                {
                    Boundaries = new[] { 0, 0.05, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 10 }
                }));
        return service;
    }
}