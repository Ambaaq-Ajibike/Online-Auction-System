using Application.DTO.Bidding;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiddingController(IBiddingService _biddingService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateBiddingRequest request)
        {
            var response = await _biddingService.CreateAsync(request);
            return (response.Status) ? Ok(response) : BadRequest(response);
        }

        [HttpPatch("{biddingId}")]
        public async Task<IActionResult> UpdateBidAsyn([FromRoute]Guid biddingId, UpdateBiddingRequest biddingRequest)
        {
            var response = await _biddingService.UpdateAsync(biddingId, biddingRequest.amount);
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
