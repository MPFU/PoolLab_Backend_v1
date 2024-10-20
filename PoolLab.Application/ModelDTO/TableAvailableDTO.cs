using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class TableAvailableDTO
    {
        public Guid Id { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? RecurringBookingId { get; set; }

        public Guid? BookingId { get; set; }

        public DateOnly? Date { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public bool? IsAvailable { get; set; }
    }

    public class NewTableAvailableDTO
    {
        public Guid? BilliardTableId { get; set; }

        public Guid? RecurringBookingId { get; set; }

        public Guid? BookingId { get; set; }

        public DateOnly? Date { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }
    }
}
