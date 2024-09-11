using Application.DTO.Payment;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentService _invoiceService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAsync(PaymentRequest request)
        {
            var response = await _invoiceService.CreateAsync(request);
            return (response.Status) ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{invoiceId}")]
        public async Task<IActionResult> GetAsync(Guid invoiceId)
        {
            var response = await _invoiceService.GetAsync(invoiceId);
            return (response.Status) ? Ok(response) : NotFound(response);
        }
    }
}
