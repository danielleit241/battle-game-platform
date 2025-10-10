using BattleGame.Shared.Common;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BattleGame.MessageBus
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumers(Assembly.GetEntryAssembly());

                config.UsingRabbitMq((context, configurator) =>
                {
                    var rabbitMqConnectionString = configuration.GetConnectionString(Const.RabbitMq)
                        ?? throw new ArgumentNullException("RabbitMq connection string is empty");
                    configurator.Host(new Uri(rabbitMqConnectionString));
                    configurator.ConfigureEndpoints(context);

                    configurator.UseMessageRetry(retry =>
                    {
                        retry.Interval(
                            retryCount: 3,
                            interval: TimeSpan.FromSeconds(5));
                    });
                });
            });
            return services;
        }
    }
}
