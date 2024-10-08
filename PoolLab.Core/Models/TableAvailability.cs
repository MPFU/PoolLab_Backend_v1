using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Models
{
    public partial class TableAvailability
    {
        public Guid Id { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? RecurringBookingId { get; set; }

        public Guid? BookingId { get; set; }

        public DateOnly? Date { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public bool? IsAvailable { get; set; }

        public virtual BilliardTable? BilliardTable { get; set; }

        public virtual RecurringBookings? RecurringBooking { get; set; }

        public virtual Booking? Booking { get; set;}
    }
}
