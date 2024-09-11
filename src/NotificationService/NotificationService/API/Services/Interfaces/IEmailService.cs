﻿using API.Model.Email;

namespace API.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendBidUpdates(SendEmailModel<BiddingUpdateEmailModel> biddingUpdate);
    }
}
