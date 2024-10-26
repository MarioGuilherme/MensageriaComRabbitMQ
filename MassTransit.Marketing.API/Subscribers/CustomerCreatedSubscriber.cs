using MessagingEvents.Shared;
using MessagingEvents.Shared.Services;

namespace MassTransit.Marketing.API.Subscribers;

public class CustomerCreatedSubscriber(IServiceProvider serviceProvider) : IConsumer<CustomerCreated> {
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public async Task Consume(ConsumeContext<CustomerCreated> context) {
        CustomerCreated @event = context.Message;

        using IServiceScope scope = this.ServiceProvider.CreateScope();
        INotificationService service = scope.ServiceProvider.GetRequiredService<INotificationService>();

        await service.SendEmail(@event.Email, "boas-vindas", new Dictionary<string, string> { { "name", @event.FullName } });
    }
}