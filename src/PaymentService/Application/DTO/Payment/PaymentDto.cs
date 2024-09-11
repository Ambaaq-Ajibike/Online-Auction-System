using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Payment
{
    public record PaymentDto(Guid invoiceId, DateTime datePayed);
}
