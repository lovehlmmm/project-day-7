using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
using WEB.Areas.Admin.Helpers;
using WEB.Areas.Admin.Models;
using WEB.Helpers;
using WEB.Hubs;

namespace WEB.Areas.Admin.Controllers
{
    [LoginAdminRequired]
    public class OrderController : Controller
    {
        private IBaseService<Order> _orderService;
        private IBaseService<Notification> _notiService;
        private IBaseService<User> _userService;
        private INotificationHub _notificationHub;
        public OrderController(IBaseService<Order> orderService, IBaseService<User> userService, IBaseService<Notification> notiService, INotificationHub notificationHub)
        {
            _notiService = notiService;
            _orderService = orderService;
            _userService = userService;
            _notificationHub = notificationHub;
        }
        // GET: Admin/Order
        public ActionResult Index()
        {
            ViewBag.FilterDate = null;
            ViewBag.Filter = null;
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
                long customerid = 0;
                var filterCustomer = Request.QueryString["customer"];
                long.TryParse(filterCustomer,out customerid);
                long search = 0;
                long.TryParse(Request.QueryString["search"], out search);

                Expression<Func<Order, bool>> expression;

                if (filter == "")
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
                    expression = expression.And(o => DbFunctions.TruncateTime(o.CreatedAt) >= from & DbFunctions.TruncateTime(o.CreatedAt) <= to);
                }

