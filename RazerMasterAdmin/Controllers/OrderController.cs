using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using RazerMasterAdmin.Services;
using RazerMasterAdmin.ViewModels;
using RazerMasterLibrary.Models;

namespace RazerMasterAdmin.Controllers
{
    public class OrderController : Controller
    {
        private OrderService orderService;

        public OrderController()
        {
            orderService = new OrderService();
        }

        // GET: Order
        [Authorize]
        public ActionResult Index()
        {
            var orderList = orderService.GetAll();
            ViewBag.coupons = orderService.GetAllCoupons();
            return View(orderList);
        }

        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var order = orderService.GetOrderByID(id);
            ViewBag.products = orderService.GetOrderDetailByID(id);
            ViewBag.coupon = orderService.GetCouponCode(order.EventCoupon);
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Orders order)
        {
            ViewBag.products = orderService.GetOrderDetailByID(order.OrderID);
            ViewBag.coupon = orderService.GetCouponCode(order.EventCoupon);
            if (ModelState.IsValid)
            {
                var result = orderService.Update(order);

                if (result.IsSuccessful)
                {
                    TempData["message"] = "Update Success";
                }
                else
                {
                    var path = result.WriteLog();
                    TempData["message"] = "Update Error ref:" + path;
                }

                return RedirectToAction("Index");
            }
            return View(order);
        }
        public ActionResult ChangeStatus(Guid id)
        {
            Orders order = orderService.GetOrderByID(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.Status = order.Status == 0 ? -1 : 0;
            orderService.Update(order);
            return RedirectToAction("Index");
        }
    }
}