using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
                pay.Status = "Hoàn tất";
                pay.PaymentDate = DateTime.UtcNow;
                await _unitOfWork.PaymentRepo.AddAsync(pay);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo giao dịch thất bại!";
                }
                return null;
            }catch(DbUpdateException)
            {
                throw;
            }
        }
    }
}
