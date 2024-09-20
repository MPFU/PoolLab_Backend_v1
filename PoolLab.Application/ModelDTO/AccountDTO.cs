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
    }

    public class AccountLoginDTO
    {
        public Guid Id { get; set; }

        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? Status { get; set; }

        public virtual Role? Role { get; set; }
    }

    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
