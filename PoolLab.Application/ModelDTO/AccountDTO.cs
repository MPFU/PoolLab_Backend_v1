using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class AccountDTO
    {
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public string? AvatarUrl { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? RoleId { get; set; }

        public Guid? StoreId { get; set; }

        public int? Point { get; set; }

        public decimal? Balance { get; set; }

        public int? TotalTime { get; set; }

        public string? Rank { get; set; }

        public int? Tier { get; set; }

        public Guid? SubId { get; set; }

        public DateTime? JoinDate { get; set; }

        public string? Status { get; set; }

        public AccountDTO()
        {

        }
    }



    public class AccountLoginDTO
    {
        public Guid Id { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? Status { get; set; }

        public Role? Role { get; set; }
    }

    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CreateAccDTO
    {
        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public string? AvatarUrl { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? RoleName { get; set; }

        public Guid? StoreId { get; set; }

    }

    public class UpdateAccDTO
    {
        public string? Email { get; set; }

        public string? AvatarUrl { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class UpdatePassDTO
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }

    public class UpdateAccStatusDTO
    {
        public string? Status { get; set; }
    }

    public class DepositBalanceDTO
    {
        public decimal Amount { get; set; }
    }

    public class GetLoginAccDTO
    {
        public Guid Id { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? Status { get; set; }

        public Role? Role { get; set; }

        public Guid? StoreId { get; set; }

    }

    public class LoginAccDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class GetAllAccDTO
    {
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? AvatarUrl { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? RoleId { get; set; }

        public string? RoleName { get; set; }

        public Guid? StoreId { get; set; }

        public int? Point { get; set; }

        public decimal? Balance { get; set; }

        public int? TotalTime { get; set; }

        public string? Rank { get; set; }

        public int? Tier { get; set; }

        public Guid? SubId { get; set; }

        public DateTime? JoinDate { get; set; }

        public string? Status { get; set; }
    }

    public class AccVerifyDTO
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
    }
}
