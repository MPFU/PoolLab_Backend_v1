using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel.Helper
{
    public class PageResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItem { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
    }
}
