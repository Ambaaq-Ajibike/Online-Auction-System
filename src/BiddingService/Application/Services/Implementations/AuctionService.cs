using Application.DTO;
using Application.DTO.Auction;
using Application.Repositories;
using Application.Services.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class AuctionService(IAuctionRepository _repository) : IAuctionService
    {
        public async Task<BaseResponse<Guid>> CreateAsync(CreateAuctionRequest auctionRequest)
        {
            if (auctionRequest.openingDate > auctionRequest.closingDate)
            {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "Opening date must be before closing date"
                };
            }
            else if (auctionRequest.openingDate < DateTime.Now) {
                return new BaseResponse<Guid>
                {
                    Status = false,
                    Message = "The provided opening date has passed"
                };
            }
            var auction = new Auction(auctionRequest.productName, auctionRequest.productDescription, auctionRequest.imageUrl, auctionRequest.price, auctionRequest.createdBy, auctionRequest.openingDate, auctionRequest.closingDate);
            await _repository.CreateAsync(auction);
            return new BaseResponse<Guid>
            {
                Status = true,
                Message = "Auction created successfully",
                Data = auction.Id
            };
        }

        public async Task<BaseResponse<List<AuctionDto>>> GetAllOpenAuctionsAsync()
        {
            var openAuctions = await _repository.ListAsync(x => x.OpeningDate > DateTime.Now);
            return new BaseResponse<List<AuctionDto>>
            {
                Status = true,
                Data = openAuctions.Select(x => new AuctionDto(x.Id, x.ProductName, x.ProductDescription, x.ImageUrl, x.Price, x.CreatedBy, x.OpeningDate, x.ClosingDate)).ToList(),
                Message = "Open Auctions"
            };
        }

        public async Task<BaseResponse<AuctionDto>> GetAsync(Guid auctionId)
        {
            var auction = await _repository.GetAsync(a => a.Id == auctionId);
            if(auction is null)
            {
                return new BaseResponse<AuctionDto>
                {
                    Message = "Auction with the Id not found",
                    Status = false,
                };
            }
            return new BaseResponse<AuctionDto>
            {
                Message = "Auction retrieved successfuly",
                Status = true,
                Data = new AuctionDto(auction.Id, auction.ProductName, auction.ProductDescription, auction.ImageUrl, auction.Price, auction.CreatedBy, auction.OpeningDate, auction.ClosingDate)
            };
        }

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
            if(auctionRequest.openingDate.HasValue)
            {
                if (auctionRequest.closingDate.HasValue)
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
                else if (auctionRequest.openingDate < DateTime.Now)
                {
                    return new BaseResponse<AuctionDto>
                    {
                        Status = false,
                        Message = "The provided opening date has passed"
                    };
                }
                else if(auctionRequest.openingDate > auction.ClosingDate)
                    {
                    return new BaseResponse<AuctionDto>
                    {
                        Status = false,
                        Message = "Opening date must be before closing date"
                    };
                }
            }
            
            else if(auctionRequest.closingDate.HasValue && auction.OpeningDate > auctionRequest.closingDate)
            {
                return new BaseResponse<AuctionDto>
                {
                    Status = false,
                    Message = "Opening date must be before closing date"
                };
            }
            
            auction = auction.Update(auctionRequest.productName, auctionRequest.productDescription, auctionRequest.imageUrl, auctionRequest.price, auctionRequest.openingDate, auctionRequest.closingDate);
            await _repository.UpdateAsync(auction);
            return new BaseResponse<AuctionDto>
            {
                Message = "Auction updated successfuly",
                Status = true,
                Data = new AuctionDto(auction.Id, auction.ProductName, auction.ProductDescription, auction.ImageUrl, auction.Price, auction.CreatedBy, auction.OpeningDate, auction.ClosingDate)
            };
        }
    }
}
