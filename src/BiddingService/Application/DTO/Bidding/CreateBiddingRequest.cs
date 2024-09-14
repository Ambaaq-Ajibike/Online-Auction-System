using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Bidding
{
    public record CreateBiddingRequest(decimal Amount, Guid AuctionId, string ProductName, string ProductDescription, string BidderId);
}
