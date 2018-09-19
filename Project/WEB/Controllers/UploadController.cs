using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constants;
using Entities;
using Helpers;
using Newtonsoft.Json;
using Repositories;
using Services;
using WEB.Models;

namespace WEB.Controllers
{
    public class UploadController : Controller
    {
        private readonly IBaseService<Product> _productRepository;
        public UploadController(IBaseService<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        // GET: Upload
        public ActionResult Index()
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as  UserSession;
            if (userSession==null)
            {

                return RedirectToAction("Index","LoginUser",new {url = Request.Url.ToString()});
            }
            var product = _productRepository.FindAll(p => p.Status.Equals(Status.Active));
            ViewBag.Product = product;
            return View();
        }
        public ActionResult GetCartUpload()
        {

            var cart = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
            ViewBag.Cart = cart;
            return PartialView("~/Views/Upload/Cart.cshtml");
        }

        public JsonResult AddCart(string image)
        {
            //byte[] imageByte = Convert.FromBase64String(image);
            var t = image.Substring(23);
            CartItem cartItem = new CartItem();
            cartItem.Image = t;
            cartItem.Quantity = 1;
            var session = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
            if (session == null)
            {
                session = new List<CartItem>();
                session.Add(cartItem);
            }
            else
            {
                session.Add(cartItem);
                
            }
            SessionHelper.SetSession(session, AppSettingConstant.CartSession);
            return Json(new {status = false},JsonRequestBehavior.AllowGet);
        }

    }
}