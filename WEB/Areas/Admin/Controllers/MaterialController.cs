using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Areas.Admin.Controllers
{
    [LoginAdminRequired]
    public class MaterialController : Controller
    {
        // GET: Admin/Material
        public ActionResult Index()
        {
            return View();
        }
    }
}