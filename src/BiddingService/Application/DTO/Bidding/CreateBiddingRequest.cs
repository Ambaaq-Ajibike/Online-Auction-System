using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Bidding
{
    public record CreateBiddingRequest(Guid auctionId, Guid bidderId, decimal amount);
}
