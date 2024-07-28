using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var connectionFactory = new ConnectionFactory()
{
    Uri = new Uri("amqps://mqofvyex:eQ6zQEZLhXyoA__cjngHeCPQ7mkgWZXj@moose.rmq.cloudamqp.com/mqofvyex"),
};

using (var connection = connectionFactory.CreateConnection())
{
    var channel = connection.CreateModel();

    //You can delete the following line, if you or publisher have already created the queue
    //If you create the queue here, you should have the same configuration as the publisher
    channel.QueueDeclare("hello-world-queue", durable: true, exclusive: false, autoDelete: false);

    //prefetchCount is set to 1, so the consumer will only receive one message at a time
    //If you set it to 0, the consumer will receive all messages at once
    //prefetchSize is set to 0, so the consumer will receive messages regardless of their size
    //global is set to false, so the prefetchCount will be applied to the consumer channel only
    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    var consumer = new EventingBasicConsumer(channel);

    //AutoAck is set to false, so we can manually acknowledge the message and remove it from the queue
    //If you set it to true, the message will be removed from the queue as soon as it is received
    channel.BasicConsume("hello-world-queue", autoAck: false, consumer);

    consumer.Received += (sender, eventArgs) =>
    {
        var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

        Thread.Sleep(1000);

        Console.WriteLine($"Message received: {message}");

        //Multiple is set to false, so only the message with the specified delivery tag will be acknowledged
        channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
    };

    //We can put in a loop to keep the console application running and keep connection open
    Console.ReadLine();
}

