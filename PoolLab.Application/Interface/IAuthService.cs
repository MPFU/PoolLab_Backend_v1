using PoolLab.Application.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDTO loginData);
        Task<string> RegisterAsync(RegisterDTO registerData);
        Task<string> LoginStaffAsync(LoginAccDTO loginAccDTO);
    }
}
