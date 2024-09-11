using Application.DTO;
using Application.DTO.Bidding;
using Application.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;

namespace Application.Services.Implementations
{
    public class BiddingService(IBiddingRepository _repository, IAuctionRepository _auctionRepository) : IBiddingService
    {
        public async Task<BaseResponse<Guid>> CreateAsync(CreateBiddingRequest request)
        {
            var existingBidding = await _repository.GetAsync(x => x.BidderId == request.bidderId && x.AuctionId == request.auctionId);
            if (existingBidding is not null)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "User already bid"
                };

            }
            var auction = await _auctionRepository.GetAsync(x => x.Id == request.auctionId);
            if (auction is null) {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Invalid auction Id"
                };
            }
            else if(auction.OpeningDate > DateTime.Now)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Auction is not open for bidding"
                };
            }
            else if(auction.ClosingDate < DateTime.Now)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Auction is closed for bidding"
                };
            }
            else if(auction.Price > request.amount)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Your bidding amount must be greater than auction price"
                };
            }
            var bidding = new Bidding(request.bidderId, request.amount, request.auctionId);
            await _repository.CreateAsync(bidding);
            return new BaseResponse<Guid>
            {
                Message = "User bid successfully",
                Status = true,
                Data = bidding.Id
            };
        }

        public async Task<BaseResponse<BiddingDto>> GetAsync(Guid id)
        {
            var bidding = await _repository.GetAsync(x => x.Id == id);
            if (bidding is null)
            {
                return new BaseResponse<BiddingDto>
                {
                    Message = "Bidding doesn't exist",
                    Status = false
                };
            }
            return new BaseResponse<BiddingDto>
            {
                Message = "Bidding succesfully retrieve",
                Status = true,
                Data = new BiddingDto(bidding.Id, bidding.Auction.ProductName, bidding.Auction.Price, bidding.Amount, bidding.BidderId)
            };
        }

        public async Task<BaseResponse<BiddingDto>> UpdateAsync(Guid biddingId, decimal amount)
        {
            var bidding = await _repository.GetAsync(x => x.Id == biddingId);
            if (bidding is null)
            {
                return new BaseResponse<BiddingDto>
                {
                    Message = "Bidding doesn't exist",
                    Status = false
                };
            }
            else if (bidding.Auction.Price > amount)
            {
                return new BaseResponse<BiddingDto>
                {
                    Status = false,
                    Message = "Your bidding amount must be greater than auction price"
                };
            }
            bidding = bidding.UpdateAmount(amount);
            await _repository.UpdateAsync(bidding);
            return new BaseResponse<BiddingDto>
            {
                Message = "Bidding succesfully updated",
                Status = true,
                Data = new BiddingDto(bidding.Id, bidding.Auction.ProductName, bidding.Auction.Price, bidding.Amount, bidding.BidderId)
            };
        }
    }
}
