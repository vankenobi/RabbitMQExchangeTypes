using System;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;

namespace Fanout 
{
    public class Program
    {
        static void Main()
        {
            FanoutExchange();
        }

        public static void FanoutExchange()
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
                channel.ExchangeDeclare(exchange: "fanout_logs",
                                        type: ExchangeType.Fanout);

                channel.ExchangeDeclare(exchange: "team_logs",
                                        type: ExchangeType.Fanout);

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

                channel.QueueDeclare(queue: "technology",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueDeclare(queue: "realmadrid",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);


                channel.QueueDeclare(queue: "barcelona",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);


                channel.QueueBind(queue: "technology",
                                  exchange: "fanout_logs",
                                  routingKey: "");

                channel.QueueBind(queue: "sport",
                                  exchange: "fanout_logs",
                                  routingKey: "");

                channel.QueueBind(queue: "fashion",
                                  exchange: "fanout_logs",
                                  routingKey: "");

                channel.QueueBind(queue: "realmadrid",
                                  exchange: "team_logs",
                                  routingKey: "");

                channel.QueueBind(queue: "barcelona",
                                  exchange: "team_logs",
                                  routingKey: "");

                var messageFashion = JsonConvert.SerializeObject("Article about Fashion sent.");
                var bodyArrFashion = Encoding.UTF8.GetBytes(messageFashion);

                var messageSport = JsonConvert.SerializeObject("Article about Sport sent.");
                var bodyArrSport = Encoding.UTF8.GetBytes(messageSport);

                var messageTechnology = JsonConvert.SerializeObject("Article about Technology sent.");
                var bodyArrTechnology = Encoding.UTF8.GetBytes(messageTechnology);

                var messageBarcelona = JsonConvert.SerializeObject("Article about Barcelona sent.");
                var bodyArrBarcelona = Encoding.UTF8.GetBytes(messageBarcelona);

                var messageRealMadrid = JsonConvert.SerializeObject("Article about RealMadrid sent.");
                var bodyArrRealMadrid = Encoding.UTF8.GetBytes(messageRealMadrid);

                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.Escape)
                        break;

                    //channel.BasicPublish(exchange: "fanout_logs",
                    //routingKey: "",
                    //basicProperties: null,
                    //body: bodyArrFashion);


                    //channel.BasicPublish(exchange: "fanout_logs",
                    //routingKey: "",
                    //basicProperties: null,
                    //body: bodyArrSport);

                    //channel.BasicPublish(exchange: "fanout_logs",
                    //routingKey: "",
                    //basicProperties: null,
                    //body: bodyArrTechnology);


                    //channel.BasicPublish(exchange: "team_logs",
                    //routingKey: "",
                    //basicProperties: null,
                    //body: bodyArrSport);

                    channel.BasicPublish(exchange: "team_logs",
                    routingKey: "",
                    basicProperties: null,
                    body: bodyArrTechnology);



                    Console.WriteLine($"[x] Sent {0}", bodyArrFashion);
                    Console.WriteLine($"[x] Sent {0}", bodyArrSport);
                    Console.WriteLine($"[x] Sent {0}", bodyArrTechnology);
                    Console.WriteLine($"[x] Sent {0}", bodyArrRealMadrid);
                    Console.WriteLine($"[x] Sent {0}", bodyArrBarcelona);

                    Console.WriteLine(" Press [enter] to send a message again.");
                }

            }
        }
    }
}