using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Auction
{
    public record CreateAuctionRequest(string productName, string productDescription, string imageUrl, decimal price, Guid createdBy, DateTime openingDate, DateTime closingDate);

}
