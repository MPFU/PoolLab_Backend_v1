﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class PlaytimeDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public Guid? BilliardTableId { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        public decimal? TotalTime { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? Status { get; set; }
    }

    public class AddNewPlayTimeDTO
    {
        public Guid? BilliardTableId { get; set; }

        public decimal? TotaLTime { get; set; }

        public decimal? TotaLPrice { get; set; }

        public string? Status { get; set; }
    }

    public class StopTimeDTO
    {
        public Guid BilliardTableID { get; set; }
        public Guid? CustomerID { get; set; }
        public string? CustomerTime { get; set; }
    }
}
