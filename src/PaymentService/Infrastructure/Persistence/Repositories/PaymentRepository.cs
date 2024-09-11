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
    public class PaymentRepository(AppDbContext _context) : IPaymentRepository
    {
        public async Task<Payment> CreateAsync(Payment invoice)
        {
           await _context.Payments.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Payment> GetAsync(Expression<Func<Payment, bool>> expression)
        {
            return await _context.Payments.FirstOrDefaultAsync(expression);
        }

    }
}
