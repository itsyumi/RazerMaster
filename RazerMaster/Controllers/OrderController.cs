using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RazerMaster.Services;
using RazerMaster.Models.Order;
using ECPay.Payment.Integration;
using RazerMaster.Helper;
using RazerMaster.Areas.Member.Repositories;
using log4net;
using RazerMaster.Areas.Member.Services;
using RazerMasterLibrary.Repositories;

namespace RazerMaster.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        public CoinRepository _repo = new CoinRepository();
        public OrderHelper orderHelper = new OrderHelper();
        public MemberinfoRepository menRepo = null;
        public Pay pay = new Pay();
        static ILog logger = LogManager.GetLogger("Web");

        // GET: Order
        [NoCache]
        public ActionResult Index()
        {
            var cart = CartSerivce.GetCurrentCart();
            //判斷購物車內是否有商品
            if (cart.Count() > 0)
            {
                ViewBag.ShipWayOptions = orderHelper.GetShipWayOptions();
                ViewBag.PaymentOptions = orderHelper.GetPaymentOptions();

                //最多折抵$10
                var maxDiscount = 10;
                var coinDiscount = _repo.GetMemberCurrentCoin(User.Identity.GetUserId()) > maxDiscount ? maxDiscount : _repo.GetMemberCurrentCoin(User.Identity.GetUserId());
                ViewBag.CoinDiscount = coinDiscount;

                var userId = User.Identity.GetUserId();
                menRepo = new MemberinfoRepository(userId);

                ViewBag.Name = menRepo.GetRealName();
                ViewBag.Phone = menRepo.GetNumber();
                ViewBag.Address = menRepo.GetAddress();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Product");
            }

        }



        [HttpPost]
        public ActionResult PostToECPay(Ship ship)
        {

            ViewBag.ShipWayOptions = orderHelper.GetShipWayOptions();
            ViewBag.PaymentOptions = orderHelper.GetPaymentOptions();

            if (this.ModelState.IsValid)
            {
                //取得目前購物車
                var currentcart = CartSerivce.GetCurrentCart();

                //最多折抵$10
                var maxDiscount = 10;
                var coinDiscount = _repo.GetMemberCurrentCoin(User.Identity.GetUserId()) > maxDiscount ? maxDiscount : _repo.GetMemberCurrentCoin(User.Identity.GetUserId());
                decimal usedCoin = 0;
                //如果有勾選雷蛇幣折抵且訂單總金額大於折扣數時
                if (ship.CoinDiscount)
                {
                    if (currentcart.TotalAmount < coinDiscount)
                    {
                        return Content("Coin discount exceeds order payment");
                    }
                    usedCoin = coinDiscount;


                }


                var userId = HttpContext.User.Identity.GetUserId();
                string hostname = this.Request.Url.Authority;
                var route = orderHelper.GetRoute(hostname);
                route.PaymentWay = ship.PaymentWay;

                var orderId = pay.CreateOrder(userId, currentcart, ship, usedCoin);
                currentcart.ClearCart();

                var payResult = pay.ProcessGreenPay(orderId, route);
                return Content(payResult);
            }
            return View("index");

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CallBack()
        {
            string result = pay.PaymentCompleted();
            logger.Info(result);
            Response.Write(result);
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Home", "");

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult OrderSucess()
        {
            string result = pay.FeekbackGreenPay();
            logger.Info(result);
            return RedirectToAction("Index", "Home", "");
        }

    }

}