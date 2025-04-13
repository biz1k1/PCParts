using System.Reflection;
using System.Text.Json.Serialization;
using PCParts.API.Extension.Middleweares;
using PCParts.API.Extension.Migration;
using PCParts.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = null; })
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); })
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = false; });
;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
});

builder.Services.AddServiceExtensions(builder.Configuration.GetConnectionString("pgsql")!);
builder.Services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));
//builder.Services.AddApiMetrics();
builder.Logging.AddConsole();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    if (!app.Environment.IsEnvironment("Testing")) app.ApplyMigration();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();
//app.MapPrometheusScrapingEndpoint();

app.Run();

public partial class Program
{
}