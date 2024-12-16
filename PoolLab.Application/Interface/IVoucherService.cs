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
    public interface IVoucherService
    {
        Task<string?> AddNewVoucher(NewVoucherDTO newVoucherDTO);
        Task<string?> InActiveVoucher(Guid Id);
        Task<string?> ReActiveVoucher(Guid Id);
        Task<VoucherDTO?> GetVoucherById(Guid id);
        Task<PageResult<VoucherDTO>?> GetAllVoucher(VoucherFilter vouFilter);
        Task<string?> UpdateVoucher(Guid Id, NewVoucherDTO newVoucherDTO);
    }
}
