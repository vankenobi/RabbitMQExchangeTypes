using System;
using System.Text;
using Consumer.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer 
{
    internal class Program
    {
        static void Main()
        {
            Consume();
        }

        public static void Consume()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "user",
                Password = "1234"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "fashion",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Order order = JsonConvert.DeserializeObject<Order>(message);
                    Console.WriteLine($"Id : {order.Id} \nUsername : {order.UserName} \nQuantity : {order.Quantity}");
                };

                channel.BasicConsume(queue: "fashion",
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();


            }
        }
    }
}