using Application.DTO;
using Application.DTO.Payment;
using Application.PaymentProcessing;
using Application.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;

namespace Application.Services.Implementations
{
    public class PaymentService(IPaymentRepository _repository, IPaystackService _paymentService) : IPaymentService
    {
        public async Task<BaseResponse<Guid>> CreateAsync(PaymentRequest paymentRequest)
        {
            var existingPayment = await _repository.GetAsync(x => x.InvoiceRef == paymentRequest.invoiceRef);
            if (existingPayment is not null) 
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Payment already created",
                };
            }
            
            
           
            var invoice = new Payment(paymentRequest.invoiceRef, true);
            await _repository.CreateAsync(invoice);
            return new BaseResponse<Guid>
            {
                Status = true,
                Message = "Payment created successfully",
                Data = invoice.Id
            };
        }
        public async Task<BaseResponse<PaymentDto>> GetAsync(Guid invoiceId)
        {
            var invoice = await _repository.GetAsync(a => a.Id == invoiceId);
            if(invoice is null)
            {
                return new BaseResponse<PaymentDto>
                {
                    Message = "Payment with the Id not found",
                    Status = false,
                };
            }
            return new BaseResponse<PaymentDto>
            {
                Message = "Payment retrieved successfuly",
                Status = true,
                Data = new PaymentDto(invoice.InvoiceRef, invoice.Date)
            };
        }
    }
}
