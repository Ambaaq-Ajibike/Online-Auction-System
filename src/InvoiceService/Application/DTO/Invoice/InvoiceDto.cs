using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Invoice
{
    public record InvoiceDto(Guid invoiceId, string payerName, Guid payerId, string receiverName, Guid receiverId, decimal amountToPay);

}
