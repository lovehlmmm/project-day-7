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
        private readonly IBaseService<Material> _materialRepository;
        private readonly IUserService _userService;
        public UploadController(IBaseService<Product> productRepository, IBaseService<Material> materialRepository, IUserService userService)
        {
            _productRepository = productRepository;
            _materialRepository = materialRepository;
            _userService = userService;
        }
        // GET: Upload
        public ActionResult Index()
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as  UserSession;
            if (userSession==null)
            {

                return RedirectToAction("Index","LoginUser",new {url = Request.Url.ToString()});
            }
            var user = _userService.Find(u => u.Username == userSession.Username & u.Status == Status.Active & u.Role == UserRole.Customer);
            if (user==null)
            {
                return RedirectToAction("Index", "LoginUser", new { url = Request.Url.ToString() });
            }
            var product = _productRepository.FindAll(p => p.Status.Equals(Status.Active));
            var materials = _materialRepository.FindAll(m => m.Status == Status.Active);
            ViewBag.Product = product;
            ViewBag.Materials = materials;
            return View();
        }
        public ActionResult GetCartUpload()
        {

            var cart = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
            ViewBag.Cart = cart;
            var products = _productRepository.FindAll(p => p.Status.Equals(Status.Active));
            ViewBag.Products = products;
            var materials = _materialRepository.FindAll(m => m.Status == Status.Active);
            ViewBag.Materials = materials;
            return PartialView("~/Views/Upload/Cart.cshtml");
        }

        public JsonResult AddCart(string item,int productId,int materialId,int option)
        {
            //byte[] imageByte = Convert.FromBase64String(image);

            if (item==null)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            var product=_productRepository.Find(p => p.ProductId == productId && p.Status.Equals(Status.Active));
            if (product==null)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            var material = _materialRepository.Find(m => m.Id == materialId && m.Status.Equals(Status.Active));
            if (material == null)
            {
                return Json(new { status = false }, JsonRequestBehavior.AllowGet);
            }
            var cartItem = JsonConvert.DeserializeObject<CartItem>(item);
            var match = Regex.Match(cartItem.Image, @"data:(?<type>.+?);base64,(?<data>.+)");
            var base64Data = match.Groups["data"].Value;
            var contentType = match.Groups["type"].Value;
            cartItem.Product = product;
            cartItem.Material = material;
            cartItem.ImageType = contentType;
            cartItem.Image = base64Data;
            cartItem.Option = option;
            var session = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
            if (session == null)
            {
                session = new List<CartItem>();
                cartItem.Id = 1;
                session.Add(cartItem);
            }
            else
            {
                var check = session.FirstOrDefault(c => c.ImageTitle.Equals(cartItem.ImageTitle) & c.Product.ProductId == productId & c.Material.Id==materialId);
                if (check != null)
                {
                    check.Quantity += cartItem.Quantity;
                }
                else
                {
                    cartItem.Id = session.Count + 1;
                    session.Add(cartItem);
                }

            }
            SessionHelper.SetSession(session, AppSettingConstant.CartSession);
            return Json(new {status = true},JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteItem (int id)
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
            var session = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
            if (userSession != null)
            {
                if (session!=null)
                {
                    var deleleItem = session.SingleOrDefault(s => s.Id == id);
                    if (deleleItem!=null)
                    {
                        session.Remove(deleleItem);
                        SessionHelper.SetSession(session, AppSettingConstant.CartSession);

                        return Json(new { status = true,count= session.Count}, JsonRequestBehavior.AllowGet);
                    } 
                }
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateItem(CartItem cartItem)
        {
            try
            {
                var product = _productRepository.Find(p => p.ProductId == cartItem.Product.ProductId && p.Status.Equals(Status.Active));
                if (product == null)
                {
                    return Json(new { status = false }, JsonRequestBehavior.AllowGet);
                }
                var material = _materialRepository.Find(m => m.Id == cartItem.Material.Id && m.Status.Equals(Status.Active));
                if (material == null)
                {
                    return Json(new { status = false }, JsonRequestBehavior.AllowGet);
                }
                UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
                var session = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
                if (userSession != null)
                {
                    if (session != null)
                    {
                        var updateItem = session.SingleOrDefault(s => s.Id == cartItem.Id);
                        if (updateItem != null)
                        {
                            updateItem.Product = product;
                            updateItem.Material = material;
                            updateItem.Option = cartItem.Option;
                            updateItem.Quantity = cartItem.Quantity;
                            SessionHelper.SetSession(session, AppSettingConstant.CartSession);
                            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
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