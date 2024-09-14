using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Invoice
    {
        public Guid Id { get; private set; }
        public Guid AuctionId { get; set; }
        public string InvoiceRef { get; private set; }
        public string PayerName { get; private set; }
        public string ProductPurchased { get; private set; }
        public decimal AmountToPay { get; private set; }
        public DateTime DateInitialized { get; private set; }
        public Invoice(string payerName, decimal amountToPay, string product, Guid auctionId) 
        {
            Id = Guid.NewGuid();
            AuctionId = auctionId;
            InvoiceRef = $"OAS-{Guid.NewGuid().ToString()[..7]}";
            PayerName = payerName;
            ProductPurchased = product;
            AmountToPay = amountToPay;
            DateInitialized = DateTime.Now;
        }

        
    }
}
