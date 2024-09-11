using Application.DTO;
using Application.DTO.Auction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAuctionService
    {
        Task<BaseResponse<Guid>> CreateAsync(CreateAuctionRequest auctionRequest);
        Task<BaseResponse<AuctionDto>> UpdateAsync(Guid auctionId, UpdateAuctionRequest auctionRequest);
        Task<BaseResponse<AuctionDto>> GetAsync(Guid auctionId);
        Task<BaseResponse<List<AuctionDto>>> GetAllOpenAuctionsAsync();
    }
}
