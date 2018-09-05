using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;

namespace WEB.Areas.Admin.Controllers
{
    
    public class HomeController : Controller
    {
        // GET: Admin/Home
        [LoginAdminRequired]
        public ActionResult Index()
        {
            return View();
        }
    }
}