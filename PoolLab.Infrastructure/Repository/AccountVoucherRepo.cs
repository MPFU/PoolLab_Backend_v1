using Microsoft.EntityFrameworkCore;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using PoolLab.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Repository
{
    public class AccountVoucherRepo : GenericRepo<AccountVoucher>, IAccountVoucherRepo
    {
        public AccountVoucherRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<AccountVoucher>> GetAllAccountVoucher()
        {
            return await _dbContext.AccountVoucher.Include(x => x.Voucher).Include(x => x.Account).ToListAsync();
        }

        public async Task<IEnumerable<AccountVoucher>> GetAllAccountVoucherByVouOrAccID(Guid Id)
        {
            return await _dbContext.AccountVoucher
                .Where(x => x.VoucherID == Id || x.CustomerID == Id)
                .Where(x => x.IsAvailable == true)
                .Include(x => x.Account)
                .Include(x => x.Voucher)
                .ToListAsync();
        }
    }
}
