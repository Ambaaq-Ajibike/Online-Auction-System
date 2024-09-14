using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Auction
{
    public record UpdateAuctionRequest(string productName, string productDescription, string imageUrl, decimal price, DateTime? openingDate, DateTime? closingDate);
}
