using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment invoice);
        Task<Payment> GetAsync(Expression<Func<Payment, bool>> expression);
    }
}
