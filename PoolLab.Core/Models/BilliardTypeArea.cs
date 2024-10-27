using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Models
{
    public partial class BilliardTypeArea
    {
        public Guid Id { get; set; }

        public Guid? BilliardTypeID { get; set; }

        public Guid? AreaID { get; set; }

        public Guid? StoreID { get; set; }

        public virtual Area? Area { get; set; }

        public virtual BilliardType? BilliardType { get; set; }

        public virtual Store? Store { get; set; }
    }
}
