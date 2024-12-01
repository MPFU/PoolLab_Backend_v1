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
    public interface IRegisterCourseService 
    {
        Task<PageResult<GetAllRegisteredCourseDTO>> GetAllRegisteredCourses(RegisteredCourseFilter registeredCourseFilter);
        Task<string?> CreateRegisteredCourse(CreateRegisteredCourseDTO create);
    }
}
