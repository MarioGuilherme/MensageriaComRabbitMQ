using EasyNetQ;
using EasyNetQ.Marketing.API.Subscribers;
using MessagingEvents.Shared.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<INotificationService, NotificationService>();

IBus bus = RabbitHutch.CreateBus("host=localhost");
builder.Services.AddSingleton(bus);

builder.Services.AddHostedService<CustomerCreatedSubscriber>();

builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();