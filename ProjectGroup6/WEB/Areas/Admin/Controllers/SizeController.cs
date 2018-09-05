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
    public class SizeController : Controller
    {
        private IBaseService<Size> _sizeService;

        public SizeController(IBaseService<Size> sizeService)
        {
            _sizeService = sizeService;
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
                var list = await _sizeService.GetAllAsync(pageNumber, pageSize, size => size.SizeId,
                    size => size.Status != Status.Deleted);
                return Json(new {status = true, data = list.OrderByDescending(size =>size.SizeId)}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false}, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Create(Size size)
        {
            try
            {
                if (size!=null)
                {
                    size.CreatedAt = DateTime.Now;
                    var result = await _sizeService.AddAsync(size);
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
                var size = await _sizeService.FindAsync(s => s.SizeId==id);
                if (size!=null)
                {
                    return Json(new { status = true,data=size }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                var size = await _sizeService.FindAsync(s => s.SizeId == id);
                if (size != null)
                {
                    size.Status = Status.Deleted;
                    var result = await _sizeService.UpdateAsync(size, id);
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