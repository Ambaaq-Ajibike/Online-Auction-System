using Application.DTO.Payment;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NATS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            _natsConnection.SubscribeAsync("MakePayment", async (sender, args) =>
            {
                try
                {
                    var requestJson = Encoding.UTF8.GetString(args.Message.Data);
                    var request = JsonSerializer.Deserialize<PaymentRequest>(requestJson);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _invoiceService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
                        await _invoiceService.CreateAsync(request);
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
