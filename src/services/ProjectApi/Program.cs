using Contracts;
using ProjectApi.EventHandling;
using RabbitMQ.Client;
using ServiceBus;
using ServiceBus.Abstractions;
using ServiceBus.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

var eventBusSection = builder.Configuration.GetSection("EventBus");
builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration.GetConnectionString("EventBus"),
        DispatchConsumersAsync = true
    };
    
    if (!string.IsNullOrEmpty(eventBusSection["UserName"]))
    {
        factory.UserName = eventBusSection["UserName"];
    }

    if (!string.IsNullOrEmpty(eventBusSection["Password"]))
    {
        factory.Password = eventBusSection["Password"];
    }

    var retryCount = eventBusSection.GetValue("RetryCount", 5);

    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
});

builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
{
    var subscriptionClientName = eventBusSection.GetValue<string>("SubscriptionClientName");
    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
    var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
    var retryCount = eventBusSection.GetValue("RetryCount", 5);

    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubscriptionsManager, subscriptionClientName, retryCount);
});

builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
builder.Services.AddTransient<ProjectEventHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<ProjectEvent, ProjectEventHandler>();

await app.RunAsync();
