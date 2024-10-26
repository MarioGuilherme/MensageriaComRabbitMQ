namespace MassTransit.Customers.API.Bus;

public class MassTransitBusService(IBus bus) : IBusService {
    private readonly IBus _bus = bus;

    public Task Publish<T>(T message) => this._bus.Publish(message!);
}