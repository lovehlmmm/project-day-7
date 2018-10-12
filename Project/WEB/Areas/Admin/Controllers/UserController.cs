using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
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
    public class UserController : Controller
    {
        private IBaseService<User> _userService;
        private IBaseService<CreditCard> _creditcardService;


        public UserController(IBaseService<User> userService, IBaseService<CreditCard> creditcardService)
        {
            _userService = userService;
            _creditcardService = creditcardService;
        }
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(string username)
        {
            if (username!=null)
            {
                try
                {
                    var checkUser = _userService.Find(u => u.Username == username.Trim());
                    if (checkUser!=null)
                    {
                        ViewBag.User = checkUser;
                        return View();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return Redirect("/error-404");
        }

        public async Task<JsonResult> GetList()
        {
            try
            {
                var pageNumber = int.Parse(Request.QueryString["pageNumber"]);
                var pageSize = int.Parse(Request.QueryString["pageSize"]);
                var filter = Request.QueryString["filter"];
                var filterDate = Request.QueryString["date"];
                string search = Request.QueryString["search"].Trim();
                Expression<Func<User, bool>> expression = (u)=>u.Role.ToLower()==UserRole.Customer.ToLower() & u.Status.ToLower() != Status.Deleted.ToLower();
                if (filter != "")
                {
                    expression = expression.And(o => o.Status.ToLower().Equals(filter.ToLower()));
                }
                if (filterDate.Trim() != "")
                {
                    string[] date = Regex.Split(filterDate, "-");
                    DateTime from = Convert.ToDateTime(date[0]).Date;
                    DateTime to = Convert.ToDateTime(date[1]).Date;
                    expression = expression.And(u => DbFunctions.TruncateTime(u.CreatedAt) >= from & DbFunctions.TruncateTime(u.CreatedAt) <= to);
                }
                var list = await _userService.GetAllAsync(pageNumber, pageSize, u => u.Username,
                    expression);
                if (search !="")
                {
                    expression = expression.And(u => u.Username.Contains(search));
                    list = _userService.FindAll(expression);
                }
                var total = _userService.FindAll(expression).Count();
                var totalPage = (int)Math.Ceiling((double)(total / pageSize)) + 1;
                return Json(new { status = true, data = list.Select(l=> new { l.Username,l.Customer.CustomerName,l.Role,l.Status,l.CreatedAt,l.ModifiedAt}), totalPage = totalPage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> ChangeStatus()
        {
            var username = Request.QueryString["username"];
            int mode = -1;
            int.TryParse(Request.QueryString["mode"],out mode);
            var message = "";
            try
            {
                var user = _userService.Find(u => u.Username.Equals(username.Trim()));
                if (user != null)
                {
                    if (mode == 1)
                    {
                        switch (user.Status)
                        {
                            case Status.Active:
                                user.Status = Status.Inactive;
                                user.ModifiedAt = DateTime.Now;
                                message = "Inactive account successfully";
                                break;
                            case Status.Inactive:
                                user.Status = Status.Active;
                                user.ModifiedAt = DateTime.Now;
                                message = "Active account successfully";
                                break;
                        }
                    }
                    else if (mode == 0)
                    {
                        user.Status = Status.Deleted;
                        user.ModifiedAt = DateTime.Now;
                        message = "Delete Success";
                    }
                    var updated = await _userService.UpdateAsync(user, user.Username);
                    if (updated != null)
                    {
                        return Json(new { status = true, message}, JsonRequestBehavior.AllowGet);
                    }
                    else
                        message = "Update failed, please try again";
                }
                else
                    message = "Account does not exist";
            }
            catch (Exception)
            {
                message = "Something wrong!!";
            }
            return Json(new { status = true,message}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCreditCard(long id)
        {
            try
            {
                var credit = _creditcardService.Find(c => c.CreditCardId == id);
                if (credit != null)
                {
                    return Json(new { status = true, data = new { CreditNumber=credit.CreditNumber.Substring(12,4), credit.Expire, credit.CVC } }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

    }
}