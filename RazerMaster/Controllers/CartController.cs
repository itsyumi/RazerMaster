using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RazerMaster.Models;
using RazerMaster.Services;
using RazerMaster.Controllers;
using RazerMaster.Functions;

namespace RazerMaster.Controllers
{
    public class CartController : Controller
    {
        //取得目前購物車頁面
        public ActionResult GetCart()
        {
            return PartialView("_CartPartial");
        }

        //以id加入Product至購物車，並回傳購物車頁面
        public ActionResult AddToCart(int id, int count,string type="")
        {
            var currentCart = CartSerivce.GetCurrentCart();
            currentCart.AddProduct(id, count,type);
            var cartPartialView = Function.RenderRazorViewToString(this.ControllerContext, "_CartPartial", currentCart);
            var orderPartialView = Function.RenderRazorViewToString(this.ControllerContext, "_OrderPartial", currentCart);
            var orderDetailPartialView = Function.RenderRazorViewToString(this.ControllerContext, "_OrderDetailPartial", currentCart);

            var json = Json(new { cartPartialView, orderPartialView, orderDetailPartialView });

            return json;
        }

        public ActionResult RemoveFromCart(int id)
        {
            var currentCart = CartSerivce.GetCurrentCart();
            currentCart.RemoveProduct(id);
            var cartPartialView = Function.RenderRazorViewToString(this.ControllerContext, "_CartPartial", currentCart);
            var orderPartialView = Function.RenderRazorViewToString(this.ControllerContext, "_OrderPartial", currentCart);
            var orderDetailPartialView = Function.RenderRazorViewToString(this.ControllerContext, "_OrderDetailPartial", currentCart);

            var json = Json(new { cartPartialView, orderPartialView, orderDetailPartialView });
            return json;
        }

        public ActionResult ClearCart()
        {
            var currentCart = CartSerivce.GetCurrentCart();
            currentCart.ClearCart();
            var cartPartialView = Function.RenderRazorViewToString(this.ControllerContext, "_CartPartial", currentCart);
            var orderPartialView = Function.RenderRazorViewToString(this.ControllerContext, "_OrderPartial", currentCart);
            var json = Json(new { cartPartialView, orderPartialView });
            return json;
        }

        public ActionResult ClickCoinDiscount()
        {
            var currentCart = CartSerivce.GetCurrentCart();
            var total = currentCart.TotalAmount;
            return Json(total);
        }

    }
}