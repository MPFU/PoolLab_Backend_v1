using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IAccountVoucherRepo : IGenericRepo<AccountVoucher>
    {
        Task<IEnumerable<AccountVoucher>> GetAllAccountVoucher();

        Task<IEnumerable<AccountVoucher>> GetAllAccountVoucherByVouOrAccID(Guid Id);
    }
}
