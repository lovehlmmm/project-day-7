using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services;
using Entities;
using Helpers;
using Constants;
namespace WEB.Areas.Admin.Assets.js
{
    [LoginAdminRequired]
    public class NotificationAdminController : Controller
    {
        IBaseService<Notification> _notificationService;
        public NotificationAdminController(IBaseService<Notification> notificationService)
        {
            _notificationService = notificationService;
        }
        // GET: Admin/NotificationAdmin
        public ActionResult GetNotification(int takeCount, int page)
        {
            var session = SessionHelper.GetSession(AppSettingConstant.LoginSessionAdmin) as UserSession;
            var today = DateTime.Now.Date;
            var noti = _notificationService.GetAll(takeCount, page, n => n.CreatedAt, n => n.SendTo == session.Username, n => n.CreatedAt).ToList();
            return PartialView("~/Admin/Views/Shared/_NotificationAdmin.cshtml", noti);
        }
    }
}