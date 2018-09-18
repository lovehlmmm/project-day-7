using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constants;
using Helpers;
using Newtonsoft.Json;
using WEB.Models;

namespace WEB.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
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