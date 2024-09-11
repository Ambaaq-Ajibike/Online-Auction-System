using Application.DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PaymentProcessing
{
    public interface IPaystackService
    {
        Task<PaymentResponseModel> InitializePayment(TransactionRequest model);
        Task<VerifyPaymentResponseModel> VerifyPayment(string PaymentReference);
    }
}
