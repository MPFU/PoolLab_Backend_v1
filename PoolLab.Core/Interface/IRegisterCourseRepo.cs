using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IRegisterCourseRepo : IGenericRepo<RegisteredCourse>
    {
        Task<IEnumerable<RegisteredCourse>> GetAllRegisteredCourses();

        Task<IEnumerable<RegisteredCourse>?> GetAllRegisterCourseCus(Guid id, DateTime startDate, DateTime endDate);

        Task<IEnumerable<RegisteredCourse>?> GetAllRegisterCourseByEnrollID(Guid id);

        Task<RegisteredCourse?> GetRegisterdCourseById(Guid id);

        Task<IEnumerable<RegisteredCourse>?> GetRegisterCourseByCourseorStudentID(Guid id);
    }
}
