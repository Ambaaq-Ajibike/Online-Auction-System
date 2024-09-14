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
        [HttpPost]
        public async Task<IActionResult> Model()
        {
            return Ok(new { Message = "Mail successfully sent" });
        }

    }
}
