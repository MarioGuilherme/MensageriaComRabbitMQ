using MessagingEvents.Shared;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace RabbitMQClient.Marketing.API.Subscribers;

public class CustomerCreatedSubscriber : IHostedService {
    private readonly IModel _channel;

    const string CUSTOMER_CREATED_QUEUE = "costumer-created";

    public CustomerCreatedSubscriber() {
        ConnectionFactory connectionFactory = new() {
            HostName = "localhost"
        };

        IConnection connection = connectionFactory.CreateConnection("curso-rabbitmq-client-consumer");

        this._channel = connection.CreateModel();
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        EventingBasicConsumer consumer = new(this._channel);

        consumer.Received += (_, eventArgs) => {
            byte[] bytes = eventArgs.Body.ToArray();
            string contentString = Encoding.UTF8.GetString(bytes);

            CustomerCreated @event = JsonSerializer.Deserialize<CustomerCreated>(contentString)!;

            Console.WriteLine($"Message received: {contentString}");

            this._channel.BasicAck(eventArgs.DeliveryTag, false); // Confirma que recebeu a mensagem
        };

        this._channel.BasicConsume(CUSTOMER_CREATED_QUEUE, false, consumer);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}