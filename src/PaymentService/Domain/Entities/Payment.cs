using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public Guid InvoiceId { get; private set; }
        public DateTime Date { get; private set; }
        public bool IsSuccessFull {  get; private set; }
        public Payment(Guid invoiceId, bool isSuccessFull) 
        {
            Id = Guid.NewGuid();
            InvoiceId = invoiceId;
            Date = DateTime.Now;
            IsSuccessFull = isSuccessFull;
        }

        
    }
}
