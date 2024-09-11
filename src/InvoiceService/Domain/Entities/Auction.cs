using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Auction
    {
        public Guid Id { get; private set; }
        public string ProductName { get; private set; }
        public string ProductDescription { get; private set; }
        public string ImageUrl { get; private set; }
        public decimal Price { get; private set; }
        public Guid CreatedBy { get; private set; }
        public bool HasEnd {  get; private set; } 
        public DateTime OpeningDate { get; private set; }
        public DateTime ClosingDate { get; private set; }
        public List<Bidding> Biddings { get; set; } = new();

        public Auction(string productName, string productDescription, string imageUrl, decimal price, Guid createdBy, DateTime openingDate, DateTime closingDate )
        {
            Id = Guid.NewGuid();
            ProductName = productName;
            ImageUrl = imageUrl;
            ProductDescription = productDescription;
            Price = price;
            CreatedBy = createdBy;
            ClosingDate = closingDate;
            OpeningDate = openingDate;
        }
        public Auction Update(string? productName, string? productDescription, string imageUrl, decimal? price, DateTime? openingDate, DateTime? closingDate )
        {
            if(DateTime.Now >= OpeningDate)
            {
                throw new InvalidOperationException("Auction cannot be editted after starting!");
            }
            ProductName = productName ?? ProductName;
            ImageUrl = imageUrl ?? ImageUrl;
            ProductDescription = productDescription ?? ProductDescription;
            Price = price ?? Price;
            ClosingDate = closingDate ?? ClosingDate;
            OpeningDate = openingDate ?? ClosingDate;
            return this;
        }
        public Auction EndAuction()
        {
            HasEnd = true;
            return this;
        }


    }
}
