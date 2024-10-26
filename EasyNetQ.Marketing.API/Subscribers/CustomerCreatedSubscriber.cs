using EasyNetQ.Topology;
using MessagingEvents.Shared;
using MessagingEvents.Shared.Services;
using Newtonsoft.Json;

namespace EasyNetQ.Marketing.API.Subscribers;

public class CustomerCreatedSubscriber(IServiceProvider services, IBus bus) : IHostedService {
    const string CUSTOMER_CREATED_QUEUE = "customer-created";

    private readonly IAdvancedBus _bus = bus.Advanced;

    public IServiceProvider Services { get; } = services;

    public Task StartAsync(CancellationToken cancellationToken) {
        Queue queue = this._bus.QueueDeclare(CUSTOMER_CREATED_QUEUE, cancellationToken: cancellationToken);

        this._bus.Consume<CustomerCreated>(queue, async (msg, _) => {
            string json = JsonConvert.SerializeObject(msg.Body);
            await this.SendEmail(msg.Body);
            Console.WriteLine($"Message Received: {json}");
        });

        return Task.CompletedTask;
    }

    public async Task SendEmail(CustomerCreated @event) {
        using IServiceScope scope = this.Services.CreateScope();
        INotificationService service = scope.ServiceProvider.GetRequiredService<INotificationService>();
        await service.SendEmail(@event.Email, CUSTOMER_CREATED_QUEUE, new() { { "name", @event.FullName } });
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}