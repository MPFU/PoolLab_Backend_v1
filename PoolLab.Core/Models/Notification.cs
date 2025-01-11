using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Models
{
    public partial class Notification
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }
        
        public string? Descript { get; set; }

        public Guid? CustomerID { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? ReadAt { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }

        public virtual Account? Customer {  get; set; }
    }
}
