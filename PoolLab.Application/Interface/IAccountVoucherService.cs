using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IAccountVoucherService
    {
        Task<string?> AddNewAccountVoucher(AddNewAccountVoucherDTO voucherDTO);

        Task<PageResult<GetAllAccountVoucherDTO>> GetAllAccountVoucher(AccountVoucherFilter accountVoucherFilter);

        Task<IEnumerable<GetAllAccountVoucherDTO>> GetAllAccountVoucherByVouOrCusID(Guid Id);

        Task<string?> UseAccountVoucher(Guid id);
    }
}
