using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IPaymentRepo : IGenericRepo<Transaction>
    {
        Task<IEnumerable<Transaction>> GetAllTransaction();

        Task<IEnumerable<Transaction>> GetAllOrderTransaction();

        Task<IEnumerable<Transaction>> GetAllCusTransaction();

        Task<Transaction?> GetOrderTransaction(Guid id);
    }
}
