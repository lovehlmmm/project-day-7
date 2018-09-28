﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Areas.Admin.Models
{
    public class ViewModelDashboard
    {
        public List<Order> OrderPending { get; set; }
        public int OrderCount { get; set; }
        public int PreviousOrder { get; set; }
        public decimal TotalPrevious { get; set; }
        public decimal Total { get; set; }
        public int ImageOfDay  { get; set; }
        public int ImageOfMonth { get; set; }
        public float PercentImageOfDay { get; set; }
        public float PercentImageOfMonth { get; set; }
    }
}