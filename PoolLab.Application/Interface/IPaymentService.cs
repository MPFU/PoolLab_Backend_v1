using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IPaymentService 
    {
        Task<string?> CreateTransactionBooking(PaymentBookingDTO paymentBookingDTO);

        Task<PageResult<PaymentDTO>> GetAllTransaction(PaymentFilter paymentFilter);

        Task<PageResult<GetAllOrderTransactionDTO>> GetAllOrderTransaction(PaymentOrderFilter paymentFilter);

        Task<string?> CreateTransactionDeposit(PaymentDepositDTO deposiDTO);

        Task<string?> UpdateTransactionStatus(Guid Id, string status);
    }
}
