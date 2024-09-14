using API.Model.Email;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using API.Context;
using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Implementations
{
    public class EmailService(IOptions<EmailConfig> config, AppDbContext context) : IEmailService
    {
        private readonly EmailConfig _emailConfig = config.Value;
        public async System.Threading.Tasks.Task SendBidUpdates(NotificationModel biddingUpdate)
        {
            var initialHighestBidder = await context.HighestBidders.SingleOrDefaultAsync(x => x.AuctionId == biddingUpdate.auctionId);
            if (initialHighestBidder == null) 
            {
                var highestBidder = new HighestBidderDetail
                {
                    Amount = biddingUpdate.highestBiddingAmount,
                    AuctionId = biddingUpdate.auctionId,
                    Name = biddingUpdate.highestBidderName,
                    Product = biddingUpdate.product

                };
                await context.HighestBidders.AddAsync(highestBidder);
                await context.SaveChangesAsync();
            }
            else
            {
                initialHighestBidder.Name = biddingUpdate.highestBidderName;
                initialHighestBidder.Amount = biddingUpdate.highestBiddingAmount;
                context.Update(initialHighestBidder);
                await context.SaveChangesAsync();
            }

            BiddingUpdateEmailModel model = new(biddingUpdate.highestBidderName, biddingUpdate.highestBiddingAmount);
            await SendEmail(biddingUpdate.bidderEmails, "Bidding Updates", GenerateBiddingUpdateMessage(model));
        }
        private string GenerateBiddingUpdateMessage(BiddingUpdateEmailModel model)
        {
            string htmlTemplate = @"
    <html>
        <body>
            <h1>Bidding Update</h1>
            <p>Dear Bidder,</p>
            <p>The current highest bid is: ${{amount}} by {{highestBidder}}.</p>
            <p>If you'd like to place a higher bid, please visit the auction page.</p>
            <p>Thank you for your participation!</p>
        </body>
    </html>";

            string emailContent = htmlTemplate
                .Replace("{{highestBidder}}", model.highestBidder)
                .Replace("{{amount}}", model.amount.ToString());

            return emailContent;
        }
        public async Task<bool> SendEmail(List<string> receiversEmails, string subject, string htmlContent)
        {
            Configuration.Default.ApiKey.Add("api-key", _emailConfig.ApiKey);

            var apiInstance = new TransactionalEmailsApi();

            SendSmtpEmailSender Email = new SendSmtpEmailSender(_emailConfig.SendName, _emailConfig.SendEmail);


            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            foreach (var email in receiversEmails)
            {
                To.Add(new SendSmtpEmailTo(email, "Recipient"));
            }
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, htmlContent, null, subject, null, null, null, null, null, null, null);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine(e.Message);
                throw;
            }
        }

    }
}
