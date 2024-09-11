using Application.DTO;
using Application.DTO.Invoice;
using Application.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class InvoiceService(IInvoiceRepository _repository) : IInvoiceService
    {
        public async Task<BaseResponse<Guid>> CreateAsync(InvoiceDto invoiceRequest)
        {
            var existingInvoice = await _repository.GetAsync(x => x.InvoiceId == invoiceRequest.invoiceId);
            if (existingInvoice is not null) 
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Invoice already created",
                };
            }
           
            var invoice = new Invoice(invoiceRequest.invoiceId, invoiceRequest.payerName, invoiceRequest.payerId, invoiceRequest.receiverName, invoiceRequest.receiverId, invoiceRequest.amountToPay);
            await _repository.CreateAsync(invoice);
            return new BaseResponse<Guid>
            {
                Status = true,
                Message = "Invoice created successfully",
                Data = invoice.Id
            };
        }
        public async Task<BaseResponse<InvoiceDto>> GetAsync(Guid invoiceId)
        {
            var invoice = await _repository.GetAsync(a => a.Id == invoiceId);
            if(invoice is null)
            {
                return new BaseResponse<InvoiceDto>
                {
                    Message = "Invoice with the Id not found",
                    Status = false,
                };
            }
            return new BaseResponse<InvoiceDto>
            {
                Message = "Invoice retrieved successfuly",
                Status = true,
                Data = new InvoiceDto(invoice.InvoiceId, invoice.PayerName, invoice.PayerId, invoice.ReceiverName, invoice.ReceiverId, invoice.AmountToPay)
            };
        }
    }
}
