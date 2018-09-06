using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Entities;
using Helpers;
using Services;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HashingData data = new HashingData(8);
            ViewBag.Title = data.EncryptString("123456","team6").ToString();
            return View();
        }
    }
}
