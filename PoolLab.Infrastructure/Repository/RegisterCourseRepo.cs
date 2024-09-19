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
    }
}
