using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.FilterModel;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface ICourseService 
    {
        Task<PageResult<GetAllCoursesDTO>> GetAllCourses(CourseFilter courseFilter);
        Task<string?> CreateCourse(CreateCourseDTO create);
        Task<string?> UpdateCourse(Guid id, UpdateCourseDTO update);
        Task<string?> DeleteCourse(Guid Id);
    }
}
