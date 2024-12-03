using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

namespace PoolLab.Application.Interface
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> CreateTransactionBooking(PaymentBookingDTO paymentBookingDTO)
        {
            try
            {
                var pay =  _mapper.Map<Transaction>(paymentBookingDTO);
                pay.Id = Guid.NewGuid();
                pay.Status = "Hoàn Tất";
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                pay.PaymentDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                await _unitOfWork.PaymentRepo.AddAsync(pay);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo giao dịch thất bại!";
                }
                return null;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> CreateTransactionDeposit(PaymentDepositDTO deposiDTO)
        {
            try
            {
                var pay = _mapper.Map<Transaction>(deposiDTO);
                pay.Id = Guid.NewGuid();
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                pay.PaymentDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                await _unitOfWork.PaymentRepo.AddAsync(pay);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo giao dịch thất bại!";
                }
                return pay.Id.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageResult<PaymentDTO>> GetAllTransaction(PaymentFilter paymentFilter)
        {
            var tranList = _mapper.Map<IEnumerable<PaymentDTO>>(await _unitOfWork.PaymentRepo.GetAllTransaction());
            IQueryable<PaymentDTO> query = tranList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(paymentFilter.Username))
                query = query.Where(x => x.Username.Contains(paymentFilter.Username, StringComparison.OrdinalIgnoreCase));

            if (paymentFilter.OrderId != null)
                query = query.Where(x => x.OrderId.Equals(paymentFilter.OrderId));

            if (paymentFilter.AccountId != null)
                query = query.Where(x => x.AccountId.Equals(paymentFilter.AccountId));

            if (paymentFilter.SubId != null)
                query = query.Where(x => x.SubId.Equals(paymentFilter.SubId));

            if (paymentFilter.TypeCode != null)
                query = query.Where(x => x.TypeCode == paymentFilter.TypeCode);

            if (!string.IsNullOrEmpty(paymentFilter.Status))
                query = query.Where(x => x.Status.Contains(paymentFilter.Status, StringComparison.OrdinalIgnoreCase));

            //Sorting
            if (!string.IsNullOrEmpty(paymentFilter.SortBy))
            {
                switch (paymentFilter.SortBy)
                {
                    case "paymentDate":
                        query = paymentFilter.SortAscending ?
                            query.OrderBy(x => x.PaymentDate) :
                            query.OrderByDescending(x => x.PaymentDate);
                        break;
                    case "amount":
                        query = paymentFilter.SortAscending ?
                            query.OrderBy(x => x.Amount) :
                            query.OrderByDescending(x => x.Amount);
                        break;
                }
            }

            //Paging
            var pageItems = query
                .Skip((paymentFilter.PageNumber - 1) * paymentFilter.PageSize)
                .Take(paymentFilter.PageSize)
                .ToList();

            return new PageResult<PaymentDTO>
            {
                Items = pageItems,
                PageNumber = paymentFilter.PageNumber,
                PageSize = paymentFilter.PageSize,
                TotalItem = query.Count(),
                TotalPages = (int)Math.Ceiling((decimal)query.Count() / (decimal)paymentFilter.PageSize)
            };
        }

        public async Task<string?> UpdateTransactionStatus(Guid Id, string status)
        {
            try
            {
                var pay = await _unitOfWork.PaymentRepo.GetByIdAsync(Id);
                if (pay == null)
                {
                    return "Không tìm thấy giao dịch này!";
                }
                pay.Status = status;
                _unitOfWork.PaymentRepo.Update(pay);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật giao dịch thất bại!";
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
