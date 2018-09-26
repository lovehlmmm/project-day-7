using Constants;
using Entities;
using Helpers;
using Services;
using System.Linq;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class UserOrdersHistoryController : Controller
    {
        private readonly IUserService _userService;
        public UserOrdersHistoryController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: UserOrdersHistory
        public ActionResult Index()
        {

            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;

            if (userSession != null)
            {
                var user = _userService.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username)) as User;
                if (user != null)
                {
                    var orders = user.Customer.Orders.Where(o => o.Status!=Status.Deleted).ToList();
                    if (orders != null)
                    {
                        ViewBag.Orders = orders;
                        return View();
                    }

                }

            }
            return RedirectToAction("Index", "LoginUser", new { url = Request.Url.ToString() });

        }
    }
}