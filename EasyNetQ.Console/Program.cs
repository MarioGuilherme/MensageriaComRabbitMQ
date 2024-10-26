using EasyNetQ;
using EasyNetQ.Console;
using EasyNetQ.Topology;

Person person = new("Mário Guilherme", "123.456.789-01", new DateTime(2003, 8, 21));
IBus bus = RabbitHutch.CreateBus("host=localhost");

#region Modo mais avançado com mais controle
const string EXCHANGE = "curso-rabbitmq";
const string QUEUE = "person-created";
const string ROUTING_KEY = "hr.person-created";

IAdvancedBus advanced = bus.Advanced;
Exchange exchange = advanced.ExchangeDeclare(EXCHANGE, "topic");
Queue queue = advanced.QueueDeclare(QUEUE);

advanced.Publish(exchange, ROUTING_KEY, true, new Message<Person>(person));

advanced.Consume<Person>(queue, (msg, _) => {
    string json = System.Text.Json.JsonSerializer.Serialize(msg.Body);
    Console.WriteLine(json);
});
#endregion

#region Modo mais simples usando convenção do EasyNetQ
await bus.PubSub.PublishAsync(person);

await bus.PubSub.SubscribeAsync<Person>("marketing", msg => {
    string json = System.Text.Json.JsonSerializer.Serialize(msg);
    Console.WriteLine(json);
});
#endregion

Console.ReadLine();