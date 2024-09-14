using Application.Repositories;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
namespace Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IInvoiceService, InvoiceService>()
                 .AddSingleton<IConnection>(sp =>
                 {
                     var options = ConnectionFactory.GetDefaultOptions();
                     options.Url = "nats://localhost:4222";
                     return new ConnectionFactory().CreateConnection(options);
                 }).AddHostedService<SubscriberService>();
        }
    }
}
