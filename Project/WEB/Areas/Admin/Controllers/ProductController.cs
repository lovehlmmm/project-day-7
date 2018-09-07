using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Constants;
using Entities;
using Helpers;
using Services;

namespace WEB.Areas.Admin.Controllers
{
    [LoginAdminRequired]
    public class ProductController : Controller
    {
        private IBaseService<Product> _productService;

        public ProductController(IBaseService<Product> productService)
        {
            _productService = productService;
        }
        // GET: Admin/Size
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetList()
        {
            try
            {
                var pageNumber = int.Parse(Request.QueryString["pageNumber"]);
                var pageSize = int.Parse(Request.QueryString["pageSize"]);
                var list = await _productService.GetAllAsync(pageNumber, pageSize, p => p.ProductId,
                    s => s.Status != Status.Deleted);
                var total = await _productService.CountAsync(size =>size.Status!=Status.Deleted);
                var totalPage = (int)Math.Ceiling((double)(total / pageSize))+1;
                return Json(new {status = true, data = list,totalPage= totalPage}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false}, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Create(Product product)
        {
            try
            {
                if (product!=null)
                {
                    product.CreatedAt = DateTime.Now;
                    var result = await _productService.AddAsync(product);
                    if (result!=null)
                    {
                        return Json(new { status = true}, JsonRequestBehavior.AllowGet);
                    }
                }    
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Get(int id)
        {
            try
            {
                var product = await _productService.FindAsync(p => p.ProductId==id);
                if (product!=null)
                {
                    return Json(new { status = true,data=product }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
            
        public async Task<JsonResult> Update(Product product,int id)
        {
            
            try
            {
                var checkProduct = await _productService.FindAsync(p => p.ProductId == id);
                if (checkProduct!=null &(product.Status.Equals(Status.Inactive) || product.Status.Equals(Status.Active)))
                {
                    checkProduct.ProductName = product.ProductName;
                    checkProduct.ProductPrice = product.ProductPrice;
                    checkProduct.ProductSize = product.ProductSize;
                    checkProduct.ModifiedAt = DateTime.Now;
                    checkProduct.Status = product.Status;
                    var result = await _productService.UpdateAsync(checkProduct, id);
                    if (result!=null)
                    {
                        return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {

            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                var product = await _productService.FindAsync(p => p.ProductId == id);
                if (product != null)
                {
                    product.Status = Status.Deleted;
                    var result = await _productService.UpdateAsync(product, id);
                    if (result!=null)
                    {
                        return Json(new { status = true}, JsonRequestBehavior.AllowGet);
                    }     
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
    }
}