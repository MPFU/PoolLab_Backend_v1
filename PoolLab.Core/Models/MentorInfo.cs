using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Models
{
    public partial class MentorInfo
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? MentorImg { get; set; }

        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public decimal? Salary { get; set; }

        public string? PaymentImg { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get;set; }

        public string? Status { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
