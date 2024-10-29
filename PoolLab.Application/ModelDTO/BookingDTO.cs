using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class BookingDTO
    {
        public Guid Id { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? ConfigId { get; set; }

        public string? Message { get; set; }

        public DateOnly? BookingDate { get; set; }

        public TimeOnly? TimeStart { get; set; }

        public TimeOnly? TimeEnd { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string? DayOfWeek { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }

    public class NewBookingDTO
    {
        public Guid? CustomerId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? AreaId { get; set; }

        public string? Message { get; set; }

        public string? BookingDate { get; set; }

        public string? TimeStart { get; set; }

        public string? TimeEnd { get; set; }
    }

    public class GetBookingDTO
    {
        public Guid Id { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? ConfigId { get; set; }

        public Guid? AreaId { get; set; }

        public string? Message { get; set; }

        public string? Username { get; set; }

        public string? TableName { get; set; }        

        public DateOnly? BookingDate { get; set; }

        public TimeOnly? TimeStart { get; set; }

        public TimeOnly? TimeEnd { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string? DayOfWeek { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }
}
