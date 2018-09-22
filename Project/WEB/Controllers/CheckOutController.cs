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
    public class CheckOutController : Controller
    {

        private readonly IBaseService<Product> _productRepository;
        private readonly IBaseService<Material> _materialRepository;
        private readonly IBaseService<Address> _addressRepository;
        private readonly IBaseService<User> _userRepository;


        public CheckOutController(IBaseService<Product> productRepository, IBaseService<Material> materialRepository , IBaseService<Address> addressRepository, IBaseService<User> userRepository)
        {
            _productRepository = productRepository;
            _materialRepository = materialRepository;
            _addressRepository = addressRepository;
            _userRepository = userRepository;
        }
        // GET: CheckOut
        public ActionResult Index()
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
           
            if (userSession!=null)
            {
                var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                if (user!=null)
                {
                    var cart = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
                    ViewBag.Cart = cart;
                    ViewBag.Address = user.Customer.Addresses;
                    return View();
                }
               
            }
                 return Redirect("/home");
        }

        public ActionResult GetModalAddress()
        {
             return PartialView("~/Views/CheckOut/ModalAddress.cshtml");
        }
        public ActionResult GetModalAddAddress()
        {
            return PartialView("~/Views/CheckOut/ModalAddAddressPartial.cshtml");

        }


    }
}