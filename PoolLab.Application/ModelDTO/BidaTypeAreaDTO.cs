using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class BidaTypeAreaDTO
    {
        public Guid Id { get; set; }

        public Guid? BilliardTypeID { get; set; }

        public Guid? AreaID { get; set; }
    }

    public class NewTypeAreaDTO
    {
        public Guid? BilliardTypeID { get; set; }

        public Guid? AreaID { get; set; }

        public Guid? StoreID { get; set; }

        public NewTypeAreaDTO()
        {
            
        }
    }

    public class GetBidaTypeAreaDTO
    {
        public Guid Id { get; set; }

        public Guid? BilliardTypeID { get; set; }

        public Guid? AreaID { get; set; }

        public Guid? StoreID { get; set; }

        public string? TypeName { get; set; }

        public string? AreaName { get; set; }
    }

    public class BidaTypeAreFilter
    {
        public Guid? Id { get; set; }

        public Guid? BilliardTypeID { get; set; }

        public Guid? AreaID { get; set; }

        public Guid? StoreID { get; set; }
    }
}
