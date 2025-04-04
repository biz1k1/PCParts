using System.Reflection;
using PCParts.API.Extension.Middleweares;
using PCParts.API.Extension.Migration;
using PCParts.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = null; })
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = false; });
;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.ApplyMigration();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();
//app.MapPrometheusScrapingEndpoint();

app.Run();

public partial class Program
{
}