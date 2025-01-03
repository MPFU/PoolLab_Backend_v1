﻿using System;
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

        public Guid? AreaId { get; set; }

        public Guid? RecurringId { get; set; }

        public string? Message { get; set; }

        public decimal? Deposit { get; set; }

        public DateOnly? BookingDate { get; set; }

        public TimeOnly? TimeStart { get; set; }

        public TimeOnly? TimeEnd { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string? DayOfWeek { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsRecurring { get; set; }

        public string? Status { get; set; }
    }

    public class NewBookingDTO
    {
        public Guid? CustomerId { get; set; }

        public Guid? BilliardTableId { get; set; }

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

        public decimal? Deposit { get; set; }

        public string? Username { get; set; }

        public string? TableName { get; set; }   
        
        public string? StoreName { get; set; }

        public string? Address { get; set; }

        public string? AreaName { get; set; }   

        public string? BidaTypeName { get; set; }

        public decimal? BidaPrice { get; set; }

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

    public class UpdateBookingStatusDTO
    {
        public string? Status { get; set; }
    }

    public class GetAllBookingDTO
    {
        public Guid Id { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? ConfigId { get; set; }

        public Guid? AreaId { get; set; }

        public string? Message { get; set; }

        public decimal? Deposit { get; set; }

        public string? Username { get; set; }

        public string? TableName { get; set; }

        public string? StoreName { get; set; }

        public string? Address { get; set; }

        public decimal? BidaPrice { get; set; }

        public DateOnly? BookingDate { get; set; }

        public TimeOnly? TimeStart { get; set; }

        public TimeOnly? TimeEnd { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string? DayOfWeek { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsRecurring { get; set; }

        public string? Status { get; set; }
    }

    public class AnswerBookingDTO
    {
        public string? Answer { get; set; } = null;
    }

    public class BookingReqDTO
    {
        public Guid TableId { get; set; }
        public Guid CustomerId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<string> RecurrenceDays { get; set; } = new List<string>();
        public string MonthBooking { get; set; }
    }

    public class NewBookingRecurrDTO
    {
        public Guid? CustomerId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? ConfigId { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? RecurringId { get; set; }

        public decimal? Deposit { get; set; }

        public DateOnly? BookingDate { get; set; }

        public TimeOnly? TimeStart { get; set; }

        public TimeOnly? TimeEnd { get; set; }
    }

    public class GetRecurringBookingDTO
    {
        public Guid Id { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? ConfigId { get; set; }

        public Guid? AreaId { get; set; }

        public string? Username { get; set; }

        public string? TableName { get; set; }

        public string? StoreName { get; set; }

        public string? Address { get; set; }

        public decimal? BidaPrice { get; set; }

        public string? Message { get; set; }

        public decimal? Deposit { get; set; }

        public TimeOnly? TimeStart { get; set; }

        public TimeOnly? TimeEnd { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public string? DayOfWeek { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsRecurring { get; set; }

        public string? Status { get; set; }

        public List<BookingDTO> RecurringBookings { get; set; }
    }
}
