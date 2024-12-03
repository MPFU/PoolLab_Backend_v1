using AutoMapper;
using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

            if (courseFilter.StoreId != null)
                result = result.Where(x => x.StoreId == courseFilter.StoreId);

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

        public async Task<string?> CreateCourse(CreateCourseDTO create)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var a = TimeZoneInfo.ConvertTimeFromUtc(now, localTimeZone);
                var date = DateOnly.FromDateTime(a);


                var check = _mapper.Map<Course>(create);

                if (check.StartDate <= date)
                {
                    return "Ngày bắt đầu không hợp lệ!";
                }

                if (check.EndDate <= check.StartDate)
                {
                    return "Ngày bắt đầu và kết thúc không hợp lệ!";
                }

                if (check.EndTime <= check.StartTime)
                {
                    return "Thời gian bắt đầu và kết thúc không hợp lệ!";
                }

                if (check.Quantity == null || check.Quantity <= 0)
                {
                    return "Số lượng khoá học không hợp lệ!";
                }

                if (check.Price == null || check.Price <= 0)
                {
                    return "Học phí khoá học không hợp lệ!";
                }

                check.Id = Guid.NewGuid();
                check.NoOfUser = 0;
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
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> UpdateCourse(Guid id, UpdateCourseDTO update)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

                var check = await _unitOfWork.CourseRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy khoá học này.";
                }

                check.Title = !string.IsNullOrEmpty(update.Title) ? update.Title : check.Title;
                check.Descript = !string.IsNullOrEmpty(update.Descript) ? update.Descript : check.Descript;
                check.Schedule = !string.IsNullOrEmpty(update.Schedule) ? update.Schedule : check.Schedule;
                check.Quantity = update.Quantity != null ? update.Quantity : check.Quantity;
                check.Price = update.Price != null ? update.Price : check.Price;
                check.Level = !string.IsNullOrEmpty(update.Level) ? update.Level : check.Level;
                check.AccountId = update.AccountId != null ? update.AccountId : check.AccountId;
                check.StoreId = update.StoreId != null ? update.StoreId : check.StoreId;
                check.Status = !string.IsNullOrEmpty(update.Status) ? update.Status : check.Status;
                check.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại.";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> DeleteCourse(Guid Id)
        {
            try
            {
                var check = await _unitOfWork.CourseRepo.GetByIdAsync(Id);
                if (check == null)
                {
                    return "Không tìm thấy khoá học này.";
                }
                _unitOfWork.CourseRepo.Delete(check);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại.";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> UpdateNoOfUser(Guid Id, int num)
        {
            try
            {
                var check = await _unitOfWork.CourseRepo.GetByIdAsync(Id);
                if (check == null)
                {
                    return "Không tìm thấy khoá học này.";
                }

                if (num <= 0 || num == null)
                {
                    return "Số lượng đăng ký không hợp lệ!";
                }

                if (check.NoOfUser >= check.Quantity)
                {
                    return "Đã vượt quá số người đăng kí!";
                }

                check.NoOfUser += num;
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại.";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> UpdateMinusNoOfUser(Guid Id, int num)
        {
            try
            {
                var check = await _unitOfWork.CourseRepo.GetByIdAsync(Id);
                if (check == null)
                {
                    return "Không tìm thấy khoá học này.";
                }

                if (num <= 0 || num == null)
                {
                    return "Số lượng đăng ký không hợp lệ!";
                }

                if (check.NoOfUser <= 0)
                {
                    return "Không có học viên nào!";
                }

                check.NoOfUser -= num;
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại.";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GetAllCoursesDTO?> GetCourseById(Guid Id)
        {         
            return _mapper.Map<GetAllCoursesDTO>(await _unitOfWork.CourseRepo.GetCourseByID(Id));
        }
    }
}
