using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Areas.Admin.Assets.js
{
    public class NotificationAdminController : Controller
    {
        // GET: Admin/NotificationAdmin
        public JsonResult GetNotification()
        {
            var message = "";

            return Json(new { status = false, message }, JsonRequestBehavior.AllowGet);
        }
    }
}