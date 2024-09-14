using API.Model.Email;

namespace API.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendBidUpdates(NotificationModel biddingUpdate);
    }
}
