using Microsoft.EntityFrameworkCore;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Interface
{
    public class PaymentRepo : GenericRepo<Transaction>, IPaymentRepo
    {
        public PaymentRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Transaction>?> GetAllCusTransaction()
        {
            return await _dbContext.Payments.Where(x => x.Order == null).Include(x => x.Account).ToListAsync();
        }

        public async Task<IEnumerable<Transaction>?> GetAllOrderTransaction()
        {
            return await _dbContext.Payments.Where(x => x.Order != null).Include(x => x.Account).Include(x => x.Order).ToListAsync();
        }

        public async Task<Transaction?> GetOrderTransaction(Guid id)
        {
            return await _dbContext.Payments.Where(x => x.Id == id).Include(x => x.Account).Include(x => x.Order).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllTransaction()
        {
           return await _dbContext.Payments.Include(x => x.Account).ToListAsync();
        }
    }
}
