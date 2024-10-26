using EasyNetQ;
using EasyNetQ.Customers.API.Bus;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IBus bus = RabbitHutch.CreateBus("host=localhost");
builder.Services.AddSingleton<IBusService, EasyNetQService>(_ => new(bus));

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