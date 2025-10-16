using System.Reflection;
using System.Text.Json.Serialization;
using PCParts.API.Extension.Middlewares;
using PCParts.DependencyInjection;
using PCParts.Storage.Common.Extensions.Migration;
using PCParts.Shared.Monitoring.Logs;
using PCParts.Shared.Monitoring.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLogging(builder.Environment)
    .AddAppMetrics();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = false; });
;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
});
builder.Services.AddServiceExtensions(builder.Configuration);
builder.Services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.ApplyMigrationAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<CachingMiddleware>();
app.MapControllers();
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.Run();

public partial class Program
{
}
