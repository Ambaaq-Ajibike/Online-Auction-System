using Application.DTO;
using Application.DTO.Bidding;
using Application.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;
using System.Text.Json;
using System.Text;
using NATS.Client;

namespace Application.Services.Implementations
{
    // BiddingService class implements IBiddingService, handles bidding operations and communication with repositories and NATS.
    public class BiddingService(IBiddingRepository _repository, IConnection _natsConnection) : IBiddingService
    {
        public async Task<BaseResponse<Guid>> CreateAsync(CreateBiddingRequest request)
        {
            var existingBidding = await _repository.GetAsync(x => x.BidderId == request.BidderId && x.AuctionId == request.AuctionId);

            // If the user already placed a bid, return a response indicating failure.
            if (existingBidding is not null)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "User already bid"
                };
            }

            var bidding = new Bidding(request.BidderId, request.Amount, request.AuctionId, request.ProductName, request.ProductDescription);
            await _repository.CreateAsync(bidding);  // Save the new bid to the repository.

            var biddings = await _repository.ListAsync(x => x.AuctionId == request.AuctionId);
            var emails = biddings.Select(x => x.BidderId).ToList();  // Collect bidder IDs (which seem is email format).

            // Find the highest bid in the auction.
            var highestBidding = biddings.MaxBy(x => x.Amount);

            var model = new NotificationModel(emails, highestBidding.Amount, highestBidding.BidderId.Split("@")[0], highestBidding.AuctionId, highestBidding.ProductName);

            // Publish the highest bid notification using NATS.
            await PublishMessage(model);

            return new BaseResponse<Guid>
            {
                Message = "User bid successfully",
                Status = true,
                Data = bidding.Id
            };
        }

        // Publishes a notification message about the highest bid to a NATS channel.
        private async Task PublishMessage(NotificationModel model)
        {
            // Serialize the notification model to JSON format and encode it as a byte array.
            var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model));

            // Publish the serialized data to the "HighestBiddingNotification" channel via NATS.
            _natsConnection.Publish("HighestBiddingNotification", data);
        }

        public async Task<BaseResponse<BiddingDto>> GetAsync(Guid id)
        {
            var bidding = await _repository.GetAsync(x => x.Id == id);

            // If the bidding is not found, return a failure response.
            if (bidding is null)
            {
                return new BaseResponse<BiddingDto>
                {
                    Message = "Bidding doesn't exist",
                    Status = false
                };
            }

            // If the bidding is found, map its details to a BiddingDto and return it in the response.
            return new BaseResponse<BiddingDto>
            {
                Message = "Bidding successfully retrieved",
                Status = true,
                Data = new BiddingDto(bidding.Id, bidding.ProductName, bidding.Amount, bidding.BidderId)
            };
        }

        public async Task<BaseResponse<BiddingDto>> UpdateAsync(string bidderId, UpdateBiddingRequest request)
        {
            var bidding = await _repository.GetAsync(x => x.AuctionId == request.auctionId && x.BidderId == bidderId);

            // If no bidding record is found, return a failure response.
            if (bidding is null)
            {
                return new BaseResponse<BiddingDto>
                {
                    Message = "Bidding doesn't exist",
                    Status = false
                };
            }

            // Update the bid amount and save the updated bid to the repository.
            bidding = bidding.UpdateAmount(request.amount);
            await _repository.UpdateAsync(bidding);

            // Return a success response with the updated bid's details.
            return new BaseResponse<BiddingDto>
            {
                Message = "Bidding successfully updated",
                Status = true,
                Data = new BiddingDto(bidding.Id, bidding.ProductName, bidding.Amount, bidding.BidderId)
            };
        }
    }
}
