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

    public class GetBilliardTableDTO
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

        public string? StoreName { get; set; }

        public string? Address { get; set; }

        public string? BilliardTypeName { get; set; }

        public string? AreaName { get; set; }

        public decimal? BidaPrice { get; set; }
    }

    public class UpdateInfoTableDTO
    {
        public string? Name { get; set; }

        public string? Descript { get; set; }

        public string? Image { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public Guid? PriceId { get; set; }
    }

    public class UpdateStatusTableDTO
    {
        public string? Status { get; set; }
    }

    public class ActiveTable
    {
        public Guid BilliardTableID { get; set; }
        public Guid CustomerID { get; set; }
        public Guid? AccountVoucherID { get; set; }
        public string? CustomerTime { get; set; }
    }

    public class GetByQRCode
    {
        public Guid BilliardTableID { get; set; }
        public Guid CustomerID { get; set; }
    }

    public class ActiveTableForGuest
    {
        public Guid? StoreId { get; set; }
        public string? StaffName { get; set; }
    }

    public class GetAllTableDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Descript { get; set; }

        public string? Image { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public Guid? PriceId { get; set; }

        public string? Qrcode { get; set; }

        public string? BidaTypeName { get; set; }

        public string? AreaName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }

    public class SearchTableRecurringDTO
    {
        public Guid? StoreId { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public List<string> RecurrenceDays { get; set; } = new List<string>();

        public string MonthBooking { get; set; }
    }

    public class SearchTableBookingDTO
    {
        public Guid? StoreId { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string BookingDate { get; set; }
    }
}
