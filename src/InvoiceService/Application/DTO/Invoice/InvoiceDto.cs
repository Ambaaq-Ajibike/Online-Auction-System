using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Invoice
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }
        public string Name { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
    }
}
