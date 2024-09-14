using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Bidding
    {
        public Guid Id { get; private set; }
        public string BidderId { get; private set; }
        public decimal Amount { get; private set; }
        public Guid AuctionId { get; private set; }
        public string ProductName { get; private set; }
        public string ProductDescription { get; private set; }
        public Bidding(string bidderId, decimal amount, Guid auctionId, string productName, string productDescription) 
        {
            Id = Guid.NewGuid();
            BidderId = bidderId;
            Amount = amount;
            AuctionId = auctionId;
            ProductName = productName;
            ProductDescription = productDescription;
        }
        public Bidding UpdateAmount(decimal amount)
        {
            Amount = amount;
            return this;
        }
    }
}
