using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQClient.Customers.API.Bus;

public class RabbitMqClientService : IBusService {
    const string EXCHANGE = "curso-rabbitmq";
    private readonly IModel _channel;

    public RabbitMqClientService() {
        ConnectionFactory connectionFactory = new() {
            HostName = "localhost"
        };

        IConnection connection = connectionFactory.CreateConnection("curso-rabbitmq-client-publisher");

        this._channel = connection.CreateModel();
    }

    public void Publish<T>(string routingKey, T message) {
        string json = JsonSerializer.Serialize(message);

        byte[] bytes = Encoding.UTF8.GetBytes(json);

        this._channel.BasicPublish(EXCHANGE, routingKey, null, bytes);
    }
}