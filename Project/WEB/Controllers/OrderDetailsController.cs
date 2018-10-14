using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constants;
using Entities;
using Helpers;
using Services;
namespace WEB.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBaseService<Order> _orderService;
        public OrderDetailsController(IUserService userService, IBaseService<Order> orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }
        // GET: OrderDetails
        public ActionResult Index(long? id)
        {

                UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;

                if (userSession != null)
                {
                    var user = _userService.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username)) as User;
                    if (user != null)
                    {
                        var order = user.Customer.Orders.SingleOrDefault(o => o.OrderId == id);
                        if (order != null)
                        {
                            ViewBag.Order = order;
                            return View();
                        }

                    }

                }
            return Redirect("/home");
        }

        public async System.Threading.Tasks.Task<JsonResult> CancelOrder(long? id)
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;

            if (userSession != null)
            {
                var user = _userService.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username)) as User;
                if (user != null)
                {
                    var order = user.Customer.Orders.SingleOrDefault(o => o.OrderId == id & o.Status==OrderStatus.Pending);
                    if (order != null)
                    {
                        order.Status = OrderStatus.Canceled;

                        var result = await _orderService.UpdateAsync(order,order.OrderId);
                        if (result!=null)
                        {
                            string path = string.Format("~/Images/Upload/{0}", result.FolderImage);
                            var fullPath = Server.MapPath(path);
                            if (System.IO.Directory.Exists(fullPath))
                            {
                                System.IO.Directory.Delete(fullPath, true);
                            }
                            return Json(new { status = true, url = "/details/" + order.OrderId },JsonRequestBehavior.AllowGet);

                        }
                     }


                }

            }
            return Json(new { status = false}, JsonRequestBehavior.AllowGet);
        }
    }
}