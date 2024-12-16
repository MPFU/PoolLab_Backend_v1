using AutoMapper;
using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Services
{
    public class AccountVoucherService : IAccountVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public AccountVoucherService(IMapper mapper, IUnitOfWork unitOfWork, IAccountService accountService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountService = accountService;

        }

        public async Task<string?> AddNewAccountVoucher(AddNewAccountVoucherDTO voucherDTO)
        {
            try
            {
                var accVou = _mapper.Map<AccountVoucher>(voucherDTO);

                //Kiểm tra tài khoản
                var acc = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)voucherDTO.CustomerID);
                if (acc == null)
                {
                    return "Không tìm thấy tài khoản này!";
                }
                if (acc.Status != "Kích Hoạt")
                {
                    return "Tài khoản của bạn không được kích hoạt để thực hiện chức năng này!";
                }

                //Kiểm tra voucher
                var voucher = await _unitOfWork.VoucherRepo.GetByIdAsync((Guid)voucherDTO.VoucherID);
                if (voucher == null)
                {
                    return "Không tìm thấy khuyến mãi này!";
                }
                if(voucher.Status != "Kích Hoạt")
                {
                    return "Khuyến mãi này không còn khả dụng!";
                }

                await _unitOfWork.BeginTransactionAsync();

                if(acc.Point < voucher.Point)
                {
                    return "Tài khoản của bạn không đủ để có được khuyến mãi này!";
                }

                var newPoint = acc.Point - voucher.Point;

                var upAcc = await _accountService.UpdateAccPoint(acc.Id, (int)newPoint);

                accVou.Id = Guid.NewGuid();
                accVou.Discount = voucher.Discount;
                accVou.IsAvailable = true;
                await _unitOfWork.AccountVoucherRepo.AddAsync(accVou);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Thất Bại!";
                }
                await _unitOfWork.CommitTransactionAsync();
                return null;
            }catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<GetAllAccountVoucherDTO>?> GetAllAccountVoucherByVouOrCusID(Guid Id)
        {
            var voulist = await _unitOfWork.AccountVoucherRepo.GetAllAccountVoucherByVouOrAccID(Id);
            if (voulist != null)
            {
                voulist = voulist.OrderByDescending(x => x.Discount);
                return _mapper.Map<IEnumerable<GetAllAccountVoucherDTO>>(voulist);
            }
            return null;
        }

        public async Task<PageResult<GetAllAccountVoucherDTO>> GetAllAccountVoucher(AccountVoucherFilter accountVoucherFilter)
        {
            var bookList = _mapper.Map<IEnumerable<GetAllAccountVoucherDTO>>(await _unitOfWork.AccountVoucherRepo.GetAllAccountVoucher());
            IQueryable<GetAllAccountVoucherDTO> result = bookList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(accountVoucherFilter.UserName))
                result = result.Where(x => x.Username.Contains(accountVoucherFilter.UserName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(accountVoucherFilter.VouName))
                result = result.Where(x => x.VoucherName.Contains(accountVoucherFilter.VouName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(accountVoucherFilter.VouCode))
                result = result.Where(x => x.VouCode.Contains(accountVoucherFilter.VouCode, StringComparison.OrdinalIgnoreCase));

            if (accountVoucherFilter.CustomerID != null)
                result = result.Where(x => x.CustomerID.Equals(accountVoucherFilter.CustomerID));

            if (accountVoucherFilter.VoucherID != null)
                result = result.Where(x => x.VoucherID.Equals(accountVoucherFilter.VoucherID));

            if (accountVoucherFilter.IsAvailable != null)
                result = result.Where(x => x.IsAvailable.Equals(accountVoucherFilter.IsAvailable));

            //Sorting
            if (!string.IsNullOrEmpty(accountVoucherFilter.SortBy))
            {
                switch (accountVoucherFilter.SortBy)
                {
                    case "discount":
                        result = accountVoucherFilter.SortAscending ?
                            result.OrderBy(x => x.Discount) :
                            result.OrderByDescending(x => x.Discount);
                        break;
                }
            }

            //Paging
            var pageItems = result
                .Skip((accountVoucherFilter.PageNumber - 1) * accountVoucherFilter.PageSize)
                .Take(accountVoucherFilter.PageSize)
                .ToList();

            return new PageResult<GetAllAccountVoucherDTO>
            {
                Items = pageItems,
                PageNumber = accountVoucherFilter.PageNumber,
                PageSize = accountVoucherFilter.PageSize,
                TotalItem = result.Count(),
                TotalPages = (int)Math.Ceiling((decimal)result.Count() / (decimal)accountVoucherFilter.PageSize)
            };
        }

        public async Task<string?> UseAccountVoucher(Guid id)
        {
            try
            {
                var accVou = await _unitOfWork.AccountVoucherRepo.GetByIdAsync(id);
                if (accVou == null)
                {
                    return "Không tìm thấy khuyến mãi mà tài khoản này sở hữu!";
                }
                accVou.IsAvailable = false;
                _unitOfWork.AccountVoucherRepo.Update(accVou);
                var result = await _unitOfWork.SaveAsync() > 0;
                if(!result)
                {
                    return "Cập nhật thất bại!";
                }

                return null;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
