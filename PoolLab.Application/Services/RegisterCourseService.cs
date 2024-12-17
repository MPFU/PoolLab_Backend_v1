using AutoMapper;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.FilterModel;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PoolLab.Application.Interface
{
    public class RegisterCourseService : IRegisterCourseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICourseService _courseService;

        public RegisterCourseService(IMapper mapper, IUnitOfWork unitOfWork, ICourseService courseService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _courseService = courseService;
        }

        public async Task<PageResult<GetAllRegisteredCourseDTO>> GetAllRegisteredCourses(RegisteredCourseFilter registeredCourseFilter)
        {
            var registeredCourseList = _mapper.Map<IEnumerable<GetAllRegisteredCourseDTO>>(await _unitOfWork.RegisterCourseRepo.GetAllRegisteredCourses());
            IQueryable<GetAllRegisteredCourseDTO> result = registeredCourseList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(registeredCourseFilter.Username))
                result = result.Where(x => x.Username.Contains(registeredCourseFilter.Username, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(registeredCourseFilter.Fullname))
                result = result.Where(x => x.Fullname.Contains(registeredCourseFilter.Fullname, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(registeredCourseFilter.Title))
                result = result.Where(x => x.Title.Contains(registeredCourseFilter.Title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(registeredCourseFilter.MentorName))
                result = result.Where(x => x.MentorName.Contains(registeredCourseFilter.MentorName, StringComparison.OrdinalIgnoreCase));

            if (registeredCourseFilter.IsEnroll != null)
                result = result.Where(x => x.IsRegistered == registeredCourseFilter.IsEnroll);

            if (registeredCourseFilter.StudentId != null)
                result = result.Where(x => x.StudentId == registeredCourseFilter.StudentId);

            if (registeredCourseFilter.StoreId != null)
                result = result.Where(x => x.StoreId == registeredCourseFilter.StoreId);

            if (registeredCourseFilter.CourseId != null)
                result = result.Where(x => x.CourseId == registeredCourseFilter.CourseId);

            if (registeredCourseFilter.EnrollID != null)
                result = result.Where(x => x.EnrollCourseId == registeredCourseFilter.EnrollID);

            if (!string.IsNullOrEmpty(registeredCourseFilter.Status))
                result = result.Where(x => x.Status.Contains(registeredCourseFilter.Status, StringComparison.OrdinalIgnoreCase));

            //Sorting
            if (!string.IsNullOrEmpty(registeredCourseFilter.SortBy))
            {
                switch (registeredCourseFilter.SortBy)
                {
                    case "createdDate":
                        result = registeredCourseFilter.SortAscending ?
                            result.OrderBy(x => x.CreatedDate) :
                            result.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "updatedDate":
                        result = registeredCourseFilter.SortAscending ?
                            result.OrderBy(x => x.UpdatedDate) :
                            result.OrderByDescending(x => x.UpdatedDate);
                        break;
                    case "price":
                        result = registeredCourseFilter.SortAscending ?
                            result.OrderBy(x => x.Price) :
                            result.OrderByDescending(x => x.Price);
                        break;
                }
            }

            //Paging
            var pageItems = result
                .Skip((registeredCourseFilter.PageNumber - 1) * registeredCourseFilter.PageSize)
                .Take(registeredCourseFilter.PageSize)
                .ToList();

            return new PageResult<GetAllRegisteredCourseDTO>
            {
                Items = pageItems,
                PageNumber = registeredCourseFilter.PageNumber,
                PageSize = registeredCourseFilter.PageSize,
                TotalItem = result.Count(),
                TotalPages = (int)Math.Ceiling((decimal)result.Count() / (decimal)registeredCourseFilter.PageSize)
            };
        }


        public async Task<string?> CreateRegisteredCourse(CreateRegisteredCourseDTO create)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var a = TimeZoneInfo.ConvertTimeFromUtc(now, localTimeZone);
                //var date = DateOnly.FromDateTime(a);

                var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)create.StudentId);
                if (cus == null)
                {
                    return "Không tìm thấy học viên này";
                }

                //if (cus.FullName == null || cus.PhoneNumber == null)
                //{
                //    return "Hãy thêm đầy đủ họ tên và số điện thoại để tiện liên lạc!";
                //}

                var course = await _unitOfWork.CourseRepo.GetByIdAsync((Guid)create.CourseId);

                if (course == null)
                {
                    return "Không tìm thấy khoá học này";
                }

                if (!course.Status.Equals("Kích Hoạt") || course.Quantity == course.NoOfUser)
                {
                    return "Khoá học này không còn khả dụng để đăng ký!";
                }

                if(cus.Id == course.AccountId)
                {
                    return "Người hướng dẫn không thể đăng ký khoá của chính mình!";
                }

                DateTime startDate = course.StartDate.Value.ToDateTime(TimeOnly.MinValue);

                DateTime endDate = course.EndDate.Value.ToDateTime(TimeOnly.MinValue);

                var dayCourse = course.Schedule.Split(",");
                var dayCourses = dayCourse.Select(day => Enum.Parse<DayOfWeek>(day)).ToList();

                //Kiểm tra các khoá học của khách
                var enroll = await _unitOfWork.RegisterCourseRepo.GetAllRegisterCourseCus(cus.Id, startDate, endDate);
                if (enroll != null)
                {
                    foreach (var roll in enroll)
                    {
                        var dayStudy = roll.Schedule.Split(",");
                        var dayStudys = dayStudy.Select(day => Enum.Parse<DayOfWeek>(day)).ToList();
                        foreach (var day in dayCourses)
                        {
                            foreach (var day1 in dayCourses)
                            {
                                if (day == day1)
                                {
                                    if ((roll.StartTime < course.EndTime && roll.StartTime >= course.StartTime) || (roll.EndTime > course.StartTime && roll.EndTime <= course.EndTime))
                                    {
                                        return $"Bạn đã có lịch học vào lúc {roll.StartTime} và kết thúc lúc {roll.EndTime}!";
                                    }
                                }
                            }
                        }
                    }
                }

                await _unitOfWork.BeginTransactionAsync();

                RegisteredCourseDTO registeredCourseDTO = new RegisteredCourseDTO();
                registeredCourseDTO.Id = Guid.NewGuid();
                registeredCourseDTO.StudentId = cus.Id;
                registeredCourseDTO.CourseId = course.Id;
                registeredCourseDTO.StoreId = course.StoreId;
                registeredCourseDTO.Price = course.Price;
                registeredCourseDTO.Schedule = course.Schedule;
                registeredCourseDTO.StartDate = startDate;
                registeredCourseDTO.EndDate = endDate;
                registeredCourseDTO.StartTime = course.StartTime;
                registeredCourseDTO.EndTime = course.EndTime;
                registeredCourseDTO.CreatedDate = a;
                registeredCourseDTO.IsRegistered = true;
                registeredCourseDTO.Status = "Kích Hoạt";

                await _unitOfWork.RegisterCourseRepo.AddAsync(_mapper.Map<RegisteredCourse>(registeredCourseDTO));
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo mới đăng kí khoá học thất bại!";
                }

                //Đếm tổng số buổi
                int count = 0;
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (dayCourses.Contains(date.DayOfWeek))
                    {
                        count++;
                    }
                }

                foreach (var day in dayCourses)
                {
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        if (date.DayOfWeek == day)
                        {
                            CreateSingleRegisterCourseDTO registeredDTO = new CreateSingleRegisterCourseDTO();
                            registeredDTO.Id = Guid.NewGuid();
                            registeredDTO.StudentId = cus.Id;
                            registeredDTO.CourseId = course.Id;
                            registeredDTO.StoreId = course.StoreId;
                            var price = course.Price / count;
                            registeredDTO.Price = Math.Round((decimal)price, 0, MidpointRounding.AwayFromZero);
                            registeredDTO.CourseDate = DateOnly.FromDateTime(date);
                            registeredDTO.StartTime = course.StartTime;
                            registeredDTO.EndTime = course.EndTime;
                            registeredDTO.CreatedDate = a;
                            registeredDTO.IsRegistered = false;
                            registeredDTO.EnrollCourseId = registeredCourseDTO.Id;
                            registeredDTO.Status = "Kích Hoạt";
                            await _unitOfWork.RegisterCourseRepo.AddAsync(_mapper.Map<RegisteredCourse>(registeredDTO));
                            var result1 = await _unitOfWork.SaveAsync() > 0;
                            if (!result1)
                            {
                                return "Tạo mới đăng kí khoá học thất bại!";
                            }
                        }
                    }
                }

                //Cập nhật số lượng học viên đăng ký
                var upNo = await _courseService.UpdateNoOfUser((Guid)registeredCourseDTO.CourseId, 1);
                if (upNo != null)
                {
                    return upNo;
                }

                await _unitOfWork.CommitTransactionAsync();
                return null;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> CancelRegisteredCourse(Guid id)
        {
            try
            {
                var enroll = await _unitOfWork.RegisterCourseRepo.GetByIdAsync(id);

                if (enroll == null)
                {
                    return "Không tìm thấy lịch đăng ký khoá học này!";
                }

                var enrollList = await _unitOfWork.RegisterCourseRepo.GetAllRegisterCourseByEnrollID(enroll.Id);

                if (enrollList == null)
                {
                    return "Không tìm thấy danh sách học của khoá học này!";
                }

                await _unitOfWork.BeginTransactionAsync();

                //Xoá danh sách khoá học
                foreach(var en in enrollList)
                {
                    _unitOfWork.RegisterCourseRepo.Delete(en);
                    var result1 = await _unitOfWork.SaveAsync() > 0;
                }

                enroll.Status = "Đã Huỷ";

                _unitOfWork.RegisterCourseRepo.Update(enroll);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại.";
                }

                //Cập nhật số lượng học viên course
                var downNo = await _courseService.UpdateMinusNoOfUser((Guid)enroll.CourseId, 1);
                if (downNo != null)
                {
                    return downNo;
                }

                await _unitOfWork.CommitTransactionAsync();
                return null;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }


        public async Task<string?> UpdateRegisteredCourse(Guid id, UpdateRegisteredCourseDTO update)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

                var check = await _unitOfWork.RegisterCourseRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy khoá học này.";
                }

                check.Status = !string.IsNullOrEmpty(update.Status) ? update.Status : check.Status;
                check.CourseId = update.CourseId != null ? update.CourseId : check.CourseId;
                check.StudentId = update.StudentId != null ? update.StudentId : check.StudentId;
                check.StoreId = update.StoreId != null ? update.StoreId : check.StoreId;
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

        public async Task<string?> DeleteRegisteredCourse(Guid id)
        {
            try
            {
                var check = await _unitOfWork.RegisterCourseRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy đăng kí khoá học này.";
                }
                _unitOfWork.RegisterCourseRepo.Delete(check);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại.";
                }
                var downNo = await _courseService.UpdateMinusNoOfUser((Guid)check.CourseId, 1);
                if (downNo != null)
                {
                    return downNo;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GetEnrollDTO?> GetRegisterdCourseById(Guid id)
        {
           return _mapper.Map<GetEnrollDTO>(await _unitOfWork.RegisterCourseRepo.GetRegisterdCourseById(id));
        }
    }
}
