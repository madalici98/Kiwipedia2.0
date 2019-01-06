using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kiwipedia2._0
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Index",
                url: "Article/Index/Sort/{type}",
                defaults: new { controller = "Article", action = "Sort", type = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Sort",
                url: "Article/Index/{category}",
                defaults: new { controller = "Article", action = "Index", category = ""}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
