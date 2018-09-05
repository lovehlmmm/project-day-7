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
using Constants;
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
                var checkUser= _userService.CheckLogin(userLogin.Username.Trim(), userLogin.Password.Trim());
                if (checkUser!= null)
                {
                    if(!checkUser.Role.Equals(UserRole.Admin)) return Json(new { status = false });
                    UserSession userSession = new UserSession(userLogin.Username, userLogin.Password);
                    SessionHelper.SetSession(userSession, AppSettingConstant.LoginSessionAdmin);
                    return Json(new { status = true,url = "/admin"}, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new {status=false});
        }
    }
}