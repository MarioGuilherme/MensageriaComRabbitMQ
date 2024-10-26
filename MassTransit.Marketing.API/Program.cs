using MassTransit;
using MassTransit.Marketing.API.Subscribers;
using MessagingEvents.Shared.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddMassTransit(c => {
    c.AddConsumer<CustomerCreatedSubscriber>();

    c.UsingRabbitMq((context, config) => {
        config.ConfigureEndpoints(context);
    });
});

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