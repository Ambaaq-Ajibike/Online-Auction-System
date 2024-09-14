﻿using Application.DTO;
using Application.DTO.Bidding;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NATS.Client;
using System.Text;
using System.Text.Json;

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
            //Receive event to start bidding, published from room service
            _natsConnection.SubscribeAsync("EnterBiddingRoom", async (sender, args) =>
            {
                try
                {
                    var requestJson = Encoding.UTF8.GetString(args.Message.Data);
                    var request = JsonSerializer.Deserialize<CreateBiddingRequest>(requestJson);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _biddingService = scope.ServiceProvider.GetRequiredService<IBiddingService>();
                        //Creating bidding from data sent by room service
                        var result = await _biddingService.CreateAsync(request);
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
