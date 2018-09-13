using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Constants;
using Entities;
using Helpers;
using Services;
using WEB.Helpers;
using WEB.Models;

namespace WEB.Controllers
{
    public class RegisterUserController : Controller
    {
        private readonly IUserService _userService;

        public RegisterUserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: ResgisterUser
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(User user, Customer customer)
        {
            HashingData hashingData = new HashingData(AppSettingConstant.SaltLength);
            try
            {
                user.Role = UserRole.Customer;
                user.Status = Status.Inactive;
                user.Customer = customer;
                var key = hashingData.EncryptString(user.Username, AppSettingConstant.PasswordHash);
                user.ActiveMail = key;
                var result = _userService.Register(user);              
                if (result)
                {

                    
                    UserEmailConfirm model = new UserEmailConfirm(user.Email, key);
                    var body = ViewToString.RenderRazorViewToString(this, "ConfirmAccount", model);
                    Task.Factory.StartNew((() =>
                    {
                        SendEmail.Send(user.Email, body, "Confirm your email!");
                    }));
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
            }
            return Json(new {status = false},JsonRequestBehavior.AllowGet);
        }
    }
}