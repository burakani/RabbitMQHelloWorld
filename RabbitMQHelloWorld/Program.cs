using RabbitMQ.Client;
using System.Text;

var connectionFactory = new ConnectionFactory()
{
    Uri = new Uri("amqps://mqofvyex:eQ6zQEZLhXyoA__cjngHeCPQ7mkgWZXj@moose.rmq.cloudamqp.com/mqofvyex"),
};

using (var connection = connectionFactory.CreateConnection())
{
    var channel = connection.CreateModel();

    channel.QueueDeclare("hello-world-queue", durable: true, exclusive: false, autoDelete: false);

    for (int i = 0; i < 50; i++)
    {
        var message = $"Message: {i}";

        var messageBody = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(string.Empty, "hello-world-queue", null, messageBody);

        Console.WriteLine($"Message {i} sent!");
    }
}

Console.WriteLine("All messages sent");

Console.ReadLine();