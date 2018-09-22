﻿using Constants;
using Entities;
using Helpers;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
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

        public JsonResult Confirm()
        {
            try
            {
                var userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer);
                if (userSession == null)
                {
                    return Json(new { status = false });
                }
                var cartSession = SessionHelper.GetSession(AppSettingConstant.CartSession);
                if (cartSession != null)
                {
                    var user = userSession as UserSession;
                    String path = Server.MapPath(string.Format("~/Images/Upload/{0}/{1}", user.Username, DateTime.Now.ToString())); //Path

                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }
                    foreach (var item in cartSession as List<CartItem>)
                    {
                        string imgPath = Path.Combine(path, item.ImageTitle);
                        byte[] imageBytes = Convert.FromBase64String(item.Image);
                        System.IO.File.WriteAllBytes(imgPath, imageBytes);
                    }
                }

            }
            catch (Exception e)
            {
            }
            return Json(new { status = false });
        }
        public ActionResult ConfirmTest()
        {
            try
            {
                var userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer);
                if (userSession == null)
                {
                    return Redirect("/home");
                }
                var cartSession = SessionHelper.GetSession(AppSettingConstant.CartSession);
                if (cartSession != null)
                {
                    var user = userSession as UserSession;
                    string folderPath = string.Format("~/Images/Upload/{0}/{1}_{2}", user.Username, DateTime.Now.Second, DateTime.Now.Millisecond);

                    String path = Server.MapPath(folderPath); //Path

                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }
                    foreach (var item in cartSession as List<CartItem>)
                    {
                        string imgPath = Path.Combine(path, item.ImageTitle);
                        byte[] imageBytes = Convert.FromBase64String(item.Image);
                        System.IO.File.WriteAllBytes(imgPath, imageBytes);
                    }
                    return Redirect("/upload");
                }

            }
            catch (Exception e)
            {
            }
            return Redirect("/home");
        }
    }
}