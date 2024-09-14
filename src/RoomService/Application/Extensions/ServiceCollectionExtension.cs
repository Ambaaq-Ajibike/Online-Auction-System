using Application.Repositories;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IRoomService, RoomService>()
                .AddSingleton<IConnection>(sp =>
                {
                    var options = ConnectionFactory.GetDefaultOptions();
                    options.Url = "nats://localhost:4222";  
                    return new ConnectionFactory().CreateConnection(options);
                }); ;
        }
    }
}
