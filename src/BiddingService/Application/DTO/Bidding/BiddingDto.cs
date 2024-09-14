using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Bidding
{
    public class BiddingDto(Guid id, string productName, decimal biddingAmount, string bidderId);
    public record NotificationModel(List<string> bidderEmails, decimal highestBiddingAmount, string highestBidderName, Guid auctionId, string product);
}
