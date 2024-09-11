using Application.DTO;
using Application.DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<BaseResponse<Guid>> CreateAsync(PaymentRequest invoiceRequest);
        Task<BaseResponse<PaymentDto>> GetAsync(Guid invoiceId);
    }
}
