using Microsoft.AspNetCore.Mvc.RazorPages;
using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IReviewService 
    {
        Task<string?> CreateReview(CreateReviewDTO reviewDTO);

        Task<PageResult<GetReviewDTO>> GetAllReview(ReviewFilter review);

        Task<string?> DeleteReview(Guid Id);
    }
}
