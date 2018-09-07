﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;

namespace WEB.Areas.Admin.Controllers
{
    [LoginAdminRequired]
    public class HomeController : Controller
    {
        // GET: Admin/Home        
        public ActionResult Index()
        {
            return View();
        }
    }
}