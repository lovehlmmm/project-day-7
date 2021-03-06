﻿using Entities;
using Services;
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
    public class ProductDetailsController : Controller
    {
        private readonly IBaseService<Product> _productRepository;
        private readonly IBaseService<Material> _materialRepository;
        private readonly IUserService _userService;
        private readonly IBaseService<Group> _groupRepository;

        public ProductDetailsController(IUserService userService , IBaseService<Product> productRepository, IBaseService<Material> materialRepository, IBaseService<Group> groupRepository)
        {
            _userService = userService;
            _productRepository = productRepository;
            _materialRepository = materialRepository;
            _groupRepository = groupRepository;
        }
        // GET: ProductDetails
        public ActionResult Index(int id)
        {
            var product = _productRepository.FindAll(p => p.Status == Status.Active).ToList();
            var material = _materialRepository.FindAll(m => m.GroupId == id & m.Status== Status.Active).ToList();
            if (material.Count==0)
            {
                return Redirect("/home");
            }
            ViewBag.Products = product;
            ViewBag.Materials = material;
            return View();
        }
        public JsonResult GetMoney(int materialId, int productId)
        {
            var product = _productRepository.Find(p => p.ProductId == productId & p.Status == Status.Active);
            var material = _materialRepository.Find(m => m.Id == materialId & m.Status == Status.Active);

            if (product !=null && material !=null )
            {
                var total = product.ProductPrice + material.Price;
                SessionHelper.SetSession(new ProductDetails() { Material = materialId, Product = productId }, AppSettingConstant.ProductDetailsSession);
                return Json(new { status = true, total = total.Value.ToString("0,0")}, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = false },JsonRequestBehavior.AllowGet );
        }
      
    }
}