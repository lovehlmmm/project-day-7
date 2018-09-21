using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var products = _productRepository.FindAll(p => p.Status.Equals(Status.Active));
            ViewBag.Products = products;
            return PartialView("~/Views/Upload/Cart.cshtml");
        }

        public JsonResult AddCart(string item,int id)
        {
            //byte[] imageByte = Convert.FromBase64String(image);

            if (item==null)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            var cartItem = JsonConvert.DeserializeObject<CartItem>(item);
            var product=_productRepository.Find(p => p.ProductId == id);
            if (product==null)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            var match = Regex.Match(cartItem.Image, @"data:(?<type>.+?);base64,(?<data>.+)");
            var base64Data = match.Groups["data"].Value;
            var contentType = match.Groups["type"].Value;
            cartItem.Product = product;
            cartItem.ImageType = contentType;
            cartItem.Image = base64Data;

            var session = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
            if (session == null)
            {
                session = new List<CartItem>();
                session.Add(cartItem);
            }
            else
            {
                var check = session.FirstOrDefault(c => c.ImageTitle.Equals(cartItem.ImageTitle) & c.Product.ProductId == id);
                if (check != null)
                {
                    check.Quantity += cartItem.Quantity;
                }
                else
                {
                    session.Add(cartItem);
                }

            }
            SessionHelper.SetSession(session, AppSettingConstant.CartSession);
            return Json(new {status = true},JsonRequestBehavior.AllowGet);
        }

    }
}