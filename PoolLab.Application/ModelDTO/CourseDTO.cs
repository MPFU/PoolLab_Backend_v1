﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class CourseDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Descript { get; set; }

        public decimal? Price { get; set; }

        public string? Schedule { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public string? Level { get; set; }

        public int? Quantity { get; set; }

        public int? NoOfUser { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? AccountId { get; set; }

        public Guid? MentorId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }

    public class CreateCourseDTO
    {
        public string? Title { get; set; }

        public string? Descript { get; set; }

        public decimal? Price { get; set; }

        public List<string> Schedule { get; set; } = new List<string>();

        public string? CourseMonth { get; set; }

        public string? StartTime { get; set; }

        public string? EndTime { get; set; }

        public string? Level { get; set; }

        public int? Quantity { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? AccountId { get; set; }
    }

    public class UpdateCourseDTO
    {
        public string? Title { get; set; }

        public string? Descript { get; set; }

        public decimal? Price { get; set; }

        public string? Schedule { get; set; }

        public string? StartDate { get; set; }

        public string? Level { get; set; }

        public int? Quantity { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? AccountId { get; set; }

        public string? Status { get; set; }
    }

    public class GetAllCoursesDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Descript { get; set; }

        public decimal? Price { get; set; }

        public string? Schedule { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public string? Level { get; set; }

        public int? Quantity { get; set; }

        public int? NoOfUser { get; set; }

        public Guid? StoreId { get; set; }

        public string? StoreName { get; set; }

        public string? Address { get; set; }

        public Guid? AccountId { get; set; }

        public string? AccountName { get; set; }

        public string? AccountAvatar {  get; set; }

        public Guid? MentorId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }
}
