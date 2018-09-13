using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constants;
using Helpers;
using Services;
using WEB.Areas.Admin.Models;

namespace WEB.Controllers
{
    public class LoginUserController : Controller
    {
        private readonly IUserService _userService;

        public LoginUserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: LoginUser
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckLogin(LoginUser loginUser)
        {
            try
            {
                var checkUser = _userService.CheckLogin(loginUser.Username.Trim(), loginUser.Password.Trim(), UserRole.Customer);
                if (checkUser != null)
                {
                    if (!checkUser.Role.Equals(UserRole.Customer)) return Json(new { status = false });
                    UserSession userSession = new UserSession(checkUser.Username, checkUser.Role);
                    SessionHelper.SetSession(userSession, AppSettingConstant.LoginSessionCustomer);
                    return Json(new { status = true}, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConfirmSuccess()
        {
            try
            {
                var temp = TempData["status"];
                if ((bool)temp == true)
                {
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
    }
}