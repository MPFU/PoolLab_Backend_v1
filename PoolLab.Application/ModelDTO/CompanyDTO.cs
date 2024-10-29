using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class CompanyDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? CompanyImg { get; set; }

        public string? Descript { get; set; }

        public string? Status { get; set; }
    }

    public class CreateCompanyDTO
    {
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? CompanyImg { get; set; }

        public string? Descript { get; set; }
    }
}
