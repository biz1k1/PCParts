using System.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Grafana.Loki;
namespace PCParts.API.Monitoring
{
    internal static class LoggingServiceCollectionExtensions
    {
        public static IServiceCollection AddApiLogging(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment environment) =>
            services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Application", "PCParts.API")
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .Enrich.With<TraceEnrich>()
                    .WriteTo.GrafanaLoki(
                        configuration["Logs:Loki"]!,
                        propertiesAsLabels: ["Application", "Environment"]))
                .CreateLogger()));
    }

    sealed internal class TraceEnrich : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var activity = Activity.Current ?? default;
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", activity?.TraceId));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", activity?.SpanId));
        }
    }
}
