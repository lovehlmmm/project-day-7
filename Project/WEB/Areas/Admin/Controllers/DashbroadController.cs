using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Services;
using Entities;
using Constants;

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
            ViewBag.Orders = _orderService.FindAll(o => o.Status.Equals(OrderStatus.Paid));
            return View();
        }
    }
}