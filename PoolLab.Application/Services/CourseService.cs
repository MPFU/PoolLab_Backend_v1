using AutoMapper;
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
    public class CourseService : ICourseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageResult<GetAllCoursesDTO>> GetAllCourses(CourseFilter courseFilter)
        {
            var courseList = _mapper.Map<IEnumerable<GetAllCoursesDTO>>(await _unitOfWork.CourseRepo.GetAllCourses());
            IQueryable<GetAllCoursesDTO> result = courseList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(courseFilter.Title))
                result = result.Where(x => x.Title.Contains(courseFilter.Title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(courseFilter.Schedule))
                result = result.Where(x => x.Schedule.Contains(courseFilter.Schedule, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(courseFilter.Descript))
                result = result.Where(x => x.Descript.Contains(courseFilter.Descript, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(courseFilter.Status))
                result = result.Where(x => x.Status.Contains(courseFilter.Status, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(courseFilter.Level))
                result = result.Where(x => x.Level.Contains(courseFilter.Level, StringComparison.OrdinalIgnoreCase));

            if (courseFilter.AccountId != null)
                result = result.Where(x => x.AccountId == courseFilter.AccountId);

            if (courseFilter.Quantity.HasValue)
                result = result.Where(x => x.Quantity == courseFilter.Quantity);

            if (courseFilter.NoOfUser.HasValue)
                result = result.Where(x => x.NoOfUser == courseFilter.NoOfUser);

            //Sorting
            if (!string.IsNullOrEmpty(courseFilter.SortBy))
            {
                switch (courseFilter.SortBy)
                {
                    case "createdDate":
                        result = courseFilter.SortAscending ?
                            result.OrderBy(x => x.CreatedDate) :
                            result.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "updatedDate":
                        result = courseFilter.SortAscending ?
                            result.OrderBy(x => x.UpdatedDate) :
                            result.OrderByDescending(x => x.UpdatedDate);
                        break;
                    case "price":
                        result = courseFilter.SortAscending ?
                            result.OrderBy(x => x.Price) :
                            result.OrderByDescending(x => x.Price);
                        break;
                    case "quantity":
                        result = courseFilter.SortAscending ?
                            result.OrderBy(x => x.Quantity) :
                            result.OrderByDescending(x => x.Quantity);
                        break;
                    case "noofuser":
                        result = courseFilter.SortAscending ?
                            result.OrderBy(x => x.NoOfUser) :
                            result.OrderByDescending(x => x.NoOfUser);
                        break;
                }
            }

            //Paging
            var pageItems = result
                .Skip((courseFilter.PageNumber - 1) * courseFilter.PageSize)
                .Take(courseFilter.PageSize)
                .ToList();

            return new PageResult<GetAllCoursesDTO>
            {
                Items = pageItems,
                PageNumber = courseFilter.PageNumber,
                PageSize = courseFilter.PageSize,
                TotalItem = result.Count(),
                TotalPages = (int)Math.Ceiling((decimal)result.Count() / (decimal)courseFilter.PageSize)
            };
        }

        public async Task<string?> CreateCourseDTO(CreateCourseDTO create)
        {
            try
            {
                DateTime now = DateTime.Now;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var check = _mapper.Map<Course>(create);
                check.Id = Guid.NewGuid();
                
                check.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(now, localTimeZone);
                check.Status = "Đang hoạt động";
                check.MentorId = null;
                await _unitOfWork.CourseRepo.AddAsync(check);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo mới khoá học thất bại!";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
