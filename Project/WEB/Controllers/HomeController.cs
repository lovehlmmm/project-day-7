using Constants;
using Helpers;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        public ActionResult Index()
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
            if (userSession != null)
            {
                var user = _userService.Find(u => u.Username == userSession.Username & u.Status == Status.Active & u.Role == UserRole.Customer);
                if (user == null)
                {
                    Session.RemoveAll();
                }
            }
            ViewBag.Title = "Home Page";
            return View();
        }
        public ActionResult GetNotification()
        {
            return PartialView("~/Views/Shared/_Notification.cshtml");
         }
    }
}
