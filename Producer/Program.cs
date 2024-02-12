using System;
using System.Text;
using Newtonsoft.Json;
using Producer.Model;
using RabbitMQ.Client;

namespace Producer
{
    internal class Program
    {
        static void Main()
        {
            Publish();
        }

        public static void Publish()
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
                channel.QueueDeclare(queue: "barcelona",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var order = new Order()
                {
                    Id = new Guid(),
                    Quantity = 23,
                    UserName = "musakucuk"
                };

                var message = JsonConvert.SerializeObject(order);
                var bodyArr = Encoding.UTF8.GetBytes(message);
               
                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.Escape)
                        break;

                    channel.BasicPublish(exchange: "",
                    routingKey: "orders",
                    basicProperties: null,
                    body: bodyArr);

                    Console.WriteLine($"[x] Sent {0}", bodyArr);
                    Console.WriteLine(" Press [enter] to send a message again.");
                }

            }

        }
    }
}