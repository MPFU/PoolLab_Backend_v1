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

            if (!string.IsNullOrEmpty(courseFilter.Status))
                result = result.Where(x => x.Status.Contains(courseFilter.Status, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(courseFilter.AccountName))
                result = result.Where(x => x.AccountName.Contains(courseFilter.AccountName, StringComparison.OrdinalIgnoreCase));

            if (courseFilter.AccountId != null)
                result = result.Where(x => x.AccountId == courseFilter.AccountId);

            if (courseFilter.StoreId != null)
                result = result.Where(x => x.StoreId == courseFilter.StoreId);

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

                DateOnly currentDate = DateOnly.FromDateTime(a);

                //THỜI GIAN ĐẶT THEO THÁNG
                TimeOnly timeStart = TimeOnly.Parse(create.StartTime);
                TimeOnly timeEnd = TimeOnly.Parse(create.EndTime);

                if (timeStart >= timeEnd)
                {
                    return "Giờ chơi bắt đầu và kết thúc không hợp lệ!";
                }

                //NGÀY ĐẶT THEO THÁNG
                var parts = create.CourseMonth.Split('/');
                int month = int.Parse(parts[0]);
                int year = int.Parse(parts[1]);

                DateOnly dateStart = new DateOnly(year, month, 1);

                DateOnly dateEnd = new DateOnly(year, month, DateTime.DaysInMonth(year, month));

                if (currentDate > dateStart)
                {
                    return "Khoá học chỉ có thể bắt đầu vào tháng tiếp theo!";
                }

                if(currentDate == dateEnd)
                {
                    var currentTime = TimeOnly.FromDateTime(a);

                    if (currentTime >= timeStart)
                    {
                        return "Thời gian bắt đầu của khoá học đã muộn hơn thời điểm hiện tại để bắt đầu trong tháng này!";
                    }
                }

                if (create.Quantity == null || create.Quantity <= 0)
                {
                    return "Số lượng khoá học không hợp lệ!";
                }

                if (create.Price == null || create.Price <= 0)
                {
                    return "Học phí khoá học không hợp lệ!";
                }

                CourseDTO course = new CourseDTO();
                course.Id = Guid.NewGuid();
                course.NoOfUser = 0;
                course.CreatedDate = a;
                course.Status = "Kích Hoạt";
                course.StartDate = dateStart;
                course.EndDate = dateEnd;
                course.StartTime = timeStart;
                course.EndTime = timeEnd;
                course.Title = create.Title;
                course.AccountId = create.AccountId;
                course.StoreId = create.StoreId;
                course.Price = create.Price;
                course.Quantity = create.Quantity;
                course.Descript = create.Descript;
                course.Level = create.Level;
                course.Schedule = string.Join(",", create.Schedule);
                await _unitOfWork.CourseRepo.AddAsync(_mapper.Map<Course>(course));
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

                check.NoOfUser += num;

                if(check.NoOfUser == check.Quantity)
                {
                    check.Status = "Vô Hiệu";
                }

                _unitOfWork.CourseRepo.Update(check);
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
                DateTime now = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var a = TimeZoneInfo.ConvertTimeFromUtc(now, localTimeZone);
                var currentDate = DateOnly.FromDateTime(a);

                var check = await _unitOfWork.CourseRepo.GetByIdAsync(Id);
                if (check == null)
                {
                    return "Không tìm thấy khoá học này.";
                }

                if (num <= 0 || num == null)
                {
                    return "Số lượng đăng ký không hợp lệ!";
                }

                check.NoOfUser -= num;

                if (check.NoOfUser < 0)
                {
                    check.NoOfUser = 0;
                }

                if(check.StartDate.Value.AddDays(-1) >= currentDate)
                {
                    if(check.Status.Equals("Vô Hiệu"))
                    {
                        check.Status = "Kích Hoạt";
                    }
                }

                _unitOfWork.CourseRepo.Update(check);
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

        public async Task<string?> CancelCourse(Guid Id)
        {
            try
            {
                var course = await _unitOfWork.CourseRepo.GetCourseByID(Id);
                if (course == null)
                {
                    return "Không tìm thấy khoá học này!";
                }

                await _unitOfWork.BeginTransactionAsync();

                var register = await _unitOfWork.RegisterCourseRepo.GetRegisterCourseByCourseorStudentID(course.Id);
                if (register != null)
                {
                    foreach (var regis in register)
                    {
                        var enrollList = await _unitOfWork.RegisterCourseRepo.GetAllRegisterCourseByEnrollID(regis.Id);

                        if (enrollList == null)
                        {
                            return "Không tìm thấy danh sách học của khoá học này!";
                        }

                        //Xoá danh sách khoá học
                        foreach (var en in enrollList)
                        {
                            _unitOfWork.RegisterCourseRepo.Delete(en);
                            var result2 = await _unitOfWork.SaveAsync() > 0;
                        }

                        regis.Status = "Đã Huỷ";
                        
                        _unitOfWork.RegisterCourseRepo.Update(regis);
                        var result1 = await _unitOfWork.SaveAsync() > 0;
                    }
                }

                course.Status = "Vô Hiệu";
                course.NoOfUser = 0;
                _unitOfWork.CourseRepo.Update(course);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Huỷ Khoá Học Thất Bại!";
                }

                await _unitOfWork.CommitTransactionAsync();

                return null;
            }catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }
    }
}
