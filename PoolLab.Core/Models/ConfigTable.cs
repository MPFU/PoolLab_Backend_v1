using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Models
{
    public partial class ConfigTable
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public int? TimeAllowBook { get; set; }

        public int? TimeDelay { get; set; }

        public int? TimeHold { get; set; }

        public int? TimeCancelBook { get; set; }

        public int? Deposit { get; set; }

        public int? MonthLimit { get; set; }

        public int? DayLimit { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string? Status { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
