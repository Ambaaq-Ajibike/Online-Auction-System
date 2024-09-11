using Application.PaymentProcessing;
using Application.Repositories;
using Application.Util;
using Infrastructure.PaymentProcessing;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AppConnectionString");
            return serviceCollection
                .AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));
        }
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IPaymentRepository, PaymentRepository>()
                .AddScoped<IPaystackService, PayStackService>()
                .AddScoped<ISerializerService, SerializerService>();
        }
    }
}
