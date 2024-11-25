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
    public class ReviewRepo : GenericRepo<Review>, IReviewRepo
    {
        public ReviewRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Review>?> GetAllReviews()
        {
           return await _dbContext.Reviews.Include(x => x.Store).Include(x => x.Customer).ToListAsync();
        }

        public async Task<Review?> GetReview(Guid id)
        {
            return await _dbContext.Reviews.Include(x => x.Store).Include(x => x.Customer).FirstOrDefaultAsync();
        }
    }
}
