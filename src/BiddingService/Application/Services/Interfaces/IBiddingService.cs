using Application.DTO;
using Application.DTO.Bidding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IBiddingService
    {
        /// <summary>
        /// Creates a new bid for a user in an auction asynchronously.
        /// Ensures the user hasn't already placed a bid for the same auction.
        /// If successful, sends a notification to all bidders with the current highest bid.
        /// </summary>
        /// <param name="request">The details of the new bid, including user, auction, and product info.</param>
        /// <returns>A response containing the status of the operation and the newly created bidding ID.</returns>
        Task<BaseResponse<Guid>> CreateAsync(CreateBiddingRequest request);

        /// <summary>
        /// Updates the bidding amount for a user in an auction asynchronously.
        /// </summary>
        /// <param name="bidderId">The ID of the bidder updating their bid.</param>
        /// <param name="request">The updated bidding details, including the new bid amount and auction ID.</param>
        /// <returns>A response indicating whether the update was successful or not, along with the updated bid details.</returns>
        Task<BaseResponse<BiddingDto>> UpdateAsync(string bidderId, UpdateBiddingRequest request);


        /// <summary>
        /// Retrieves a specific bidding entry asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the bid to retrieve.</param>
        /// <returns>A response containing the bidding details if found, or an error message if not.</returns>
        Task<BaseResponse<BiddingDto>> GetAsync(Guid id);

    }
}
