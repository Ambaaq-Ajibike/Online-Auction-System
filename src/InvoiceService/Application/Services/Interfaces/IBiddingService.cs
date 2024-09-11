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
        Task<BaseResponse<Guid>> CreateAsync(CreateBiddingRequest request);
        Task<BaseResponse<BiddingDto>> UpdateAsync(Guid biddingId, decimal amount);
        Task<BaseResponse<BiddingDto>> GetAsync(Guid id);
    }
}
