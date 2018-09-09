using System.Web.Mvc;

namespace WEB.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_Home",
                "Admin/home",
                new { area="admin", controller = "dashbroad", action = "Index"}
            );
            context.MapRoute(
                "Admin_Login",
                "Admin/{controller}/{action}",
                new { area = "admin", controller = "login", action = "Index" }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}