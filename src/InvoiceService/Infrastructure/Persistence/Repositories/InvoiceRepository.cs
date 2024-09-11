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
    public class InvoiceRepository(AppDbContext _context) : IInvoiceRepository
    {
        public async Task<Invoice> CreateAsync(Invoice invoice)
        {
           await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice> GetAsync(Expression<Func<Invoice, bool>> expression)
        {
            return await _context.Invoices.FirstOrDefaultAsync(expression);
        }

    }
}
