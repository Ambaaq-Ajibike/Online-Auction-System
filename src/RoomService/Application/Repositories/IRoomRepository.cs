using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IRoomRepository
    {
        Task<Auction> CreateAsync(Auction auction);
        Task<Auction> UpdateAsync(Auction auction);
        Task<Auction> GetAsync(Expression<Func<Auction, bool>> expression);
        Task<IList<Auction>> ListAsync(Expression<Func<Auction, bool>> expression);
    }
}
