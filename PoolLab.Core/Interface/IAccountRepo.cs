using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IAccountRepo : IGenericRepo<Account>
    {
        Task<string?> CheckDuplicateEmailvsUsername(Guid? id,string? email, string? username);
        Task<Account?> GetAccountByEmailOrUsername(string email);
        Task<Account?> GetAccountLoginStaff(string email);
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<decimal?> GetAccountBalanceByID(Guid id);
        Task<Account?> GetAccountById(Guid id);
    }
}
