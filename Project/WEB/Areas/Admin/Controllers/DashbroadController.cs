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
            var today = DateTime.Now.Date;
            var privious = DateTime.Now.AddDays(-1).Date;
            var orderPrevious = _orderService.FindAll(o=>o.CreatedAt.Value.Month.Equals(previousMonth) & o.CreatedAt.Value.Year.Equals(previousYear)).ToList();
            var orderMonth = _orderService.FindAll(o => o.CreatedAt.Value.Month.Equals(month) & o.CreatedAt.Value.Year.Equals(year) & o.Status == OrderStatus.Received).ToList();  
            int imageOfDay = orderMonth.Where(o => o.CreatedAt.Value.Date == today).Sum(od => od.OrderDetails.Sum(o => o.Quantity.Value));
            int imageOfMonth = orderMonth.Sum(od => od.OrderDetails.Sum(o => o.Quantity.Value));
            int imageOfDayPrevious = orderMonth.Where(o => o.CreatedAt.Value.Date == privious).Sum(od => od.OrderDetails.Sum(o => o.Quantity.Value));
            int imageOfMonthPrevious = orderPrevious.Sum(od => od.OrderDetails.Sum(o => o.Quantity.Value));
            double percentImageOfDay = 0;
            decimal totalMonth = orderMonth.Sum(o => o.OrderDetails.Sum(od => od.Quantity * (od.Material.Price + od.Product.ProductPrice))).Value;
            decimal totalPrevious = orderPrevious.Sum(o => o.OrderDetails.Sum(od => od.Quantity * (od.Material.Price + od.Product.ProductPrice))).Value;
            double percentTotal = 0;
            double percentOrder = 0;
            if (orderPrevious.Count>0)
            {
                percentOrder = ((orderMonth.Count - orderPrevious.Count) / orderPrevious.Count) * 100;
            }
            else
            {
                percentOrder = (orderMonth.Count - orderPrevious.Count) * 100;
            }
            if (totalPrevious>0)
            {
                double percent = Decimal.ToInt32((totalMonth - totalPrevious) / totalPrevious);
                percentTotal = Math.Round(percent,2) * 100;
            }
            else
            {
                percentTotal = Decimal.ToInt32((totalMonth - totalPrevious)) * 100;
            }
            if (imageOfDayPrevious>0)
            {
                percentImageOfDay = (imageOfDay - imageOfDayPrevious) / imageOfDayPrevious * 100;
            }
            else
            {
                percentImageOfDay = (imageOfDay - imageOfDayPrevious)*100;
            }
            //Percent Day
            double percentImageOfMonth = 0;
            if (imageOfMonthPrevious > 0)
            {
                percentImageOfMonth = (imageOfMonth - imageOfMonthPrevious) / imageOfMonthPrevious * 100;
            }
            else
            {
               percentImageOfMonth=  (imageOfMonth - imageOfMonthPrevious) * 100;
            }


            ViewModelDashboard viewModel = new ViewModelDashboard()
            {
                OrderCount = orderMonth.Count,
                PreviousOrder = orderPrevious.Count,
                Total = orderMonth.Sum(o => o.OrderDetails.Sum(od => od.Quantity * (od.Product.ProductPrice + od.Material.Price.Value))).Value,
                TotalPrevious = orderPrevious.Sum(o => o.OrderDetails.Sum(od => od.Quantity * (od.Product.ProductPrice + od.Material.Price.Value))).Value,
                ImageOfDay = imageOfDay,
                ImageOfMonth = imageOfMonth,
                PercentImageOfDay = percentImageOfDay,
                PercentImageOfMonth = percentImageOfMonth,
                PercentTotal = percentTotal,
                PercentOrder = percentOrder
                
            };

            return View(viewModel);
        }
        public ActionResult GetOrderPending(int num,int page)
        {
            var orderPending = _orderService.GetAll(num, page, o => o.CreatedAt, o => o.Status == OrderStatus.Pending, o => o.CreatedAt).ToList();
            return PartialView("~/Areas/Admin/Views/Dashbroad/_OrderPending.cshtml", orderPending);
        }
    }
}