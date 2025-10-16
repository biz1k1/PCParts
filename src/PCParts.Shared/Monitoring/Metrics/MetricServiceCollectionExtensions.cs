using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PCParts.Shared.Monitoring.Metrics;

public static class MetricServiceCollectionExtensions
{
    public static IServiceCollection AddAppMetrics(this IServiceCollection service)
    {
        service
            .AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddNpgsqlInstrumentation()
                .AddPrometheusExporter()
                .AddMeter("Microsoft.EntityFrameworkCore")
                .AddMeter("PCParts.API")
                .AddMeter("PCParts.Notification")
                .AddPrometheusExporter(options =>
                {
                    options.ScrapeEndpointPath = "/internal/metrics";
                    options.DisableTotalNameSuffixForCounters = true;
                }))
            .WithTracing(builder => builder
                .ConfigureResource(r => r.AddService("PCParts.API"))
                .AddSource("PCParts.API")
                .AddSource("PCParts.Notification")
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.Filter = context =>
                        !context.Request.Path.Value!.Contains("metrics", StringComparison.InvariantCultureIgnoreCase) &&
                        !context.Request.Path.Value!.Contains("swagger", StringComparison.InvariantCultureIgnoreCase);
                    options.EnrichWithHttpResponse = (activity, response) =>
                        activity.AddTag("error", response.StatusCode >= 400);
                }));
        return service;
    }
}
