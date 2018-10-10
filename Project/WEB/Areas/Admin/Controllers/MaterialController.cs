using Constants;
using Entities;
using Helpers;
using Newtonsoft.Json;
using Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WEB.Areas.Admin.Controllers
{
    [LoginAdminRequired]
    public class MaterialController : Controller
    {
        private IBaseService<Material> _materialService;
        private IBaseService<Group> _groupService;

        public MaterialController(IBaseService<Material> materialService, IBaseService<Group> groupService)
        {
            _materialService = materialService;
            _groupService = groupService;
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
        public async Task<JsonResult> Create(string materials, HttpPostedFileBase materialimg)
        {

            try
            {
                if (materials != null)
                {
                    var checkmaterial = JsonConvert.DeserializeObject<Material>(materials);
                    checkmaterial.CreatedAt = DateTime.Now;
                    if (!System.IO.Directory.Exists(Server.MapPath("~/Images/Material")))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/Material"));
                    }
                    if (materialimg != null)
                    {
                        string pic = System.IO.Path.GetFileName(materialimg.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Images/Material"), pic);
                        // file is uploaded
                        materialimg.SaveAs(path);
                        checkmaterial.Image = pic;
                    }
                    var result = await _materialService.AddAsync(checkmaterial);

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
                var group = _groupService.FindAll(g => g.Status.Equals(Status.Active)).ToList();

                if (material != null)
                {
                    return Json(new { status = true, data = material, group }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Update(string materials, int id, HttpPostedFileBase materialimg)
        {

            try
            {
                var material = JsonConvert.DeserializeObject<Material>(materials);
                var checkMaterial = await _materialService.FindAsync(m => m.Id == id);
                if (checkMaterial != null & (material.Status.Equals(Status.Inactive) || material.Status.Equals(Status.Active)))
                {
                    checkMaterial.Name = material.Name;
                    checkMaterial.Price = material.Price;
                    if (!System.IO.Directory.Exists(Server.MapPath("~/Images/Material")))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Images/Material"));
                    }
                    if (materialimg != null)
                    {
                        string pic = System.IO.Path.GetFileName(materialimg.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Images/Material"), pic);
                        // file is uploaded
                        materialimg.SaveAs(path);
                        checkMaterial.Image = pic;
                    }
                    if (material.GroupId == 0)
                    {
                        checkMaterial.GroupId = null;
                    }
                    else
                    {
                        checkMaterial.GroupId = material.GroupId;
                    }
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

        public JsonResult GetGroup()
        {
            try
            {
                var group = _groupService.FindAll(g => g.Status.Equals(Status.Active)).ToList();

                if (group != null)
                {
                    return Json(new { status = true, data = group }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModalGroup()
        {

            var group = _groupService.FindAll(g => g.Status.Equals(Status.Active)).ToList();
            if (group != null)
            {
                ViewBag.Group = group;
            }
            return PartialView("~/Areas/Admin/Views/Material/ModalGroup.cshtml");
        }

        public async Task<JsonResult> GetDataGroup(int id)
        {
            try
            {
                var group = await _groupService.FindAsync(g => g.Id == id);

                if (group != null)
                {
                    return Json(new { status = true, data =  group }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> UpdateGroup(string group , int id)
        {
            try
            {
                var checkGroup = await _groupService.FindAsync(g => g.Id == id);
                var groupD = JsonConvert.DeserializeObject<Group>(group);
                if (checkGroup != null & (groupD.Status.Equals(Status.Inactive) || groupD.Status.Equals(Status.Active)))
                {     
                    checkGroup.GroupName = groupD.GroupName;
                    checkGroup.MaxItem = groupD.MaxItem;
                    checkGroup.ModifiedAt = DateTime.Now;
                    checkGroup.Status = groupD.Status;
                    var result = await _groupService.UpdateAsync(checkGroup, id);
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

        public async Task<JsonResult> NewGroup(string group)
        {

            try
            {
                if (group != null)
                {
                    var groupD = JsonConvert.DeserializeObject<Group>(group);
                    var checkGroup = new Group();
                    checkGroup.GroupName = groupD.GroupName;
                    checkGroup.MaxItem = groupD.MaxItem;
                    checkGroup.Status = groupD.Status;
                    checkGroup.CreatedAt = DateTime.Now;

                    var result = await _groupService.AddAsync(checkGroup);

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
        public async Task<JsonResult> DeleteGroup(int id)
        {
            try
            {
                var group = await _groupService.FindAsync(g => g.Id == id);
                if (group != null)
                {
                    group.Status = Status.Deleted;
                    var result = await _groupService.UpdateAsync(group, id);
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