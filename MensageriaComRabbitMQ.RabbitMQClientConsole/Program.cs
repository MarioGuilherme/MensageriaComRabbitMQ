using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

ConnectionFactory connectionFactory = new() {
    HostName = "localhost"
};
IConnection connection = connectionFactory.CreateConnection("curso-rabbitmq123");

#region Enviar mensagens
const string EXCHANGE = "curso-rabbitmq";
const string ROUTING_KEY = "hr.person-created";

Person person = new("Mário Guilherme", "123.456.789-01", new DateTime(2003, 8, 21));

IModel channel = connection.CreateModel();

string json = JsonSerializer.Serialize(person);
byte[] byteArray = Encoding.UTF8.GetBytes(json);
channel.BasicPublish(EXCHANGE, ROUTING_KEY, null, byteArray);

Console.WriteLine($"Message published: {json}");
#endregion

#region Consumir mensagens
const string QUEUE = "person-created";

IModel consumerChannel = connection.CreateModel();
EventingBasicConsumer consumer = new(consumerChannel);

consumer.Received += (_, eventArgs) => {
    byte[] contentArray = eventArgs.Body.ToArray();
    string contentString = Encoding.UTF8.GetString(contentArray);

    Person message = JsonSerializer.Deserialize<Person>(contentString)!;

    Console.WriteLine($"Message received: {contentString}");

    consumerChannel.BasicAck(eventArgs.DeliveryTag, false); // Confirma que recebeu a mensagem
};

consumerChannel.BasicConsume(QUEUE, false, consumer);// Não indicado fazer o ack automático
#endregion

Console.ReadLine();