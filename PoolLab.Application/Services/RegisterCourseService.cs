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

namespace PoolLab.Application.Interface
{
    public class RegisterCourseService : IRegisterCourseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCourseService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageResult<GetAllRegisteredCourseDTO>> GetAllRegisteredCourses(RegisteredCourseFilter registeredCourseFilter)
        {
            var registeredCourseList = _mapper.Map<IEnumerable<GetAllRegisteredCourseDTO>>(await _unitOfWork.RegisterCourseRepo.GetAllRegisteredCourses());
            IQueryable<GetAllRegisteredCourseDTO> result = registeredCourseList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(registeredCourseFilter.Status))
                result = result.Where(x => x.Status.Contains(registeredCourseFilter.Status, StringComparison.OrdinalIgnoreCase));

            if (registeredCourseFilter.StudentId != null)
                result = result.Where(x => x.StudentId == registeredCourseFilter.StudentId);

            if (registeredCourseFilter.StoreId != null)
                result = result.Where(x => x.StoreId == registeredCourseFilter.StoreId);

            if (registeredCourseFilter.CourseId != null)
                result = result.Where(x => x.CourseId == registeredCourseFilter.CourseId);


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
                    case "status":
                        result = registeredCourseFilter.SortAscending ?
                            result.OrderBy(x => x.Status) :
                            result.OrderByDescending(x => x.Status);
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


                var check = _mapper.Map<RegisteredCourse>(create);

                check.Id = Guid.NewGuid();
                check.Status = "Đang hoạt động";
                check.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(now, localTimeZone);       

                await _unitOfWork.RegisterCourseRepo.AddAsync(check);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo mới đăng kí khoá học thất bại!";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
