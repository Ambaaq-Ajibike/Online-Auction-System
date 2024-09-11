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
        public Guid InvoiceId { get; private set; }
        public string PayerName { get; private set; }
        public Guid PayerId { get; private set; }
        public string ReceiverName { get; private set; }
        public Guid ReceiverId { get; private set; }
        public decimal AmountToPay { get; private set; }
        public DateTime DateInitialized { get; private set; }
        public Invoice(Guid invoiceId, string payerName, Guid payerId, string receiverName, Guid receiverId, decimal amountToPay) 
        {
            Id = Guid.NewGuid();
            InvoiceId = invoiceId;
            PayerName = payerName;
            PayerId = payerId;
            ReceiverName = receiverName;
            ReceiverId = receiverId;
            AmountToPay = amountToPay;
            DateInitialized = DateTime.Now;
        }

        
    }
}
