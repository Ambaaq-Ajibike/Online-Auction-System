using Application.DTO;
using Application.DTO.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<BaseResponse<Guid>> CreateAsync(InvoiceDto invoiceRequest);
        Task<BaseResponse<InvoiceDto>> GetAsync(Guid invoiceId);
    }
}
