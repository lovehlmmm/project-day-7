using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Constants;
using Entities;
using Helpers;
using Services;
using WEB.Hubs;

namespace WEB.Areas.Admin.Controllers
{
    [LoginAdminRequired]
    public class OrderController : Controller
    {
        private IBaseService<Order> _orderService;

        public OrderController(IBaseService<Order> orderService)
        {
            _orderService = orderService;
        }
        // GET: Admin/Order
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList()
        {
            try
            {
                var pageNumber = int.Parse(Request.QueryString["pageNumber"]);
                var pageSize = int.Parse(Request.QueryString["pageSize"]);
                var filter = Request.QueryString["filter"];
                var filterDate = Request.QueryString["date"];
                long search=0;
                long.TryParse(Request.QueryString["search"],out search);
               
                Expression<Func<Order, bool>> expression;

                if (filter=="")
                {
                    expression = (o) => o.Status != Status.Deleted;
                }
                else
                {
                    expression = (o) => o.Status == filter;
                }
                if (filterDate.Trim() != "")
                {
                    string[] date = Regex.Split(filterDate, "-");
                    DateTime from = Convert.ToDateTime(date[0]).Date;
                    DateTime to = Convert.ToDateTime(date[1]).Date;
                    expression =  expression.And(o => DbFunctions.TruncateTime(o.CreatedAt) >= from & DbFunctions.TruncateTime(o.CreatedAt) <= to);
                }
                var list = await _orderService.GetAllAsync(pageNumber, pageSize, o => o.CreatedAt,
                    expression);
                if (search > 0)
                {
                    expression = expression.And(o => o.OrderId == search);
                    list = _orderService.FindAll(expression);
                }
                var total = list.Count();
                var totalPage = (int)Math.Ceiling((double)(total / pageSize)) + 1;
                var list2 = list.Select(o => new
                {
                    o.OrderId,
                    o.Customer.CustomerName,
                    Total = (o.OrderDetails?.Sum(od =>  od?.Quantity * (od?.Product.ProductPrice+od?.Material.Price))),
                    o.Status,
                    o.PhoneNumber,
                    o.Address?.AddressDetails,
                    o.CreatedAt,
                    o.ModifiedAt,
                    o.FolderImage
                }).ToList();
                return Json(new { status = true, data = list2, totalPage = totalPage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        public  ActionResult Get(long id)
        {
            try
            {
                Order order =  _orderService.Find(o => o.OrderId == id && o.Status != Status.Deleted);
                if (order!=null)
                {
                    
                    ViewBag.Data = order;
                }
             
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return  PartialView("~/Areas/Admin/Views/Order/OrderDetailsPartial.cshtml");
        }
        public async Task<JsonResult> ChangeStatus(string mode, long id)
        {
            var message = "";
            if (mode==null|id<1)
            {
                return Json(new { status = false, message }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                Order order = _orderService.Find(o => o.OrderId == id && o.Status != Status.Deleted);
                if (order!=null)
                {
                    var check = GetOrderChange(mode, order);
                    if (check!=null)
                    {
                        var updated = await _orderService.UpdateAsync(check, check.OrderId);
                        NotificationHub hub = new NotificationHub();
                        hub.SendNotification("test9");
                        if (updated!=null)
                        {
                            return Json(new { status = true, message }, JsonRequestBehavior.AllowGet);
                        }
                       
                    }
                }
            }
            catch (Exception)
            {

            }
            return Json(new { status = false, message }, JsonRequestBehavior.AllowGet);
        }

        private Order GetOrderChange(string mode,Order order)
        {
            if (order == null|mode==null)
                return null;
            if (mode==OrderStatus.Confirmed & order.Status==OrderStatus.Pending)
            {
                order.Status = OrderStatus.Confirmed;
                return order;
            }
            if (mode == OrderStatus.Cancelled & order.Status == OrderStatus.Pending)
            {
                order.Status = OrderStatus.Cancelled;
                return order;
            }
            if (mode == OrderStatus.Received & order.Status == OrderStatus.Confirmed)
            {
                order.Status = OrderStatus.Received;
                return order;
            }
            if (mode == Status.Deleted & (order.Status == OrderStatus.Cancelled | order.Status == OrderStatus.Received))
            {
                order.Status = Status.Deleted;
                return order;
            }
            return null; 
        }
    }
}