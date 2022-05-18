using RazerMaster.Areas.Member.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;
using RazerMaster.Areas.Member.Models;
using GoogleRecaptcha;
using System;
using RazerMaster.Areas.Member.Services;
using System.Net;

namespace RazerMaster.Areas.Member.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public MemberController()
        {
        }

        public MemberController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        // GET: /Member/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var memberId = User.Identity.GetUserId();
            if (memberId != null)
                return RedirectToAction("Index", "Home", new { area = "" });
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Member/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [NoCache]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //信箱認證要通過
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    ViewBag.errorMessage = "You must have a confirmed email to log on.";
                    return View("Error");
                }
            }
            // 這不會計算為帳戶鎖定的登入失敗
            // 若要啟用密碼失敗來觸發帳戶鎖定，請變更為 shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    //Tim加入Cookie
                    HttpCookie cookie = new HttpCookie("userID");
                    cookie.Value = model.Email;
                    cookie.Expires.AddMinutes(30);
                    cookie.HttpOnly = true;//設定HttpOnly可有效降低 XSS 的影響並提升攻擊難度
                    Response.Cookies.Add(cookie);
                    //Tim加入CookieEnd
                    if (returnUrl != null && returnUrl.Contains("Comment"))
                        return RedirectToAction("Index", "Product", new { area = "" });

                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "登入嘗試失試。");
                    return View(model);
            }
        }


        //
        // GET: /Member/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var memberId = User.Identity.GetUserId();
            if (memberId != null)
                return RedirectToAction("Index", "Home", new { area = "" });
            return View();
        }

        //
        // POST: /Member/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [NoCache]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            IRecaptcha<RecaptchaV2Result> recaptcha = new RecaptchaV2(new RecaptchaV2Data() { Secret = "6Ld44cIZAAAAAOdhxet9iFtxJLhps0_25XJ7pvQH" });

            // Verify the captcha
            var RecaptchaV2result = recaptcha.Verify();
            if (RecaptchaV2result.Success) // Success!!!
            {
                if (ModelState.IsValid)
                {

                    var user = new ApplicationUser { RealName = model.RealName, UserName = model.Email, Email = model.Email, NickName = model.NickName, ResigiterDate = DateTime.Now };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        // 如需如何進行帳戶確認及密碼重設的詳細資訊，請前往 https://go.microsoft.com/fwlink/?LinkID=320771
                        // 傳送包含此連結的電子郵件
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Member", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        await UserManager.SendEmailAsync(user.Id, "RazerMaster Member confirm", "<a type='button' href=\"" + callbackUrl + "\">Click this</a>");
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        //登出時始刪除Cookie
                        Response.Cookies["userID"].Expires = DateTime.Now.AddDays(-1);

                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                    AddErrors(result);
                }
            }

            return View();

        }

        //
        // GET: /Member/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Member/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            var memberId = User.Identity.GetUserId();
            if (memberId != null)
                return RedirectToAction("Index", "Home", new { area = "" });
            return View();
        }

        //
        // POST: /Member/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // 不顯示使用者不存在或未受確認
                    return View("ForgotPasswordConfirmation");
                }

                // 如需如何進行帳戶確認及密碼重設的詳細資訊，請前往 https://go.microsoft.com/fwlink/?LinkID=320771
                // 傳送包含此連結的電子郵件
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackurl = Url.Action("ResetPassword", "Member", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                await UserManager.SendEmailAsync(user.Id, "重設密碼", "請按 <a href=\"" + callbackurl + "\">這裏</a> 重設密碼");
                return RedirectToAction("ForgotPasswordConfirmation", "Member");
            }

            // 如果執行到這裡，發生某項失敗，則重新顯示表單
            return View(model);
        }

        //
        // GET: /Member/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            var memberId = User.Identity.GetUserId();
            if (memberId != null)
                return RedirectToAction("Index", "Home", new { area = "" });
            return View();
        }

        //
        // GET: /Member/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code)
        {
            if (code == null || userId == null)
                return View("Error");

            return View();
        }



        //
        // POST: /Member/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [NoCache]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await UserManager.ResetPasswordAsync(model.UserId, model.Code, model.Password);
            if (result.Succeeded)
            {
                string newCode = await UserManager.GeneratePasswordResetTokenAsync(model.UserId);
                return RedirectToAction("ResetPasswordConfirmation", "Member");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Member/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Member/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 要求重新導向至外部登入提供者
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Member", new { ReturnUrl = returnUrl }));
        }



        // GET: /Member/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // 若使用者已經有登入資料，請使用此外部登入提供者登入使用者
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    // 若使用者沒有帳戶，請提示使用者建立帳戶
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    var check_user = await UserManager.FindByNameAsync(loginInfo.Email);
                    if (check_user != null)
                    {
                        ViewBag.String = "This mailbox has been registered, please log in and link this account from member management";
                        return View("ExternalLoginFailure");
                    }
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Member/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage", "Member");
            }

            if (ModelState.IsValid)
            {


                // 從外部登入提供者處取得使用者資訊
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    ViewBag.String = "服務的登入失敗。";
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { RealName = model.RealName, UserName = model.Email, NickName = model.Nick, Email = model.Email, ResigiterDate = DateTime.Now };
                var result = await UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Member/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoCache]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //登出時始刪除Cookie
            Response.Cookies["userID"].Expires = DateTime.Now.AddDays(-1);

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        //
        // GET: /Member/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
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
                if (error.Contains("名稱"))
                    continue;
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}