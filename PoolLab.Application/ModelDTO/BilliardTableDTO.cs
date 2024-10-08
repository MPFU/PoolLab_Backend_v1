using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class BilliardTableDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Descript { get; set; }

        public string? Image { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public string? Qrcode { get; set; }

        public Guid? PriceId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }

        public BilliardTableDTO()
        {
            
        }
    }

    public class NewBilliardTableDTO
    {
        public string? Name { get; set; }

        public string? Descript { get; set; }

        public string? Image { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public Guid? PriceId { get; set; }
    }
}
