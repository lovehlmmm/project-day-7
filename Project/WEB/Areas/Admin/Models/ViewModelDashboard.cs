using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Areas.Admin.Models
{
    public class ViewModelDashboard
    {
        public int OrderCount { get; set; }
        public int PreviousOrder { get; set; }
        public double PercentOrder { get; set; }
        public double PercentTotal { get; set; }
        public decimal TotalPrevious { get; set; }
        public decimal Total { get; set; }
        public int ImageOfDay  { get; set; }
        public int ImageOfMonth { get; set; }
        public double PercentImageOfDay { get; set; }
        public double PercentImageOfMonth { get; set; }
    }
}