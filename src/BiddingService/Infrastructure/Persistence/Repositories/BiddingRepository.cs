using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class BiddingRepository(AppDbContext _context) : IBiddingRepository
    {
        public async Task<Bidding> CreateAsync(Bidding bidding)
        {
            await _context.Biddings.AddAsync(bidding);
            await _context.SaveChangesAsync();
            return bidding;
        }

        public async Task<Bidding> GetAsync(Expression<Func<Bidding, bool>> expression)
        {
            return await _context.Biddings.FirstOrDefaultAsync(expression);
        }
        public async Task<List<Bidding>> ListAsync(Expression<Func<Bidding, bool>> expression)
        {
            return await _context.Biddings.Where(expression).ToListAsync();
        }

        public async Task<Bidding> UpdateAsync(Bidding bidding)
        {
            _context.Biddings.Update(bidding);
            await _context.SaveChangesAsync();
            return bidding;
        }
    }
}
