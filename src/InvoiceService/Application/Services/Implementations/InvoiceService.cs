using Application.DTO;
using Application.DTO.Invoice;
using Application.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;
using NATS.Client;
using System.Text;
using System.Text.Json;

namespace Application.Services.Implementations
{
    // InvoiceService class implementing the IInvoiceService interface.
    // It handles invoice-related business logic and communicates with the repository and NATS messaging system.
    public class InvoiceService(IInvoiceRepository _repository, IConnection _natsConnection) : IInvoiceService
    {
        // Asynchronously creates a new invoice and publishes payment data to NATS for further processing.
        public async Task<BaseResponse<Guid>> CreateAsync(InvoiceDto invoiceRequest)
        {
            // Create a new Invoice entity from the DTO passed in the request.
            var invoice = new Invoice(invoiceRequest.Name, invoiceRequest.Amount, invoiceRequest.Product, invoiceRequest.AuctionId);

            // Call repository to store the created invoice in the database.
            await _repository.CreateAsync(invoice);

            // Prepare an anonymous object with invoice details to be sent for payment processing.
            var model = new
            {
                Amount = invoice.AmountToPay,
                invoice.InvoiceRef,
                Email = $"{invoice.PayerName}@gmail.com",
            };

            // Serialize the model to JSON and convert it to a byte array for transmission.
            var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model));

            // Publish the serialized payment information to the "MakePayment" channel via NATS.
            _natsConnection.Publish("MakePayment", data);

            // Return a successful response containing the invoice ID.
            return new BaseResponse<Guid>
            {
                Status = true,
                Message = "Invoice created successfully",
                Data = invoice.Id
            };
        }

        // Asynchronously retrieves an invoice by its ID and returns the corresponding invoice DTO.
        public async Task<BaseResponse<InvoiceDto>> GetAsync(Guid invoiceId)
        {
            // Query the repository for an invoice with the provided ID.
            var invoice = await _repository.GetAsync(a => a.Id == invoiceId);

            // If the invoice is not found, return a failure response.
            if (invoice is null)
            {
                return new BaseResponse<InvoiceDto>
                {
                    Message = "Invoice with the Id not found",
                    Status = false,
                };
            }

            // If the invoice is found, map its details to an InvoiceDto and return it in the response.
            return new BaseResponse<InvoiceDto>
            {
                Message = "Invoice retrieved successfully",
                Status = true,
                Data = new InvoiceDto
                {
                    Product = invoice.ProductPurchased,
                    Amount = invoice.AmountToPay,
                    Name = invoice.PayerName,
                }
            };
        }
    }
}
