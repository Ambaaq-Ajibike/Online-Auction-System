using Application.DTO.Auction;
using Application.DTO.Bidding;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController(IAuctionService _auctionService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateAuctionRequest request)
        {
            var response = await _auctionService.CreateAsync(request);
            return (response.Status) ? Ok(response) : BadRequest(response);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetOpenAuctions()
        {
            var response = await _auctionService.GetAllOpenAuctionsAsync();
            return Ok(response);
        }

        [HttpPut("{auctionId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid auctionId, UpdateAuctionRequest auctionRequest)
        {
            var response = await _auctionService.UpdateAsync(auctionId, auctionRequest);
            return (response.Status) ? Ok(response) : NotFound(response);
        }

        [HttpGet("{auctionId}")]
        public async Task<IActionResult> GetAsync(Guid auctionId)
        {
            var response = await _auctionService.GetAsync(auctionId);
            return (response.Status) ? Ok(response) : NotFound(response);
        }
    }
}
