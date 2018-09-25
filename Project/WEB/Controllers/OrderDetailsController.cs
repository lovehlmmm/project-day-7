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
        public OrderDetailsController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: OrderDetails
        public ActionResult Index(long id)
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
            return Redirect("/login");
        }
    }
}