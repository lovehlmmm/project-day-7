using Constants;
using Entities;
using Helpers;
using Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
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

            HashingData hashingData = new HashingData();
            try
            {
                user.Role = UserRole.Customer;
                user.Status = Status.Inactive;
                customer.CreatedAt = DateTime.Now;
                customer.Status = Status.Inactive;
                user.Customer = customer;
                var key = hashingData.EncryptString(user.Username, AppSettingConstant.PasswordHash);
                user.ActiveMail = key;
                user.CreatedAt = DateTime.Now;
                var result = _userService.Register(user);
                if (result)
                {
                    UserEmailConfirm model = new UserEmailConfirm(user.Email, hashingData.Encode(key), user.Username);
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
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckExistAccount(string username)        {            if (username != null)            {                var userCheck = _userService.Find(u => u.Username.Equals(username) & u.Status != Status.Deleted & u.Role != UserRole.Admin);                if (userCheck == null)                {                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);                }            }            return Json(new { status = false }, JsonRequestBehavior.AllowGet);        }
    }
}