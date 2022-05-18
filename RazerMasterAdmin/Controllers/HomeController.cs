using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using RazerMasterLibrary.Repositories;
using RazerMasterLibrary.Models;
using RazerMasterLibrary.Repositories.Interface;
using Newtonsoft.Json;

namespace RazerMasterAdmin.Controllers
{
    public class HomeController : Controller
    {
        private IGenericRepository<Products> _productRepo;
        private IGenericRepository<AdminUsers> _adminUserRepo;
        private IGenericRepository<Orders> _orderRepo;
        public ChartRepository _repo;

        public HomeController()
        {
            _productRepo = new GenericRepository<Products>();
            _adminUserRepo = new GenericRepository<AdminUsers>();
            _orderRepo = new GenericRepository<Orders>();
            _repo = new ChartRepository();
        }


        [Authorize]
        public ActionResult Index()
        {
            ViewBag.ProductList = _productRepo.GetAll().Where(x => x.Status == 1 && x.StartSellTime <= DateTime.Now && x.EndSellTime >= DateTime.Now).ToList();
            ViewBag.OrderList = _orderRepo.GetAll().ToList();

            //為null時要賦值
            ViewBag.Revenue = _orderRepo.GetAll().Select(x => x.TotalPrice).DefaultIfEmpty(0).Sum();

            ViewBag.MemberCount = _repo.GetMemberCount();
            ViewBag.CategoryTotalSale = _repo.GetCategoryTotalSale();
            ViewBag.Top5Orders = _repo.Top5OrdersChart();
            ViewBag.PurchaseANLS = _repo.PurchaseANLSChart();
            ViewBag.CountANLS = _repo.PieCharted();

            string JsonMonthANLS = JsonConvert.SerializeObject(_repo.MonthTotaled());
            ViewBag.JsonMonthANLS = JsonMonthANLS;

            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Account, string Password)
        {

            //去資料庫抓取輸入的帳號密碼
            var r = _adminUserRepo.GetAll().SingleOrDefault(x => x.Account == Account && x.Password == Password);

            if (r == null)
            {
                TempData["Error"] = "Your account or password is error!";
                return View();
            }

            // 登入時清空所有 Session 資料
            Session.RemoveAll();

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
              r.Account,//你想要存放在 User.Identy.Name 的值，通常是使用者帳號
              DateTime.Now,
              DateTime.Now.AddMinutes(30),
              false,//將管理者登入的 Cookie 設定成 Session Cookie
              r.Rank.ToString(),//userdata
              FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(ticket);

            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}