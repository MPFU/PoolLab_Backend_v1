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
        Task<GetEnrollDTO> GetRegisterdCourseById(Guid id);
        Task<PageResult<GetAllRegisteredCourseDTO>> GetAllRegisteredCourses(RegisteredCourseFilter registeredCourseFilter);
        Task<string?> CreateRegisteredCourse(CreateRegisteredCourseDTO create);
        Task<string?> CancelRegisteredCourse(Guid id);
        Task<string?> UpdateRegisteredCourse(Guid id, UpdateRegisteredCourseDTO update);
        Task<string?> DeleteRegisteredCourse(Guid id);
    }
}
