using Constants;
using Entities;
using Helpers;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public CheckOutController(IBaseService<Product> productRepository, IBaseService<Material> materialRepository, IBaseService<Address> addressRepository, IBaseService<User> userRepository)
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

            if (userSession != null)
            {
                var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                if (user != null)
                {
                    var cart = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
                    ViewBag.Cart = cart;
                    ViewBag.User = user;
                    return View();
                }

            }
            return Redirect("/home");
        }

        public ActionResult GetModalAddress()
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
            return PartialView("~/Views/CheckOut/ModalAddress.cshtml");
        }
        public ActionResult GetModalAddAddress()
        {
            return PartialView("~/Views/CheckOut/ModalAddAddressPartial.cshtml");

        }
        [AllowAnonymous]
        public JsonResult Confirm(long addressId, string phone)
        {
            if (phone == "" & addressId == 0)
                return Json(new { status = false, message = "Please enter your information!" }, JsonRequestBehavior.AllowGet);
            else if (addressId == 0)
                return Json(new { status = false, message = "Please enter your address!" }, JsonRequestBehavior.AllowGet);
            else if (phone == "")
                return Json(new { status = false, message = "Please enter your phone!" }, JsonRequestBehavior.AllowGet);

            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
            if (userSession != null)
            {
                var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                if (user != null)
                {
                    var address = _addressRepository.Find(a => a.AddressId.Equals(addressId));
                    if (address != null)
                    {
                        CheckOut checkOut = new CheckOut();
                        checkOut.Address = address;
                        checkOut.PhoneNumber = phone;
                        TempData["checkout"] = checkOut;
                        Order order = new Order();
                        order.AddressId = address.AddressId;
                        order.PhoneNumber = phone;
                        SessionHelper.SetSession(order, AppSettingConstant.CheckOutSession);
                        return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult ConfirmTest()
        //{
        //    try
        //    {
        //        var userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer);
        //        if (userSession == null)
        //        {
        //            return Redirect("/home");
        //        }
        //        var cartSession = SessionHelper.GetSession(AppSettingConstant.CartSession);
        //        if (cartSession != null)
        //        {
        //            var user = userSession as UserSession;
        //            string folderPath = string.Format("~/Images/Upload/{0}/{1}_{2}", user.Username, DateTime.Now.Second, DateTime.Now.Millisecond);

        //            String path = Server.MapPath(folderPath); //Path

        //            //Check if directory exist
        //            if (!System.IO.Directory.Exists(path))
        //            {
        //                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
        //            }
        //            foreach (var item in cartSession as List<CartItem>)
        //            {
        //                string imgPath = Path.Combine(path, item.ImageTitle);
        //                byte[] imageBytes = Convert.FromBase64String(item.Image);
        //                System.IO.File.WriteAllBytes(imgPath, imageBytes);
        //            }
        //            return Redirect("/upload");
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    return Redirect("/home");
        //}
        public async System.Threading.Tasks.Task<JsonResult> AddAddress(string address)
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
            if (userSession != null)
            {
                var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                if (user != null)
                {
                    Address obj = new Address();
                    obj.CustomerId = user.CustomerId;
                    obj.AddressDetails = address;
                    obj.CreatedAt = DateTime.Now;
                    obj.Status = Status.Active;
                    var result = await _addressRepository.AddAsync(obj);
                    if (result != null)
                    {
                        return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                    }
                }

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