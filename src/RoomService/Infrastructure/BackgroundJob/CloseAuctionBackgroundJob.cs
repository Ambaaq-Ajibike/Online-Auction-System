﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.BackgroundJob
{
    using System;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Infrastructure.Persistence;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using NATS.Client;

    public class CloseAuctionBackgroundJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CloseAuctionBackgroundJob> _logger;

        public CloseAuctionBackgroundJob(IServiceProvider serviceProvider, ILogger<CloseAuctionBackgroundJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Auction background service is starting.");

            // Keep the service running until the application stops or it is canceled
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Checking auctions...");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var _natsConnection = scope.ServiceProvider.GetRequiredService<IConnection>();

                        var auctionsToClose = dbContext.Auctions
                            .Where(a => !a.HasEnd && a.ClosingDate <= DateTime.Now)
                            .ToList();

                        for(int i = 0; i < auctionsToClose.Count; i++)
                        {
                            
                            auctionsToClose[i] = auctionsToClose[i].EndAuction();
                            dbContext.Update(auctionsToClose[i]);
                            dbContext.SaveChanges();
                            var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(auctionsToClose[i].Id));

                            _natsConnection.Publish("CloseRoom", data);
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while checking auctions.");
                }

                // Delay the next execution for 1 minute
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }

            _logger.LogInformation("Auction background service is stopping.");
        }
    }

}
