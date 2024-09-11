using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IBiddingRepository
    {
        Task<Bidding> CreateAsync(Bidding bidding);
        Task<Bidding> UpdateAsync(Bidding auction);
        Task<Bidding> GetAsync(Expression<Func<Bidding, bool>> expression);
    }
}
