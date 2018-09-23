using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Constants;
using Entities;
using Services;

namespace WEB.Areas.Admin.Controllers
{
    [LoginAdminRequired]
    public class MaterialController : Controller
    {
        private IBaseService<Material> _materialService;

        public MaterialController(IBaseService<Material> materialService)
        {
            _materialService = materialService;

        }
        // GET: Admin/Material
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
                var list = await _materialService.GetAllAsync(pageNumber, pageSize, m => m.Id,
                    size => size.Status != Status.Deleted);
                var total = await _materialService.CountAsync(size => size.Status != Status.Deleted);
                var totalPage = (int)Math.Ceiling((double)(total / pageSize)) + 1;
                return Json(new { status = true, data = list, totalPage = totalPage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Create(Material material)
        {
            try
            {
                if (material != null)
                {
                    material.CreatedAt = DateTime.Now;
                    var result = await _materialService.AddAsync(material);
                    if (result != null)
                    {
                        return Json(new { status = true }, JsonRequestBehavior.AllowGet);
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
                var material = await _materialService.FindAsync(m => m.Id == id);
                if (material != null)
                {
                    return Json(new { status = true, data = material }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Update(Material material, int id)
        {

            try
            {
                var checkMaterial = await _materialService.FindAsync(m => m.Id == id);
                if (checkMaterial != null & (material.Status.Equals(Status.Inactive) || material.Status.Equals(Status.Active)))
                {
                    checkMaterial.Name = material.Name;
                    checkMaterial.Price = material.Price;
                    checkMaterial.ModifiedAt = DateTime.Now;
                    checkMaterial.Status = material.Status;
                    var result = await _materialService.UpdateAsync(checkMaterial, id);
                    if (result != null)
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
                var material = await _materialService.FindAsync(m => m.Id == id);
                if (material != null)
                {
                    material.Status = Status.Deleted;
                    var result = await _materialService.UpdateAsync(material, id);
                    if (result != null)
                    {
                        return Json(new { status = true }, JsonRequestBehavior.AllowGet);
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