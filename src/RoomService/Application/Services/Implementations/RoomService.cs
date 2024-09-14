using Application.DTO;
using Application.DTO.Auction;
using Application.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;
using NATS.Client;
using System.Text;
using System.Text.Json;

namespace Application.Services.Implementations
{
    /// <summary>
    /// Service implementation for managing auction rooms and auction-related operations.
    /// </summary>
    public class RoomService(IRoomRepository _repository, IConnection _natsConnection) : IRoomService
    {
        /// <summary>
        /// Creates a new auction asynchronously.
        /// Validates the auction's opening and closing dates before creation.
        /// </summary>
        /// <param name="userId">The ID of the user creating the auction.</param>
        /// <param name="auctionRequest">The auction details provided by the user.</param>
        /// <returns>A response indicating the success or failure of the auction creation.</returns>
        public async Task<BaseResponse<Guid>> CreateAsync(string userId, CreateAuctionRequest auctionRequest)
        {
            // Validation: opening date must be before closing date
            if (auctionRequest.openingDate > auctionRequest.closingDate)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Opening date must be before closing date"
                };
            }
            // Validation: opening date must not be in the past
            else if (auctionRequest.openingDate < DateTime.Now)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "The provided opening date has passed"
                };
            }

            // Create a new auction and persist it
            var auction = new Auction(auctionRequest.productName, auctionRequest.productDescription, auctionRequest.imageUrl, auctionRequest.price, userId, auctionRequest.openingDate, auctionRequest.closingDate);
            await _repository.CreateAsync(auction);

            return new BaseResponse<Guid>
            {
                Status = true,
                Message = "Auction created successfully",
                Data = auction.Id
            };
        }

        /// <summary>
        /// Retrieves a list of all currently open auctions asynchronously.
        /// </summary>
        /// <returns>A response with a list of auction DTOs for all open auctions.</returns>
        public async Task<BaseResponse<List<AuctionDto>>> GetAllOpenAuctionsAsync()
        {
            // Get auctions that are currently open
            var openAuctions = await _repository.ListAsync(x => x.OpeningDate <= DateTime.Now && x.ClosingDate > DateTime.Now);

            return new BaseResponse<List<AuctionDto>>
            {
                Status = true,
                Data = openAuctions.Select(x => new AuctionDto(x.Id, x.ProductName, x.ProductDescription, x.ImageUrl, x.Price, x.CreatedBy, x.OpeningDate, x.ClosingDate)).ToList(),
                Message = "Open Auctions"
            };
        }

        /// <summary>
        /// Retrieves details of a specific auction by its ID.
        /// </summary>
        /// <param name="auctionId">The ID of the auction to retrieve.</param>
        /// <returns>A response containing auction details if found, or an error message if not found.</returns>
        public async Task<BaseResponse<AuctionDto>> GetAsync(Guid auctionId)
        {
            var auction = await _repository.GetAsync(a => a.Id == auctionId);

            if (auction is null)
            {
                return new BaseResponse<AuctionDto>
                {
                    Message = "Auction with the Id not found",
                    Status = false,
                };
            }

            return new BaseResponse<AuctionDto>
            {
                Message = "Auction retrieved successfully",
                Status = true,
                Data = new AuctionDto(auction.Id, auction.ProductName, auction.ProductDescription, auction.ImageUrl, auction.Price, auction.CreatedBy, auction.OpeningDate, auction.ClosingDate)
            };
        }

        /// <summary>
        /// Updates an existing auction's details asynchronously.
        /// </summary>
        /// <param name="auctionId">The ID of the auction to update.</param>
        /// <param name="auctionRequest">The updated auction details.</param>
        /// <returns>A response indicating the success or failure of the update.</returns>
        public async Task<BaseResponse<AuctionDto>> UpdateAsync(Guid auctionId, UpdateAuctionRequest auctionRequest)
        {
            var auction = await _repository.GetAsync(a => a.Id == auctionId);

            if (auction is null)
            {
                return new BaseResponse<AuctionDto>
                {
                    Message = "Auction with the Id not found",
                    Status = false,
                };
            }

            // Validate the opening and closing dates
            if (auctionRequest.openingDate.HasValue && auctionRequest.closingDate.HasValue)
            {
                if (auctionRequest.openingDate > auctionRequest.closingDate)
                {
                    return new BaseResponse<AuctionDto>
                    {
                        Status = false,
                        Message = "Opening date must be before closing date"
                    };
                }
            }

            // Update auction details and persist them
            auction = auction.Update(auctionRequest.productName, auctionRequest.productDescription, auctionRequest.imageUrl, auctionRequest.price, auctionRequest.openingDate, auctionRequest.closingDate);
            await _repository.UpdateAsync(auction);

            return new BaseResponse<AuctionDto>
            {
                Message = "Auction updated successfully",
                Status = true,
                Data = new AuctionDto(auction.Id, auction.ProductName, auction.ProductDescription, auction.ImageUrl, auction.Price, auction.CreatedBy, auction.OpeningDate, auction.ClosingDate)
            };
        }

        /// <summary>
        /// Enters a user into an auction room for bidding.
        /// Publishes a bidding request to NATS.
        /// </summary>
        /// <param name="email">The email of the bidder.</param>
        /// <param name="auctionId">The ID of the auction to bid on.</param>
        /// <param name="request">The bidding request details.</param>
        /// <returns>A response indicating the success or failure of the bidding request.</returns>
        public async Task<BaseResponse<Guid>> EnterRoomAsync(string email, Guid auctionId, BiddingRequest request)
        {
            var auction = await _repository.GetAsync(x => x.Id == auctionId);

            // Various validations for auction and bid amounts
            if (auction is null)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Invalid auction Id"
                };
            }
            else if (auction.OpeningDate > DateTime.Now)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Auction is not open for bidding"
                };
            }
            else if (auction.ClosingDate < DateTime.Now)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Auction is closed for bidding"
                };
            }
            else if (auction.Price > request.amount)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Your bidding amount must be greater than auction price"
                };
            }

            // Serialize and publish bidding request to NATS
            var bidding = new
            {
                Amount = request.amount,
                ProductName = auction.ProductName,
                ProductDescription = auction.ProductDescription,
                AuctionId = auction.Id,
                BidderId = email
            };
            var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(bidding));
            _natsConnection.Publish("EnterBiddingRoom", data);

            return new BaseResponse<Guid>
            {
                Status = true,
                Message = "Bidding request published successfully",
                Data = auction.Id
            };
        }

        /// <summary>
        /// Updates the bidding amount for a user in an auction asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user updating their bid.</param>
        /// <param name="auctionId">The ID of the auction being bid on.</param>
        /// <param name="request">The updated bidding details.</param>
        /// <returns>A response indicating the success or failure of the bid update.</returns>
        public async Task<BaseResponse<Guid>> UpdateBiddingAmountAsync(string userId, Guid auctionId, BiddingRequest request)
        {
            var auction = await _repository.GetAsync(x => x.Id == auctionId);

            // Validate the bidding amount
            if (auction.Price > request.amount)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Your bidding amount must be greater than auction price"
                };
            }

            return new BaseResponse<Guid>
            {
                Status = true,
                Message = "Bidding amount updated successfully"
            };
        }
    }
}
