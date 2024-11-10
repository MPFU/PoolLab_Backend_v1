using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class ProductDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Descript { get; set; }

        public int? Quantity { get; set; }

        public int? MinQuantity { get; set; }

        public decimal? Price { get; set; }

        public string? ProductImg { get; set; }

        public Guid? ProductTypeId { get; set; }

        public Guid? ProductGroupId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? UnitId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }

    public class CreateProductDTO
    {
        public string? Name { get; set; }

        public string? Descript { get; set; }

        public int? Quantity { get; set; }

        public int? MinQuantity { get; set; }

        public decimal? Price { get; set; }

        public string? ProductImg { get; set; }

        public Guid? ProductTypeId { get; set; }

        public Guid? ProductGroupId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? UnitId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }
}
