using Constants;
using Entities;
using Helpers;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Models;

namespace WEB.Controllers
{
    public class PaymentCheckOutController : Controller
    {
        private readonly IBaseService<Product> _productRepository;
        private readonly IBaseService<Address> _addressRepository;
        private readonly IBaseService<User> _userRepository;

        public PaymentCheckOutController(IBaseService<Product> productRepository,   IBaseService<Address> addressRepository, IBaseService<User> userRepository)
        {
            _productRepository = productRepository;
             _addressRepository = addressRepository;
            _userRepository = userRepository;
        }
        // GET: PaymentCheckOut
        public ActionResult Index()
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;

            if (userSession != null)
            {
                var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                if (user != null)
                {
                    var checkOut = TempData["checkout"] as CheckOut;
                    if (checkOut != null)
                    {
                        var cart = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
                        ViewBag.Cart = cart;
                        ViewBag.User = user;
                        ViewBag.CheckOut = checkOut;
                        return View();
                    }
                   
                }

            }
            return Redirect("/home");
        }
        public ActionResult GetModalCredit()
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;

            if (userSession != null)
            {
                var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                if (user != null)
                {
                    ViewBag.User = user;
                }
             }
            return PartialView("~/Views/PaymentCheckOut/ModalCredit.cshtml");
        }

        public JsonResult GetCredit(long id)
        {
            try
            {
                UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
                if (userSession != null)
                {
                    var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                    if (user != null)
                    {
                        var credit = user.Customer.CreditCards.SingleOrDefault(a => a.CustomerId == user.CustomerId & a.CreditCardId == id & a.Status.Equals(Status.Active));
                        if (credit != null)
                        {
                            return Json(new { status = true, credit = new { credit.CreditNumber, credit.CreditCardId,credit.Expire} }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
            }
            catch (Exception)
            {

            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAddress(long id)
        {
            try
            {
                UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
                if (userSession != null)
                {
                    var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                    if (user != null)
                    {
                        var address = user.Customer.Addresses.SingleOrDefault(a => a.CustomerId == user.CustomerId & a.AddressId == id);
                        if (address != null)
                        {
                            return Json(new { status = true, address = new { address.AddressDetails, address.AddressId } }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
            }
            catch (Exception)
            {

            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
    }
} 