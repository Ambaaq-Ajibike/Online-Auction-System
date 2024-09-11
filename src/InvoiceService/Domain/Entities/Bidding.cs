using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Bidding
    {
        public Guid Id { get; set; }
        public Guid BidderId { get; set; }
        public decimal Amount { get; set; }
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }
        public Bidding(Guid bidderId, decimal amount, Guid auctionId) 
        {
            Id = Guid.NewGuid();
            BidderId = bidderId;
            Amount = amount;
            AuctionId = auctionId;
        }
        public Bidding UpdateAmount(decimal amount)
        {
            Amount = amount;
            return this;
        }
    }
}
