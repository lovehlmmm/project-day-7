using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Constants;
using Helpers;
using Services;
using WEB.Areas.Admin.Models;
using WEB.Models;

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
        public ActionResult Index(string key = null)
        {
            if (key!=null)
            {
                TempData["key"] = key;
                return Redirect("/admin");
            }
            ViewBag.Url = Request.QueryString["url"];
            return View();
        }

        public JsonResult CheckLogin(LoginUser loginUser, string url)
        {
            var message = "";
            var status = false;
            try
            {
                var checkUser = _userService.CheckLogin(loginUser.Username.Trim(), loginUser.Password.Trim(), UserRole.Customer);
                if (checkUser != null)
                {
                    if (!checkUser.Role.Equals(UserRole.Customer)) return Json(new { status = false });
                    if (checkUser.Status == Status.Inactive)
                    {
                        message = "Account locked";
                    }else if (checkUser.ActiveMail != null && checkUser.Status == Status.Inactive)
                    {
                        message = "Account is not verify email!";
                    }
                    else
                    {
                        UserLoginCookie loginCookie = new UserLoginCookie();
                        loginCookie.Username = checkUser.Username;
                        loginCookie.CustomerName = checkUser.Customer.CustomerName;
                        var cookie = CookieHelper.Create(AppSettingConstant.LoginCookieCustomer, Server.UrlEncode(JsonConvert.SerializeObject(loginCookie, Formatting.Indented)), DateTime.Now.AddDays(1));
                        Response.Cookies.Add(cookie);
                        UserSession userSession = new UserSession(checkUser.Username, checkUser.Role);
                        SessionHelper.SetSession(userSession, AppSettingConstant.LoginSessionCustomer);
                        SessionHelper.SetSession(userSession.Username, AppSettingConstant.NotifiSession);
                        status = true;
                    }

                }
                else
                {
                    message = "Username password incorrect!";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = status, message, url = url }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Logout()
        {
            Session.RemoveAll();
            if (Request.Cookies[AppSettingConstant.LoginCookieCustomer] != null)
            {
                var c = new HttpCookie(AppSettingConstant.LoginCookieCustomer);
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return RedirectToAction("index", "home");
        }

    }
}