using Microsoft.EntityFrameworkCore;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Interface
{
    public class RegisterCourseRepo : GenericRepo<RegisteredCourse>, IRegisterCourseRepo
    {
        public RegisterCourseRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<RegisteredCourse>> GetAllRegisteredCourses()
        {
            return await _dbContext.RegisteredCourses
                .Include(x => x.Course)
                .ThenInclude(y => y.Account)
                .Include(x => x.Store)
                .Include(x => x.Student)
                .ToListAsync();
        }

        public async Task<IEnumerable<RegisteredCourse>?> GetAllRegisterCourseCus(Guid id, DateTime startDate, DateTime endDate)
        {
            return await _dbContext.RegisteredCourses
                .Where(x => x.StudentId == id && x.IsRegistered == true)
                .Where(x => x.StartDate == startDate && x.EndDate == endDate)
                .Where(x => x.Status.Equals("Kích Hoạt"))
                .ToListAsync();
        }

        public async Task<IEnumerable<RegisteredCourse>?> GetAllRegisterCourseByEnrollID(Guid id)
        {
            return await _dbContext.RegisteredCourses.Where(x => x.EnrollCourseId == id && x.Status.Equals("Kích Hoạt")).ToListAsync();
        }

        public async Task<RegisteredCourse?> GetRegisterdCourseById(Guid id)
        {
            return await _dbContext.RegisteredCourses
                .Include(x => x.Course)
                .ThenInclude(y => y.Account)
                .Include(x => x.Store)
                .Include(x => x.Student)
                .Include(x => x.RegisteredCourses)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RegisteredCourse>?> GetRegisterCourseByCourseorStudentID(Guid id)
        {
            return await _dbContext.RegisteredCourses
                .Where(x => x.CourseId == id || x.StudentId == id)
                .Where(x => x.IsRegistered == true && x.Status.Equals("Kích Hoạt"))
                .ToListAsync();
        }
    }
}
