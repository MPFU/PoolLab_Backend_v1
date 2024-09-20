using Microsoft.EntityFrameworkCore;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Interface
{
    public class AccountRepo : GenericRepo<Account>, IAccountRepo
    {
        public AccountRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<string?> CheckDuplicateEmailvsUsername(string email, string username)
        {
            var checkE = await _dbContext.Accounts.Where(x => x.Email.Equals(email)).FirstOrDefaultAsync();
            var checkU = await _dbContext.Accounts.Where(x => x.UserName.Equals(username)).FirstOrDefaultAsync();
            if(checkE != null && checkU != null)
            {
                return "Email và Username của bạn đã bị trùng!";
            }else if(checkE != null)
            {
                return "Email của bạn đã bị trùng!";
            }else if(checkU != null)
            {
                return "Username của bạn đã bị trùng!";
            }
            else
            {
                return null;
            }
        }

        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _dbContext.Accounts
                .Where(x => x.Email.Equals(email))
                .Include(x => x.Role)
                .FirstOrDefaultAsync();
        }
    }
}
