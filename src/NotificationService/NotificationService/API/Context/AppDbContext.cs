using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using API.Model;

namespace API.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {

        }
        public DbSet<HighestBidderDetail> HighestBidders { get; set; }
    }
}
