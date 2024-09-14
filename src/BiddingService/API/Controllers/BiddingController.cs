using Application.DTO.Bidding;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiddingController(IBiddingService _biddingService) : ControllerBase
    {
       

        [HttpPatch("{biddingId}")]
        [Authorize]
        public async Task<IActionResult> UpdateBidAsyn(UpdateBiddingRequest biddingRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _biddingService.UpdateAsync(userId, biddingRequest);
            return (response.Status) ? Ok(response) : NotFound(response);
        }
        
        [HttpGet("{biddingId}")]
        public async Task<IActionResult> GetBiddingAsync(Guid biddingId)
        {
            var response = await _biddingService.GetAsync(biddingId);
            return (response.Status) ? Ok(response) : NotFound(response);
        }
    }
}
