﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckLogin(LoginUser loginUser)
        {
            var message = "";
            var status = false;
            try
            {
                var checkUser = _userService.CheckLogin(loginUser.Username.Trim(), loginUser.Password.Trim(), UserRole.Customer);
                if (checkUser != null)
                {
                    if (!checkUser.Role.Equals(UserRole.Customer)) return Json(new { status = false });
                    if (checkUser.Status==Status.Inactive)
                    {
                        message = "Account is not verify email!";
                    }
                    else {
                        UserLoginCookie loginCookie = new UserLoginCookie();
                        loginCookie.Username = checkUser.Username;
                        loginCookie.CustomerName = checkUser.Customer.CustomerName;
                        var cookie = CookieHelper.Create(AppSettingConstant.LoginCookieCustomer,Server.UrlEncode(JsonConvert.SerializeObject(loginCookie, Formatting.Indented)),DateTime.Now.AddDays(1));
                        Response.Cookies.Add(cookie);
                        UserSession userSession = new UserSession(checkUser.Username,checkUser.Role);
                        SessionHelper.SetSession(userSession,AppSettingConstant.LoginSessionCustomer);
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
            return Json(new { status = status,message}, JsonRequestBehavior.AllowGet);
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