                if (customerid!=0)
                {
                    expression = expression.And(o => o.CustomerId.Value == customerid);
                }
                var list = await _orderService.GetAllAsync(pageNumber, pageSize, o => GetStatusOrder(o.Status),
                    expression);
                if (search > 0)
                {
                    expression = expression.And(o => o.OrderId == search);
                    list = _orderService.FindAll(expression);
                }
                var total = _orderService.FindAll(expression).Count();
                var totalPage = (int)Math.Ceiling((double)(total / pageSize)) + 1;
                var list2 = list.OrderByDescending(o=>o.CreatedAt).OrderBy(o => GetStatusOrder(o.Status)).Select(o => new
                {
                    o.OrderId,
                    o.Customer.CustomerName,
                    Total = (o.OrderDetails?.Sum(od => od?.Quantity * (od?.Product.ProductPrice + od?.Material.Price))).Value.ToString("0,0 VNĐ"),
                    o.Status,
                    o.PhoneNumber,
                    o.Address?.AddressDetails,
                    o.CreatedAt,
                    o.ModifiedAt,
                    o.FolderImage
                }).ToList();
                return Json(new { status = true, data = list2,  totalPage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get(long id)
        {
            try
            {
                Order order = _orderService.Find(o => o.OrderId == id && o.Status != Status.Deleted);
                if (order != null)
                {

                    ViewBag.Data = order;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return PartialView("~/Areas/Admin/Views/Order/OrderDetailsPartial.cshtml");
        }
        private int GetStatusOrder(string status)
        {
            int result = 0;
            switch (status)
            {
                case OrderStatus.Pending:
                    result= 1;
                    break;
                case OrderStatus.Confirmed:
                    result= 2;
                    break;
                case OrderStatus.Canceled:
                    result = 4;
                    break;
                case OrderStatus.Received:
                    result= 3;
                    break;
            }
            return result;
        }
        public async Task<JsonResult> ChangeStatus(string mode, long id,string reason = null)
        {
            var message = "";
            if (mode == null | id < 1)
            {
                return Json(new { status = false, message }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                Order order = _orderService.Find(o => o.OrderId == id && o.Status != Status.Deleted);
                if (order != null)
                {
                    var check = GetOrderChange(mode, order);
                    if (check != null)
                    {
                        check.ModifiedAt = DateTime.Now;
                        var updated = await _orderService.UpdateAsync(check, check.OrderId);
                        
                        if (updated != null)
                        {
                            
                            var user = await _userService.FindAsync(u => u.CustomerId == updated.CustomerId);
                            if (updated.Status == OrderStatus.Canceled|updated.Status==OrderStatus.Received)
                            {
                                string path = string.Format("~/Images/Upload/{0}", updated.FolderImage);
                                var fullPath = Server.MapPath(path);
                                if (System.IO.Directory.Exists(fullPath))
                                {
                                    System.IO.Directory.Delete(fullPath,true);
                                }
                            }
                            if (updated.Status == OrderStatus.Received)
                            {
                                var body = ViewToStringAdmin.RenderViewToString("Order", "MailReceivedOrder","admin", updated,this.Request.RequestContext);
                                await Task.Factory.StartNew((() =>
                                 {
                                     SendEmail.Send(user.Email, body, "Thanks for using our service");
                                 }));
                            }
                            if (updated.Status==OrderStatus.Canceled)
                            {
                                MailCancelModel model = new MailCancelModel()
                                {
                                    Order = updated,
                                    Reason = reason
                                };
                                var body = ViewToStringAdmin.RenderViewToString("Order", "MailCancelOrder", "admin", model, this.Request.RequestContext);
                                await Task.Factory.StartNew((() =>
                                {
                                    SendEmail.Send(user.Email, body, "Your order has been canceled");
                                }));
                            }
                            //NotificationHub hub = new NotificationHub();
                            var noti = GetNotifi(mode, updated, reason);
                                Notification notification = new Notification()
                            {
                                IsRead = false,
                                SendTo = user.Username,
                                IsReminder = false,
                                CreatedAt = DateTime.Now,
                                Status = Status.Active,
                                NotificationType = noti.Type,
                                Details = noti.Details,
                                Title = noti.Title,
                                SendFrom = UserRole.Admin,
                                Url = string.Format("details/{0}",updated.OrderId)
                            };
                            
                            var notiAdd = await _notiService.AddAsync(notification);
                            if (notiAdd!=null)
                            {
                               _notificationHub.SendNotification(user.Username,notiAdd);
                            }
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
        private Notifi GetNotifi(string mode,Order order,string reason=null)
        {
            Notifi notifi = null;
            switch (mode)
            {
                case OrderStatus.Confirmed:
                    notifi = new Notifi()
                    {
                        Title = "The order has been confirmed",
                        Details = string.Format("The order #{0} has been confirmed, we will contact you", order.OrderId),
                        Type = NotificationType.Success
                    };
                    break;
                case OrderStatus.Canceled:
                    notifi = new Notifi()
                    {
                        Title = "The order was canceled",
                        Details = string.Format("The order {0} was canceled due to: {1}",order.OrderId,reason),
                        Type = NotificationType.Warning
                    };
                    break;
                case OrderStatus.Received:
                    notifi = new Notifi()
                    {
                        Title = "Thank you!!",
                        Details = string.Format("Thanks for using our service"),
                        Type = NotificationType.Success
                    };
                    break;

            }
            return notifi;
        }

        private Order GetOrderChange(string mode, Order order)
        {
            if (order == null | mode == null)
                return null;
            if (mode == OrderStatus.Confirmed & order.Status == OrderStatus.Pending)
            {
                order.Status = OrderStatus.Confirmed;
                return order;
            }
            if (mode == OrderStatus.Canceled & order.Status == OrderStatus.Pending)
            {
                order.Status = OrderStatus.Canceled;
                return order;
            }
            if (mode == OrderStatus.Received & order.Status == OrderStatus.Confirmed)
            {
                order.Status = OrderStatus.Received;
                return order;
            }
            if (mode == Status.Deleted & (order.Status == OrderStatus.Canceled | order.Status == OrderStatus.Received))
            {
                order.Status = Status.Deleted;
                return order;
            }
            return null;
        }
        private class Notifi
        {
            public string Title { get; set; }
            public string Details { get; set; }
            public string Type { get; set; }
        }
    }

}