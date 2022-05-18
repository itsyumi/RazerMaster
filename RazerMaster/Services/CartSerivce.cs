using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using RazerMaster.Models;
using RazerMaster.Models.Cart;


namespace RazerMaster.Services
{
    //購物車操作類別(使用WebMethod需宣告為靜態)
    public static class CartSerivce
    {
        [WebMethod(EnableSession =true)]//啟用Session
        public static Cart GetCurrentCart()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session["Cart"] == null)
                {
                    var cart = new Cart();
                    HttpContext.Current.Session["Cart"] = cart;
                }

                //回傳Session Cart
                return (Cart)HttpContext.Current.Session["Cart"];
            }
            else
            {
                throw new InvalidOperationException("HttpContext.Current is null");
            }
        }
    }
}