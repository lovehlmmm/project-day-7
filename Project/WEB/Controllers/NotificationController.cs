using Constants;
using Entities;
using Helpers;
using Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBaseService<Notification> _notificationService;
        public NotificationController(IUserService userService, IBaseService<Notification> notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }
        // GET: Notification
        public ActionResult GetNotification(int takeCount,int page)
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
            if (userSession!=null)
            {
                var user = _userService.Find(u => u.Username == userSession.Username & u.Status == Status.Active & u.Role == UserRole.Customer);
                if (user!=null)
                {
                    var today = DateTime.Now.Date;
                    var notis =  _notificationService.GetAll(takeCount, page, n => n.CreatedAt, n => n.SendTo == user.Username, n => n.CreatedAt).ToList();
                    //var notis = _notificationService.FindAll(n => n.SendTo == user.Username/* & DbFunctions.TruncateTime(n.CreatedAt)==today*/).OrderByDescending(n=>n.CreatedAt)./*Take().*/ToList();
                    ViewBag.Notifications = notis;
                }
            }

            return PartialView("~/Views/Shared/_Notification.cshtml");
        }
    }
}