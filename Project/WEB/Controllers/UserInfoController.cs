using Constants;
using Entities;
using Helpers;
using Services;
using System;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class UserInfoController : Controller
    {

        private readonly IUserService _userService;

        public UserInfoController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: UserInfo
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public ActionResult GetUser()
        {
            var userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as User;
            if (userSession != null)
            {
                var user = _userService.Find(u => u.Username == userSession.Username);
                if (user != null)
                {
                    user.Password = "";
                    ViewBag.ShowUser = user;
                    return View();
                }

            }
            return Redirect("/home");
        }

        public ActionResult UpdateUser(Customer customer )
        {
 
            return View();
        }
    }
}