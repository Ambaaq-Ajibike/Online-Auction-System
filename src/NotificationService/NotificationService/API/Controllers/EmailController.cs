using API.Model.Email;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController(IEmailService _emailService) : ControllerBase
    {
        [HttpPost("bidding")]
        public async Task<IActionResult> SendBidUpdates(SendEmailModel<BiddingUpdateEmailModel> biddingUpdate)
        {
            var mailSent = await _emailService.SendBidUpdates(biddingUpdate);
            return Ok(new { Message = "Mail successfully sent" });
        }

    }
}
