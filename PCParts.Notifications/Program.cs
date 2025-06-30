using PCParts.Notifications;
using PCParts.Notifications.Common.Initializer;
using PCParts.Notifications.Common.Initializer.Connection;
using PCParts.Notifications.Common.Models;
using PCParts.Notifications.Common.Services.EmailService;
using PCParts.Notifications.Common.Services.NotificationConsumerService;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ElasticEmailOptions>(builder.Configuration.GetSection("ElasticEmail"));
builder.Services.AddHostedService<NotificationBackground>();
builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddSingleton<IRabbitMqInitializer, RabbitMqInitializer>();
builder.Services.AddSingleton<INotificationSenderService, NotificationSenderService>();
builder.Services.AddSingleton<INotificationConsumerService, NotificationConsumerService>();
builder.Services.AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
{
    HostName = "rabbitmq",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
});
builder.Services.AddHttpClient("ElasticEmail.client",
        client => { client.BaseAddress = new Uri("https://api.elasticemail.com/v2/email/send"); })
    .UseSocketsHttpHandler((handler, _) =>
    {
        handler.PooledConnectionLifetime = TimeSpan.FromMinutes(15);
        handler.PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2);
    })
    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);


var app = builder.Build();

app.Run();