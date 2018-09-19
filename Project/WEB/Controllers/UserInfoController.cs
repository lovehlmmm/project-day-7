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
            var userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
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

        public async System.Threading.Tasks.Task<JsonResult> UpdateUser(User userUpdate)
        {
            var userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
            if (userSession != null)
            {
                var user = _userService.Find(u => u.Username == userSession.Username);
                if (user != null)
                {
                    if (!userUpdate.Password.Equals(""))
                    {
                        user.Password = userUpdate.Password;
                    }
                    user.Customer = userUpdate.Customer;
                    user.ModifiedAt = DateTime.Now;
                    var result = await _userService.UpdateAsync(user, user.Username);
                    if (result!=null)
                    {
                        return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                    }
                    
                }

            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
    }
}