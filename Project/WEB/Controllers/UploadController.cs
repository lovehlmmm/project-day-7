using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constants;
using Helpers;
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

        public JsonResult AddCart(int id, string image, int quantity)
        {
            var cart = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
            return Json(new {status = false});
        }

    }
}