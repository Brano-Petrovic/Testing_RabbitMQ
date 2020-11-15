using RabbitMQ.Client;
using System.Threading;
using System;
using System.Text;

namespace Testing_RabbitMQ
{
    class Program
    {
        static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("testExchange", type: ExchangeType.Fanout, durable: true, autoDelete: false) ;
            channel.QueueDeclare(queue: "testQueue",durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind("testQueue", "testExchange", string.Empty);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            int count = 0;
            var number = new Random();

            while (true)
            {
                count++;
                
                var msg = $"Testing message number:{count}. Value {number.Next(-100, 100)}.";
                Console.WriteLine(msg);
                
                channel.BasicPublish("testExchange", 
                    string.Empty, 
                    properties,
                    Encoding.UTF8.GetBytes(msg)
                    );
                Thread.Sleep(500);
            }
        }
    }
}
