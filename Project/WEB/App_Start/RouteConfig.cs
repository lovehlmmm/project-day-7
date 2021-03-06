﻿using System;
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
                url: "login/{key}",
                defaults: new { controller = "LoginUser", action = "Index", key = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Register",
                url: "register/{action}",
                defaults: new { controller = "RegisterUser", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "LoginUser", action = "Logout", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "OrderDetails",
                url: "details/{id}",
                defaults: new { controller = "OrderDetails", action = "Index", id = UrlParameter.Optional }
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
               name: "History",
               url: "history",
               defaults: new { controller = "UserOrdersHistory", action = "Index" }
           );
            routes.MapRoute(
            name: "ProductDetails",
            url: "products",
            defaults: new { controller = "ProductDetails", action = "Index" }
        );
            routes.MapRoute(
                name: "Error",
                url: "error-404",
                defaults: new { controller = "Error", action = "NotFound" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
