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
                var checkUser = _userService.CheckLogin(userLogin.Username.Trim(), userLogin.Password.Trim(),UserRole.Admin);
                if (checkUser != null)
                {
                    if (!checkUser.Role.Equals(UserRole.Admin)) return Json(new { status = false });
                    UserSession userSession = new UserSession(checkUser.Username, checkUser.Role);
                    SessionHelper.SetSession(userSession, AppSettingConstant.LoginSessionAdmin);
                    return Json(new { status = true, url = "/admin/home" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Logout()
        {
            try
            {
                var checkSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionAdmin);
                if (checkSession != null)
                {
                    SessionHelper.Delete(AppSettingConstant.LoginSessionAdmin);
                    return Json(new { status = true, url="/admin" },JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
    }
}