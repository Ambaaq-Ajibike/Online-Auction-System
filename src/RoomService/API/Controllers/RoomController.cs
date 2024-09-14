using Application.DTO.Auction;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController(IRoomService _auctionService) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync(CreateAuctionRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _auctionService.CreateAsync(userId, request);
            return (response.Status) ? Ok(response) : BadRequest(response);
        }
        
        [HttpPost("/rooms/{roomId}/enter")]
        [Authorize]
        public async Task<IActionResult> EnterRoomAsync([FromRoute]Guid roomId, BiddingRequest request)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var response = await _auctionService.EnterRoomAsync(userEmail, roomId, request);
            return (response.Status) ? Ok(response) : BadRequest(response);
        }
        
        [HttpPut("{roomId}/amount")]
        [Authorize]
        public async Task<IActionResult> UpdateBiddingAmountAsync([FromRoute] Guid roomId, BiddingRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _auctionService.UpdateBiddingAmountAsync(userId, roomId, request);
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
