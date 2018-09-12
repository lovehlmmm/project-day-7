using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constants;
using Entities;
using Services;

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
            try
            {
                user.Role = UserRole.Customer;
                user.Status = Status.Active;
                user.Customer = customer;
                var result = _userService.Register(user);
                
                if (result)
                {
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