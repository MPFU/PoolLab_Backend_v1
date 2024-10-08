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

        public async Task<string?> CheckDuplicateEmailvsUsername(Guid? id, string? email, string? username)
        {
            Account? checkE = null;
            Account? checkU = null;
            if (!string.IsNullOrEmpty(email))
            {
                checkE = await _dbContext.Accounts.Where(x => x.Email.Equals(email)).FirstOrDefaultAsync();
            }

            if (!string.IsNullOrEmpty(username))
            {
                checkU = await _dbContext.Accounts.Where(x => x.UserName.Equals(username)).FirstOrDefaultAsync();
            }

            if (id != null)
            {
                if ((checkE != null && checkU != null) && (!checkE.Id.Equals(id) && !checkU.Id.Equals(id)))
                {
                    return "Email và Username của bạn đã bị trùng!";
                }
                else if (checkE != null && !checkE.Id.Equals(id))
                {
                    return "Email của bạn đã bị trùng!";
                }
                else if (checkU != null && !checkU.Id.Equals(id))
                {
                    return "Username của bạn đã bị trùng!";
                }
                else
                {
                    return null;
                }
            }

            if (checkE != null && checkU != null)
            {
                return "Email và Username của bạn đã bị trùng!";
            }
            else if (checkE != null)
            {
                return "Email của bạn đã bị trùng!";
            }
            else if (checkU != null)
            {
                return "Username của bạn đã bị trùng!";
            }
            else
            {
                return null;
            }
        }

        public async Task<Account?> GetAccountByEmailOrUsername(string email)
        {
            return await _dbContext.Accounts
                .Where(x => x.Email.Equals(email) || x.UserName.Equals(email))
                .Include(x => x.Role)
                .FirstOrDefaultAsync();
        }

        public async Task<Account?> GetAccountLoginStaff(string email, Guid? id)
        {

            return await _dbContext.Accounts
                           .Where(x => x.Email.Equals(email) || x.UserName.Equals(email))
                           .Where(x => x.StoreId.Equals(id) || x.CompanyId.Equals(id))
                           .Include(x => x.Role)
                           .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await _dbContext.Accounts
                .Include(x => x.Role)
                .Include(x => x.Store)
                .Include(x => x.Company)
                .Include(x => x.Sub)
                .ToListAsync();
        }
    }
}
