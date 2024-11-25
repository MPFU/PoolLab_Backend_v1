using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStoreService _storeService;

        public ReviewService(IMapper mapper, IUnitOfWork unitOfWork, IStoreService storeService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _storeService = storeService;

        }
        public async Task<string?> CreateReview(CreateReviewDTO reviewDTO)
        {
            try
            {
                var store = await _unitOfWork.StoreRepo.GetByIdAsync((Guid)reviewDTO.StoreId);
                if (store == null)
                {
                    return "Không tìm thấy chi nhánh này!";
                }

                var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)reviewDTO.CustomerId);
                if(cus == null)
                {
                    return "Không tìm thấy thành viên này!";
                }

                var review = _mapper.Map<Review>(reviewDTO);
                review.Id = Guid.NewGuid();
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                review.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                await _unitOfWork.ReviewRepo.AddAsync(review);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Đánh giá thất bại";
                }

                if(review.Rated != null)
                {
                    var upRate = await _storeService.UpdateStoreRating((Guid)review.StoreId, (int)review.Rated);
                    if(upRate != null)
                    {
                        return upRate;
                    }
                }
                return null;
            }
            catch(DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> DeleteReview(Guid Id)
        {
            try
            {
                var review = await _unitOfWork.ReviewRepo.GetByIdAsync(Id);
                if (review == null) 
                { 
                    return "Không tìm thấy đánh giá này!"; 
                }

                _unitOfWork.ReviewRepo.Delete(review);
                var result = await _unitOfWork.SaveAsync() > 0;
                return null;
            }catch(DbUpdateException)
            {
                throw;
            }
        }

        public async Task<PageResult<GetReviewDTO>> GetAllReview(ReviewFilter review)
        {
            var reviewList = _mapper.Map<IEnumerable<GetReviewDTO>>(await _unitOfWork.ReviewRepo.GetAllReviews());
            IQueryable<GetReviewDTO> query = reviewList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(review.Username))
                query = query.Where(x => x.CusName.Contains(review.Username, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(review.StoreName))
                query = query.Where(x => x.StoreName.Contains(review.StoreName, StringComparison.OrdinalIgnoreCase));

            if (review.StoreId != null)
                query = query.Where(x => x.StoreId.Equals(review.StoreId));

            if (review.CustomerId != null)
                query = query.Where(x => x.CustomerId.Equals(review.CustomerId));        

            

            //Sorting
            if (!string.IsNullOrEmpty(review.SortBy))
            {
                switch (review.SortBy)
                {
                    case "createdDate":
                        query = review.SortAscending ?
                            query.OrderBy(x => x.CreatedDate) :
                            query.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "rated":
                        query = review.SortAscending ?
                            query.OrderBy(x => x.Rated) :
                            query.OrderByDescending(x => x.Rated);
                        break;
                }
            }

            //Paging
            var pageItems = query
                .Skip((review.PageNumber - 1) * review.PageSize)
                .Take(review.PageSize)
                .ToList();

            return new PageResult<GetReviewDTO>
            {
                Items = pageItems,
                PageNumber = review.PageNumber,
                PageSize = review.PageSize,
                TotalItem = query.Count(),
                TotalPages = (int)Math.Ceiling((decimal)query.Count() / (decimal)review.PageSize)
            };
        }


    }
}
