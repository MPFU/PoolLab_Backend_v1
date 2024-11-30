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
    public class CourseRepo : GenericRepo<Course>, ICourseRepo
    {
        public CourseRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {

        }
        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _dbContext.Courses
                .Include(x => x.Account)
                .Include(x => x.Store)
                .Include(x => x.RegisteredCourses)
                .ToListAsync();
        }
    }
}
