using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "LoginUser", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Register",
                url: "register",
                defaults: new { controller = "RegisterUser", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Info",
                url: "info",
                defaults: new { controller = "UserInfo", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Active",
                url: "active",
                defaults: new { controller = "Active", action = "Index"}
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
