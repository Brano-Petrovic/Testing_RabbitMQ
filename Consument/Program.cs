using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using System;
using System.Text;
using System.Collections.Generic;

namespace Consument
{
    class Program
    {
        static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var conn = factory.CreateConnection();
            var channel = conn.CreateModel();

            channel.QueueDeclare(queue: "testQueue", durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(channel);
            var helpList = new List<string>();
            consumer.Received += (s, e) =>
            {
                //consume
                //var msg = Encoding.UTF8.GetString(e.Body.ToArray());
                //Console.WriteLine(msg);
                //channel.BasicAck(e.DeliveryTag, false);

                //Thread.Sleep(800);

                var msg = Encoding.UTF8.GetString(e.Body.ToArray());
                helpList.Add(msg);
                channel.BasicAck(e.DeliveryTag, false);

                if (helpList.Count == 5)
                {
                    foreach (var i in helpList)
                        Console.WriteLine(i);
                    helpList.Clear();
                }

                Thread.Sleep(800);
            };
            channel.BasicConsume("testQueue", false, consumer);
            Console.ReadLine();
        }
    }
}
