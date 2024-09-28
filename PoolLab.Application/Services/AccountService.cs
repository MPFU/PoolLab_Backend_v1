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


        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string?> AddNewUser(RegisterDTO registerDTO)
        {
            try
            {
                var check = await _unitOfWork.AccountRepo.CheckDuplicateEmailvsUsername(null,registerDTO.Email, registerDTO.UserName);
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
                var role1 = await _unitOfWork.RoleRepo.GetRoleByName("Customer");
                account.RoleId = role1.Id;
                await _unitOfWork.AccountRepo.AddAsync(account);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo tài khoản thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<AccountLoginDTO?> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            var acc = await _unitOfWork.AccountRepo.GetAccountByEmail(email);
            if (acc != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, acc.PasswordHash))
                {
                    return _mapper.Map<AccountLoginDTO>(acc);
                }
            }
            return null;
        }

        public async Task<AccountDTO?> GetAccountById(Guid Id)
        {
            return _mapper.Map<AccountDTO?>(await _unitOfWork.AccountRepo.GetByIdAsync(Id));
        }

        public async Task<string?> UpdateAccountInfo(Guid Id, UpdateAccDTO updateAccDTO)
        {
            try
            {
                var acc = await _unitOfWork.AccountRepo.GetByIdAsync(Id);
                if (acc == null)
                {
                    return "Không tìm thấy tài khoản này!";
                }
                var checkdup = await _unitOfWork.AccountRepo.CheckDuplicateEmailvsUsername(Id,updateAccDTO.Email, updateAccDTO.UserName);
                if(checkdup != null)
                {
                    return checkdup;
                }
                acc.Email = !string.IsNullOrEmpty(updateAccDTO.Email) ? updateAccDTO.Email : acc.Email;              
                acc.UserName = !string.IsNullOrEmpty(updateAccDTO.UserName)  ? updateAccDTO.UserName : acc.UserName;
                acc.FullName = !string.IsNullOrEmpty(updateAccDTO.FullName) ? updateAccDTO.FullName : acc.FullName;
                acc.AvatarUrl = !string.IsNullOrEmpty(updateAccDTO.AvatarUrl) ? updateAccDTO.AvatarUrl : acc.AvatarUrl;
                acc.PhoneNumber = !string.IsNullOrEmpty(updateAccDTO.PhoneNumber )? updateAccDTO.PhoneNumber : acc.PhoneNumber;
                _unitOfWork.AccountRepo.Update(acc);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> UpdatePassword(Guid Id, UpdatePassDTO password)
        {
            try
            {
                var acc = await _unitOfWork.AccountRepo.GetByIdAsync(Id);
                if (acc == null)
                {
                    return "Không tìm thấy tài khoản này!";
                }
                if (!BCrypt.Net.BCrypt.Verify(password.OldPassword, acc.PasswordHash))
                {
                    return "Mật khẩu cũ không trùng khớp!";
                }
                acc.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password.NewPassword);
                _unitOfWork.AccountRepo.Update(acc);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật mật khẩu thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}
