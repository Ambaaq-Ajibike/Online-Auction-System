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

namespace API.Services.Implementations
{
    public class EmailService(IOptions<EmailConfig> config) : IEmailService
    {
        private readonly EmailConfig _emailConfig = config.Value;
        public async Task<bool> SendBidUpdates(SendEmailModel<BiddingUpdateEmailModel> biddingUpdate)
        {
            Configuration.Default.ApiKey.Add("api-key", _emailConfig.ApiKey);

            var apiInstance = new TransactionalEmailsApi();

            SendSmtpEmailSender Email = new SendSmtpEmailSender(_emailConfig.SendName, _emailConfig.SendEmail);
           
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(biddingUpdate.recieverEmail, biddingUpdate.recieverName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            string HtmlContent = GenerateBiddingUpdateMessage(biddingUpdate.data);
            string Subject = "Bidding Update";
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, null, Subject, null, null, null, null, null, null, null);
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

    }
}
