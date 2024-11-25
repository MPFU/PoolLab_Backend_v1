using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IReviewRepo : IGenericRepo<Review>
    {
        Task<Review?> GetReview(Guid id);

        Task<IEnumerable<Review>?> GetAllReviews();
    }
}
