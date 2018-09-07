using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Constants;
namespace Helpers
{
    public class LoginAdminRequiredAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var adminSession = filterContext.HttpContext.Session[AppSettingConstant.LoginSessionAdmin] as UserSession;
            if (adminSession == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { area = "Admin", controller = "login", action = "index" }));
            }
            else
            {
                if (adminSession.Role != UserRole.Admin)

                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { area = "Admin", controller = "login", action = "index" }));
            }
        }
    }

}
