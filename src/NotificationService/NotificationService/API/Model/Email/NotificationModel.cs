namespace API.Model.Email
{
    public record NotificationModel(List<string> bidderEmails, decimal highestBiddingAmount, string highestBidderName, Guid auctionId, string product);
}
