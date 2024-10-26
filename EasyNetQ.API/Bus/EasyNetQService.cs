using EasyNetQ.Topology;

namespace EasyNetQ.Customers.API.Bus;

public class EasyNetQService(IBus bus) : IBusService {
    private readonly IAdvancedBus _bus = bus.Advanced;
    private const string EXCHANGE = "curso-rabbitmq";

    public void Publish<T>(string routingKey, T message) {
        Exchange exchange = this._bus.ExchangeDeclare(EXCHANGE, "topic");
        this._bus.Publish(exchange, routingKey, true, new Message<T>(message));
    }
}