using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RazerMaster
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "SearchProduct",
            url: "Product/SearchProduct/{keyword}",
            defaults: new { controller = "Product", action = "SearchProduct", keyword = UrlParameter.Optional },
            namespaces: new string[] { "RazerMaster.Controllers" }
            );

            routes.MapRoute(
            name: "CommentProduct",
            url: "Product/AddComment",
            defaults: new { controller = "Product", action = "AddComment", keyword = UrlParameter.Optional },
            namespaces: new string[] { "RazerMaster.Controllers" }
            );

            routes.MapRoute(
                name: "ProdCategory",
                url: "Product/{cat}",
                defaults: new { controller = "Product", action = "Index", cat = UrlParameter.Optional },
                namespaces: new string[] { "RazerMaster.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "RazerMaster.Controllers" }
            );
        }
    }
}
