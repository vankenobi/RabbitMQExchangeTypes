using System;
using System.Text;
using RabbitMQ.Client;

namespace DirectExchange 
{
    public class Program
    {
        static void Main()
        {
            DirectExchangeProducer();
        }

        public static void DirectExchangeProducer()
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
                channel.ExchangeDeclare(exchange: "direct_logs",
                                        type: ExchangeType.Direct);

                // Create exchanges for categories
                channel.QueueDeclare(queue: "technology",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                channel.QueueDeclare(queue: "sport",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                channel.QueueDeclare(queue: "fashion",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                // Binding Queues with exchange

                channel.QueueBind(queue: "technology",
                              exchange: "direct_logs",
                              routingKey: "technology");

                channel.QueueBind(queue: "sport",
                                  exchange: "direct_logs",
                                  routingKey: "sport");

                channel.QueueBind(queue: "fashion",
                                  exchange: "direct_logs",
                                  routingKey: "fashion");

                string message = "News about technology";
                var bodyArr = Encoding.UTF8.GetBytes(message);

                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.Escape)
                        break;

                    channel.BasicPublish(exchange: "direct_logs",
                                     routingKey: "sport",
                                     basicProperties: null,
                                     body: bodyArr);

                    Console.WriteLine(" [x] Sent '{0}':'{1}'", "sport", message);

                    channel.BasicPublish(exchange: "direct_logs",
                                     routingKey: "technology",
                                     basicProperties: null,
                                     body: bodyArr);

                    Console.WriteLine(" [x] Sent '{0}':'{1}'", "technology", message);

                    channel.BasicPublish(exchange: "direct_logs",
                                     routingKey: "fashion",
                                     basicProperties: null,
                                     body: bodyArr);

                    Console.WriteLine(" [x] Sent '{0}':'{1}'", "fashion", message);

                }

            }

        }
    }


}