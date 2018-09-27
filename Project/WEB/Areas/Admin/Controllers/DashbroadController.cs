using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Services;
using Entities;
using Constants;
using WEB.Areas.Admin.Models;

namespace WEB.Areas.Admin.Controllers
{
    [LoginAdminRequired]
    public class DashbroadController : Controller
    {
        public readonly IBaseService<Order> _orderService;
        public DashbroadController(IBaseService<Order> orderService)
        {
            _orderService = orderService;
        }
        // GET: Admin/Home        
        public ActionResult Index()
        {
            var previousMonth = DateTime.Now.AddMonths(-1).Month;
            var previousYear = DateTime.Now.AddMonths(-1).Year;
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var orderPrevious = _orderService.FindAll(o=>o.ModifiedAt.Value.Month.Equals(previousMonth) & o.ModifiedAt.Value.Year.Equals(previousYear)).ToList();
            var orderMonth = _orderService.FindAll(o => o.ModifiedAt.Value.Month.Equals(month) & o.ModifiedAt.Value.Year.Equals(year) & o.Status == OrderStatus.Received).ToList();
            var orderPending = _orderService.FindAll(o => o.Status == OrderStatus.Pending).ToList();
            ViewModelDashboard viewModel = new ViewModelDashboard()
            {
                OrderCount = orderMonth.Count,
                OrderPending = orderPending,
                PreviousOrder = orderPrevious.Count,
                Total = orderMonth.Sum(o => o.OrderDetails.Sum(od => od.Quantity * (od.Product.ProductPrice + od.Material.Price.Value))).Value,
                TotalPrevious  = orderPrevious.Sum(o => o.OrderDetails.Sum(od => od.Quantity * (od.Product.ProductPrice + od.Material.Price.Value))).Value
            };
            return View(viewModel);
        }
    }
}