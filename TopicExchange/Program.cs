using System;
using System.Text;
using RabbitMQ.Client;

namespace TopicExchange  
{
    public class Program
    {
        static void Main()
        {
            TopicExchange();
        }

        public static void TopicExchange()
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
                channel.ExchangeDeclare("topic_logs",
                                        type: ExchangeType.Topic);

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

                channel.QueueDeclare(queue: "politica",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueBind(queue: "technology",
                                  exchange: "topic_logs",
                                  routingKey: "technology.*");

                channel.QueueBind(queue: "sport",
                                  exchange: "topic_logs",
                                  routingKey: "*.sport.*");

                channel.QueueBind(queue: "politica",
                                  exchange: "topic_logs",
                                  routingKey: "#.politica");


                string message = "new tech content published.";
                var bodyArr = Encoding.UTF8.GetBytes(message);

                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.Escape)
                        break;

                    channel.BasicPublish(exchange: "topic_logs",
                                        routingKey: "technology.news",
                                        basicProperties: null,
                                        body: bodyArr);

                    Console.WriteLine("[x] sent '{0}' : {1}", "technology.news", message);

                }


            }
        }
    }
}