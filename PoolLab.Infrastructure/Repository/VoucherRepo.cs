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
    public class VoucherRepo : GenericRepo<Voucher>, IVoucherRepo
    {
        public VoucherRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }
    }
}
