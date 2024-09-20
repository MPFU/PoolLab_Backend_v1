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
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService( IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string?> AddNewUser(RegisterDTO registerDTO)
        {
            try
            {
                var check = await _unitOfWork.AccountRepo.CheckDuplicateEmailvsUsername(registerDTO.Email, registerDTO.UserName);
                if (check != null)
                {
                    return check;
                }
                var account = _mapper.Map<Account>(registerDTO);
                account.Id = Guid.NewGuid();
                account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
                account.JoinDate = DateTime.UtcNow;
                account.Point = 0;
                account.Balance = 0;
                account.Tier = 0;
                account.TotalTime = 0;
                account.Status = "Kích hoạt";
                await _unitOfWork.AccountRepo.AddAsync(account);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo tài khoản thất bại!";
                }
                return null;
            }catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<AccountLoginDTO?> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            var acc = await _unitOfWork.AccountRepo.GetAccountByEmail(email);
            if (acc != null)
            {
                if(BCrypt.Net.BCrypt.Verify(password, acc.PasswordHash))
                {
                    return _mapper.Map<AccountLoginDTO>(acc);
                }
            }
            return null;
        }
    }
}
