
using API.Context;
using API.Model.Email;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NATS.Client;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services.Implementations
{
    public class SubscriberService : IHostedService
    {
        private readonly IConnection _natsConnection;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SubscriberService(IConnection natsConnection, IServiceScopeFactory serviceScopeFactory)
        {
            _natsConnection = natsConnection;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _natsConnection.SubscribeAsync("HighestBiddingNotification", async (sender, args) =>
            {
                try
                {
                    var requestJson = Encoding.UTF8.GetString(args.Message.Data);
                    var request = JsonSerializer.Deserialize<NotificationModel>(requestJson);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                      await _emailService.SendBidUpdates(request);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
            
            _natsConnection.SubscribeAsync("CloseRoom", async (sender, args) =>
            {
                try
                {
                    var requestJson = Encoding.UTF8.GetString(args.Message.Data);
                    var request = JsonSerializer.Deserialize<Guid>(requestJson);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var highestBidder = await _context.HighestBidders.SingleOrDefaultAsync(x => x.AuctionId == request);

                        var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(highestBidder));
                        _natsConnection.Publish("GenerateInvoice", data);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _natsConnection?.Close();
            return Task.CompletedTask;
        }
    }

}
