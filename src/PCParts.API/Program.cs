using System.Reflection;
using System.Text.Json.Serialization;
using PCParts.API.Extension.Middleweares;
using PCParts.API.Extension.Migration;
using PCParts.API.Monitoring;
using PCParts.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiLogging(builder.Configuration, builder.Environment);

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
    if (!app.Environment.IsEnvironment("Testing")) await app.ApplyMigrationAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
//app.MapPrometheusScrapingEndpoint();

app.Run();

public partial class Program
{
}
