using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities;
using Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services;
using WEB.Areas.Admin.Models;

namespace WEB.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckLogin(string user)
        {
            try
            {
                LoginUser userLogin = JsonConvert.DeserializeObject<LoginUser>(user);
               var checkUser= _userService.CheckLogin(userLogin.Username, userLogin.Password);
                if (checkUser!= null)
                {
                    return Json(new { status = true });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Json(new {status=false});
        }
    }
}