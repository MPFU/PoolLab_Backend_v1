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
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public VoucherService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
        }

        public async Task<string?> AddNewVoucher(NewVoucherDTO newVoucherDTO)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                var voucher = _mapper.Map<Voucher>(newVoucherDTO);
                voucher.Id = Guid.NewGuid();
                voucher.CreatedDate = now;
                voucher.Status = "Kích Hoạt";
                voucher.VouCode = $"V{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
                await _unitOfWork.VoucherRepo.AddAsync(voucher);
                var result = await _unitOfWork.SaveAsync() > 0;
                if(!result){
                    return "Tạo voucher thất bại!";
                }
                return null;

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageResult<VoucherDTO>?> GetAllVoucher(VoucherFilter vouFilter)
        {
            var vouList = _mapper.Map<IEnumerable<VoucherDTO>>(await _unitOfWork.VoucherRepo.GetAllAsync());
            IQueryable<VoucherDTO> result = vouList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(vouFilter.Name))
                result = result.Where(x => x.Name.Contains(vouFilter.Name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(vouFilter.VouCode))
                result = result.Where(x => x.VouCode.Contains(vouFilter.VouCode, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(vouFilter.Status))
                result = result.Where(x => x.Status.Contains(vouFilter.Status, StringComparison.OrdinalIgnoreCase));
        

            //Sorting
            if (!string.IsNullOrEmpty(vouFilter.SortBy))
            {
                switch (vouFilter.SortBy)
                {
                    case "updatedDate":
                        result = vouFilter.SortAscending ?
                            result.OrderBy(x => x.UpdatedDate) :
                            result.OrderByDescending(x => x.UpdatedDate);
                        break;
                    case "createdDate":
                        result = vouFilter.SortAscending ?
                            result.OrderBy(x => x.CreatedDate) :
                            result.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "discount":
                        result = vouFilter.SortAscending ?
                            result.OrderBy(x => x.Discount) :
                            result.OrderByDescending(x => x.Discount);
                        break;
                    case "point":
                        result = vouFilter.SortAscending ?
                            result.OrderBy(x => x.Point) :
                            result.OrderByDescending(x => x.Point);
                        break;
                }
            }

            //Paging
            var pageItems = result
                .Skip((vouFilter.PageNumber - 1) * vouFilter.PageSize)
                .Take(vouFilter.PageSize)
                .ToList();

            return new PageResult<VoucherDTO>
            {
                Items = pageItems,
                PageNumber = vouFilter.PageNumber,
                PageSize = vouFilter.PageSize,
                TotalItem = result.Count(),
                TotalPages = (int)Math.Ceiling((decimal)result.Count() / (decimal)vouFilter.PageSize)
            };
        }

        public async Task<VoucherDTO?> GetVoucherById(Guid id)
        {
            return _mapper.Map<VoucherDTO>(await _unitOfWork.VoucherRepo.GetByIdAsync(id));
        }

        public async Task<string?> InActiveVoucher(Guid Id)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                var voucher = await _unitOfWork.VoucherRepo.GetByIdAsync(Id);
                if(voucher == null)
                {
                    return "Không tìm thấy khuyến mãi này!";
                }
                await _unitOfWork.BeginTransactionAsync();
                var accVou = await _unitOfWork.AccountVoucherRepo.GetAllAccountVoucherByVouOrAccID(voucher.Id);

                if(accVou != null)
                {
                    foreach (var vou in accVou)
                    {
                        var acc = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)vou.CustomerID);

                        if (acc == null)
                        {
                            return "Không tìm thấy tài khoản của khuyến mãi này!";
                        }

                        var upAcc = await _accountService.UpdateAccPoint(acc.Id, (int)(acc.Point + voucher.Point));
                        if (upAcc != null)
                        {
                            return upAcc;
                        }

                        _unitOfWork.AccountVoucherRepo.Delete(vou);
                        var result1 = await _unitOfWork.SaveAsync() > 0;
                        if (!result1)
                        {
                            return "Cập nhật voucher thất bại!";
                        }
                    }
                }
                
                voucher.Status = "Vô Hiệu";
                 _unitOfWork.VoucherRepo.Update(voucher);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật voucher thất bại!";
                }

                await _unitOfWork.CommitTransactionAsync();
                return null;

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> ReActiveVoucher(Guid Id)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                var voucher = await _unitOfWork.VoucherRepo.GetByIdAsync(Id);
                if (voucher == null)
                {
                    return "Không tìm thấy khuyến mãi này!";
                }

                voucher.Status = "Kích Hoạt";
                _unitOfWork.VoucherRepo.Update(voucher);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật voucher thất bại!";
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> UpdateVoucher(Guid Id, NewVoucherDTO newVoucherDTO)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                var voucher = await _unitOfWork.VoucherRepo.GetByIdAsync(Id);
                if (voucher == null)
                {
                    return "Không tìm thấy khuyến mãi này!";
                }

                voucher.Name = !string.IsNullOrEmpty(newVoucherDTO.Name) ? newVoucherDTO.Name : voucher.Name;
                voucher.Description = !string.IsNullOrEmpty(newVoucherDTO.Description) ? newVoucherDTO.Description : voucher.Description;
                voucher.Discount = voucher.Discount != null ? newVoucherDTO.Discount : voucher.Discount;
                voucher.Point = voucher.Point != null ? newVoucherDTO.Point : voucher.Point;
                voucher.UpdatedDate = now;
                _unitOfWork.VoucherRepo.Update(voucher);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật voucher thất bại!";
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
