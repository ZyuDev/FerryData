using FerryData.Contract;
using FerryData.WorkerService.Consumers;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace FerryData.WorkerService
{
    public class Program
    {
        public static async Task Main()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var massTransitSection = config.GetSection("MassTransit");
                var url = massTransitSection.GetValue<string>("Url");
                var userName = massTransitSection.GetValue<string>("UserName");
                var password = massTransitSection.GetValue<string>("Password");

                cfg.Host($"rabbitmq://{url}/", configurator =>
                {
                    configurator.Username(userName);
                    configurator.Password(password);
                });

                cfg.ReceiveEndpoint(nameof(IMessageBrokerRasult), e =>
                {
                    e.Consumer<MessageConsumer>();
                });
            });

            Console.WriteLine("ќжидаем сообщени€...");
            await busControl.StartAsync();
        }

    }
}
