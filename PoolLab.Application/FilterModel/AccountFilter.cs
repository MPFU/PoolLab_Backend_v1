using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class AccountFilter : FilterOption<Account>
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Rank { get; set; }
        public string? Status { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? SubId { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
