using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;


        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
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

                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                account.JoinDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                account.AvatarUrl = "https://capstonepoollab.blob.core.windows.net/avatar/2755cb64-c635-421f-b695-bd0ba80608ed.jpg";
                account.Point = 0;
                account.Balance = 0;
                account.Tier = 0;
                account.Status = "Kích hoạt";
                var role1 = await _unitOfWork.RoleRepo.GetRoleByName("Member");
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

        public async Task<string?> CreateAccount(CreateAccDTO createAccDTO)
        {
            try
            {
                AccountDTO account = new AccountDTO();
                var check = await _unitOfWork.AccountRepo.CheckDuplicateEmailvsUsername(null, createAccDTO.Email, createAccDTO.UserName);
                if (check != null)
                {
                    return check;
                }
                account.Id = Guid.NewGuid();
                account.Email = createAccDTO.Email;
                account.AvatarUrl = !string.IsNullOrEmpty(createAccDTO.AvatarUrl)  ? createAccDTO.AvatarUrl : "https://capstonepoollab.blob.core.windows.net/avatar/2755cb64-c635-421f-b695-bd0ba80608ed.jpg";
                account.PhoneNumber = createAccDTO.PhoneNumber;
                account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createAccDTO.PasswordHash);
                account.UserName = createAccDTO.UserName;   
                account.FullName = createAccDTO.FullName;
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                account.JoinDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                account.Point = 0;
                account.Balance = 0;
                account.Tier = 0;
                account.TotalTime = 0;
                account.Status = "Kích Hoạt";                              
                var roleId = await _unitOfWork.RoleRepo.GetRoleByName(createAccDTO.RoleName);
                if (roleId == null)
                {
                    return "Không tìm thấy chức vụ này !";
                }              
                account.RoleId = roleId.Id;
                if(createAccDTO.RoleName.Equals("Super Manager") || createAccDTO.RoleName.Equals("Admin"))
                {
                    account.StoreId = null;
                    account.CompanyId = createAccDTO.CompanyId;
                }
                else
                {
                    account.StoreId = createAccDTO.StoreId;
                    account.CompanyId = null;
                }
               
                await _unitOfWork.AccountRepo.AddAsync(_mapper.Map<Account>(account));
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo tài khoản thất bại.";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> DepositBalance(Guid Id, decimal amount)
        {
            try
            {
                var cus = await _unitOfWork.AccountRepo.GetByIdAsync(Id);
                if(cus == null)
                {
                    return "Không tìm thấy thành viên này!";
                }

                if(amount <= 0 )
                {
                    return "Số tiền của bạn không hợp lệ!";
                }

                cus.Balance = cus.Balance + amount;
                _unitOfWork.AccountRepo.Update(cus);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Nạp tiền thất bại!";
                }
             
                return null ;
            }
            catch(DbUpdateException)
            {
                throw;
            }
        }

        public async Task<AccountLoginDTO?> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            var acc = await _unitOfWork.AccountRepo.GetAccountByEmailOrUsername(email);
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


        public async Task<PageResult<GetAllAccDTO>> GetAllAccount(AccountFilter accountFilter)
        {
            var accList = _mapper.Map<IEnumerable<GetAllAccDTO>>(await _unitOfWork.AccountRepo.GetAllAccounts());
            IQueryable<GetAllAccDTO> result = accList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(accountFilter.UserName))
                result = result.Where(x => x.UserName.Contains(accountFilter.UserName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(accountFilter.FullName))
                result = result.Where(x => x.FullName.Contains(accountFilter.FullName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(accountFilter.PhoneNumber))
                result = result.Where(x => x.PhoneNumber.Contains(accountFilter.PhoneNumber, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(accountFilter.Rank))
                result = result.Where(x => x.Rank.Contains(accountFilter.Rank, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(accountFilter.Status))
                result = result.Where(x => x.Status.Contains(accountFilter.Status, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(accountFilter.RoleName))
                result = result.Where(x => x.RoleName.Contains(accountFilter.RoleName, StringComparison.OrdinalIgnoreCase));

            if (accountFilter.RoleId != null)
                result = result.Where(x => x.RoleId == accountFilter.RoleId);

            if (accountFilter.StoreId != null)
                result = result.Where(x => x.StoreId == accountFilter.StoreId);

            if (accountFilter.CompanyId != null)
                result = result.Where(x => x.CompanyId == accountFilter.CompanyId);

            if (accountFilter.SubId != null)
                result = result.Where(x => x.SubId == accountFilter.SubId);

            //Sorting
            if (!string.IsNullOrEmpty(accountFilter.SortBy))
            {
                switch (accountFilter.SortBy)
                {
                    case "joinDate":
                        result = accountFilter.SortAscending ?
                            result.OrderBy(x => x.JoinDate) :
                            result.OrderByDescending(x => x.JoinDate);
                        break;
                    case "point":
                        result = accountFilter.SortAscending ? 
                            result.OrderBy(x => x.Point) :
                            result.OrderByDescending(x => x.Point);
                        break;
                    case "balance":
                        result = accountFilter.SortAscending ?
                            result.OrderBy(x => x.Balance) :
                            result.OrderByDescending(x => x.Balance);
                        break;
                    case "totalTime":
                        result = accountFilter.SortAscending ?
                            result.OrderBy(x => x.TotalTime) :
                            result.OrderByDescending(x => x.TotalTime);
                        break;
                }
            }

            //Paging
            var pageItems = result
                .Skip((accountFilter.PageNumber - 1) * accountFilter.PageSize)
                .Take(accountFilter.PageSize)
                .ToList();

            return new PageResult<GetAllAccDTO>
            {
                Items = pageItems,
                PageNumber = accountFilter.PageNumber,
                PageSize = accountFilter.PageSize,
                TotalItem = result.Count(),
                TotalPages = (int)Math.Ceiling((decimal)result.Count() / (decimal)accountFilter.PageSize)
            };
        }

        //Login of Manager, Staff,...
        public async Task<GetLoginAccDTO?> GetLoginAcc(LoginAccDTO loginAccDTO)
        {           
            var acc = await _unitOfWork.AccountRepo.GetAccountLoginStaff(loginAccDTO.Email);
            if(acc != null)
            {
                if(BCrypt.Net.BCrypt.Verify(loginAccDTO.Password, acc.PasswordHash))
                {
                    return _mapper.Map<GetLoginAccDTO>(acc);
                }
            }
            return null;
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

        public async Task<string?> UpdateBalance(Guid Id, decimal amount)
        {
            try
            {
                var acc = await _unitOfWork.AccountRepo.GetByIdAsync(Id);
                if (acc == null)
                {
                    return "Không tìm thấy tài khoản này!";
                }
                if(amount < 0)
                {
                    amount = 0;
                }
                acc.Balance = amount;
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật số dư thất bại!";
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

        public async Task<string?> UpdateAccStatus(Guid Id, UpdateAccStatusDTO statusDTO)
        {
            try
            {
                var acc = await _unitOfWork.AccountRepo.GetByIdAsync(Id);
                if (acc == null)
                {
                    return "Không tìm thấy tài khoản này!";
                }
               
                acc.Status = !string.IsNullOrEmpty(statusDTO.Status) ? statusDTO.Status : acc.Status; 
                _unitOfWork.AccountRepo.Update(acc);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật trạng thái thất bại!";
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
