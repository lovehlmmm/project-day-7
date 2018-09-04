using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Entities;
using Services;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        private IBaseService<User> _baseService;

        public HomeController(IBaseService<User> baseService)
        {
            _baseService = baseService;
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "";
            return View();
        }
    }
}
