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
    }
}
