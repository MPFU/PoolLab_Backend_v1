using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel.Helper
{
    public class FilterOption<T>
    {
        // Pagination properties
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Sorting properties
        public string? SortBy { get; set; } // Property to sort by
        public bool SortAscending { get; set; } = true; // Sort direction
    }
}
