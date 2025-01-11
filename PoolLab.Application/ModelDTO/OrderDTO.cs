using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }

        public string? OrderCode { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? StoreId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? Discount { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? PaymentMethod { get; set; }

        public string? Status { get; set; }
    }

    public class AddNewOrderDTO
    {
        public Guid? CustomerId { get; set; }

        public string? Username { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? PlayTimeId { get; set; }

        public string? OrderBy {  get; set; }

        public int? Discount { get; set; }

        public string? PaymentMethod { get; set; }
    }

    public class GetAllOrderDTO
    {
        public Guid Id { get; set; }

        public string? OrderCode { get; set; }

        public Guid? CustomerId { get; set; }

        public string? Username { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? StoreId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? Discount { get; set; }

        public decimal? TotalPrice { get; set; }

        public decimal? FinalPrice { get; set; }

        public string? OrderBy { get; set; }

        public string? PaymentMethod { get; set; }

        public string? Status { get; set; }
    }

    public class CreateOrderBill 
    {
        public string? TimePlay {  get; set; }

        public Guid? BilliardTableId { get; set;}
    }

    public class GetOrderBillDTO
    {
        public Guid Id { get; set; }

        public string? OrderCode { get; set; }

        public string? Username { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? StoreId { get; set; }

        public string? StoreName { get; set; }

        public string? Address { get; set; }

        public PlaytimeDTO? PlayTime { get; set; }

        public List<OrderDetailDTO>? OrderDetails { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? OrderBy { get; set; }

        public decimal? TotalPrice { get; set; }

        public decimal? Discount { get; set; }

        public decimal? FinalPrice { get; set; }

        public decimal? CustomerPay { get; set; }

        public decimal? ExcessCash { get; set; }

        public decimal? AdditionalFee { get; set; }

        public string? PaymentMethod { get; set; }

        public string? Status { get; set; }
    }

    public class UpdateCusPayDTO
    {
        public decimal? Discount { get; set; }

        public decimal? TotalPrice { get; set; }

        public decimal? CustomerPay { get; set; }

        public decimal? ExcessCash { get; set; }

        public decimal? FinalPrice { get; set; }

        public decimal? AdditionalFee { get; set; }

        public string? PaymentMethod { get; set; }

        public string? Status { get; set; }
    }
}
