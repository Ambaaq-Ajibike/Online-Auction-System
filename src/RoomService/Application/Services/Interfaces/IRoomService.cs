using Application.DTO;
using Application.DTO.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IRoomService
    {
        /// <summary>
        /// Creates a new auction asynchronously.
        /// Validates the auction's opening and closing dates before creation.
        /// </summary>
        /// <param name="userId">The ID of the user creating the auction.</param>
        /// <param name="auctionRequest">The auction details provided by the user.</param>
        /// <returns>A response indicating the success or failure of the auction creation.</returns>
        Task<BaseResponse<Guid>> CreateAsync(string userId, CreateAuctionRequest auctionRequest);


        Task<BaseResponse<AuctionDto>> UpdateAsync(Guid auctionId, UpdateAuctionRequest auctionRequest);

        /// <summary>
        /// Retrieves details of a specific auction by its ID.
        /// </summary>
        /// <param name="auctionId">The ID of the auction to retrieve.</param>
        /// <returns>A response containing auction details if found, or an error message if not found.</returns>
        Task<BaseResponse<AuctionDto>> GetAsync(Guid auctionId);

        /// <summary>
        /// Retrieves a list of all currently open auctions asynchronously.
        /// </summary>
        /// <returns>A response with a list of auction DTOs for all open auctions.</returns>
        Task<BaseResponse<List<AuctionDto>>> GetAllOpenAuctionsAsync();

        /// <summary>
        /// Enters a user into an auction room for bidding.
        /// Publishes a bidding request to NATS.
        /// </summary>
        /// <param name="email">The email of the bidder.</param>
        /// <param name="auctionId">The ID of the auction to bid on.</param>
        /// <param name="request">The bidding request details.</param>
        /// <returns>A response indicating the success or failure of the bidding request.</returns>
        Task<BaseResponse<Guid>> EnterRoomAsync(string email, Guid auctionId, BiddingRequest request);

        /// <summary>
        /// Updates an existing auction's details asynchronously.
        /// </summary>
        /// <param name="auctionId">The ID of the auction to update.</param>
        /// <param name="auctionRequest">The updated auction details.</param>
        /// <returns>A response indicating the success or failure of the update.</returns>
        Task<BaseResponse<Guid>> UpdateBiddingAmountAsync(string userId, Guid auctionId, BiddingRequest request);
    }
}
