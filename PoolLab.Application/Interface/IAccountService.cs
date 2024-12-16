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
    public interface IAccountService 
    {
        Task<string?> AddNewUser(RegisterDTO registerDTO);

        Task<AccountLoginDTO?> GetAccountByEmailAndPasswordAsync(string email, string password);

        Task<AccountDTO?> GetAccountById(Guid Id);

        Task<string?> CreateAccount(CreateAccDTO createAccDTO);

        Task<string?> UpdateAccountInfo(Guid Id, UpdateAccDTO updateAccDTO);

        Task<string?> UpdatePassword(Guid Id, UpdatePassDTO password);

        Task<GetLoginAccDTO?> GetLoginAcc(LoginAccDTO loginAccDTO);

        Task<PageResult<GetAllAccDTO>> GetAllAccount(AccountFilter accountFilter);

        Task<string?> UpdateBalance(Guid Id, decimal amount);

        Task<string?> DepositBalance(Guid Id, decimal amount);

        Task<string?> UpdateAccStatus(Guid Id, UpdateAccStatusDTO statusDTO);

        Task<string?> UpdateAccPoint(Guid Id, int point);
    }
}
