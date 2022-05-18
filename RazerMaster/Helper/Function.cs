using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;



namespace RazerMaster.Functions
{
    public static class Function
    {
        public static string GetPricePercentage(decimal origin, decimal sale)
        {
            if (origin - sale <= 0)
            {
                return String.Empty;
            }
            else
            {
                return Math.Truncate((1 - sale / origin) * 100).ToString() + "% OFF";
            }
        }

        public static string RenderRazorViewToString(ControllerContext controllerContext, String viewName, Object model)
        {
            controllerContext.Controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                //將渲染出來資料透過Response.Output輸出到Client端
                ViewResult.View.Render(ViewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

    }
}