using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RazerMaster.Areas.Member.Models;
using RazerMaster.Areas.Member.ViewModels;
//using RazerMasterAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RazerMaster.ViewModels;
using RazerMaster.Services;
using RazerMaster.Helper;
using RazerMasterLibrary.Repositories;
using RazerMasterLibrary.Models;
using Newtonsoft.Json;

namespace RazerMaster.Areas.Member.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public Pay pay = new Pay();
        public OrderHelper orderHelper = new OrderHelper();
        public CoinRepository _repo;
        public GenericRepository<Orders> orderrepo = new GenericRepository<Orders>();
        public GenericRepository<OrderDetails> detailsrepo = new GenericRepository<OrderDetails>();
        public GenericRepository<Coupons> Couponsrepo = new GenericRepository<Coupons>();

        public ManageController()
        {
            _repo = new CoinRepository();
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //



        public async Task<ActionResult> SetMember()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            MemberDetail model = new MemberDetail()
            {
                RealName = user.RealName,
                Address = user.Address,
                NickName = user.NickName,
                Number = user.PhoneNumber
            };

            ViewBag.CurrentCoin = _repo.GetMemberCurrentCoin(User.Identity.GetUserId());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetMember(MemberDetail model)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            model.RealName = user.RealName;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (user == null)
            {
                //return RedirectToAction("Index", new { Message = ManageMessageId.Error });
                ViewBag.StatusMessage = "Error";
                return View(model);
            }
            user.PhoneNumber = model.Number;
            user.NickName = model.NickName;
            user.Address = model.Address;
            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                //return RedirectToAction("Index", new { Message = ManageMessageId.SetMemberSuccess });
                ViewBag.StatusMessage = "Your detail have been saved.";
                return View(model);
            }
            AddErrors(result);
            return View(model);

        }
        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }
        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            if (HasPassword())
            {
                return View();
            }
            else
            {
                return View("SetPassword");
            }
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                //return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                ViewBag.StatusMessage = "Your Password have been saved.";
                return View(model);
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    //return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                    ViewBag.StatusMessage = "Your Password have been saved.";
                    return View(model);
                }
                AddErrors(result);
            }

            // 如果執行到這裡，發生某項失敗，則重新顯示表單
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "已移除外部登入。"
                : message == ManageMessageId.Error ? "發生錯誤。"
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // 要求重新導向至外部登入提供者，以連結目前使用者的登入
            return new MemberController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }
        public ActionResult Failure()
        {
            return View();
        }

        public ActionResult OrderHistory()
        {

            OrderService orderService = new OrderService();
            //RazerMasterDataContext db = new RazerMasterDataContext();
            var memberId = User.Identity.GetUserId();
            var order = orderService.GetOrderHistoryViewModel(memberId);
            //var orderList = orderrepo.GetAll().Where(x => x.UserID == memberId).OrderByDescending(x => x.OrderTime).ToList();
            //var orderDetailList = detailsrepo.GetAll().Where(x => orderList.Where(y => x.OrderID == y.OrderID).Any());


            return View(order);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Coupon(string code)
        {
            //RazerMasterDataContext db = new RazerMasterDataContext();
            var Coupon = Couponsrepo.Get(x => x.Code == code);
            return Json(Coupon);
        }

        [ChildActionOnly]
        public ActionResult ConfirmAgain()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ConfirmAgain(Guid orderId)
        {

            pay.CanCelOrder(orderId);
            return RedirectToAction("OrderHistory", "Manage", "Member");
        }

        [ChildActionOnly]
        public ActionResult Resend()
        {

            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult CouponContent()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Resend(Guid orderId)
        {

            string hostname = this.Request.Url.Authority;
            var route = orderHelper.GetRoute(hostname);
            var result = pay.ProcessGreenPay(orderId, route);

            return Content(result);
        }

        public ActionResult CoinHistory()
        {
            CoinRepository repo = new CoinRepository();
            List<Coins> coinLog = repo.GetMemberCoinLog(User.Identity.GetUserId());


            return View(coinLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
        public ActionResult UserNickName()
        {
            RazerMasterContext userDB = new RazerMasterContext();
            string username = User.Identity.GetUserName();
            string Users = userDB.Users.Where(x => x.UserName == username).First().NickName;
            return Content(Users);
        }



        #region Helper
        // 新增外部登入時用來當做 XSRF 保護
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            SetMemberSuccess,
            Error
        }

        #endregion
    }
}