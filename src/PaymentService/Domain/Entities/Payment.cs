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
        public string InvoiceRef { get; private set; }
        public DateTime Date { get; private set; }
        public bool IsSuccessFull {  get; private set; }
        public Payment(string invoiceRef, bool isSuccessFull) 
        {
            Id = Guid.NewGuid();
            InvoiceRef = invoiceRef;
            Date = DateTime.Now;
            IsSuccessFull = isSuccessFull;
        }

        
    }
}
