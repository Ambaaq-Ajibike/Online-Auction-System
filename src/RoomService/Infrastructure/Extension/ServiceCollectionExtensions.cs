using Application.Repositories;
using Infrastructure.BackgroundJob;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
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
                .AddScoped<IRoomRepository, RoomRepository>().AddHostedService<CloseAuctionBackgroundJob>(); ;
        }
    }
}
