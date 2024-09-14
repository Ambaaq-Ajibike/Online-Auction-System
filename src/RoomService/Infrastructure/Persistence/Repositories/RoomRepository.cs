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
    public class RoomRepository(AppDbContext _context) : IRoomRepository
    {
        public async Task<Auction> CreateAsync(Auction auction)
        {
           await _context.Auctions.AddAsync(auction);
            await _context.SaveChangesAsync();
            return auction;
        }

        public async Task<Auction> GetAsync(Expression<Func<Auction, bool>> expression)
        {
            return await _context.Auctions.FirstOrDefaultAsync(expression);
        }

        public async Task<IList<Auction>> ListAsync(Expression<Func<Auction, bool>> expression)
        {
            return await _context.Auctions.Where(expression).ToListAsync();
        }

        public async Task<Auction> UpdateAsync(Auction auction)
        {
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();
            return auction;
        }
    }
}
