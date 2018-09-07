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
    public class UserController : Controller
    {
        private IBaseService<User> _userService;

        public UserController(IBaseService<User> userService)
        {
            _userService = userService;
        }
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details()
        {
            return View();
        }

        public async Task<JsonResult> GetList()
        {
            try
            {
                var pageNumber = int.Parse(Request.QueryString["pageNumber"]);
                var pageSize = int.Parse(Request.QueryString["pageSize"]);
                var list = await _userService.GetAllAsync(pageNumber, pageSize, u => u.Username,
                    u => u.Status != Status.Deleted & u.Role!=UserRole.Admin);
                var total = await _userService.CountAsync(size => size.Status != Status.Deleted);
                var totalPage = (int)Math.Ceiling((double)(total / pageSize)) + 1;
                return Json(new { status = true, data = list.Select(l=> new { l.Username,l.Customer.CustomerName,l.Role,l.Status,l.CreatedAt,l.ModifiedAt}), totalPage = totalPage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

    }
}