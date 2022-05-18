using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Twitter;
using Owin;
using RazerMaster.Areas.Member.Models;


namespace RazerMaster
{
    public partial class Startup
    {
        // 如需設定驗證的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // 設定資料庫內容、使用者管理員和登入管理員，以針對每個要求使用單一執行個體
            app.CreatePerOwinContext(RazerMasterContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // 讓應用程式使用 Cookie 儲存已登入使用者的資訊
            // 並使用 Cookie 暫時儲存使用者利用協力廠商登入提供者登入的相關資訊；
            // 在 Cookie 中設定簽章
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Member/Member/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // 讓應用程式在使用者登入時驗證安全性戳記。
                    // 這是您變更密碼或將外部登入新增至帳戶時所使用的安全性功能。  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // 讓應用程式在雙因素驗證程序中驗證第二個因素時暫時儲存使用者資訊。
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // 讓應用程式記住第二個登入驗證因素 (例如電話或電子郵件)。
            // 核取此選項之後，將會在用來登入的裝置上記住登入程序期間的第二個驗證步驟。
            // 這類似於登入時的 RememberMe 選項。
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // 註銷下列各行以啟用利用協力廠商登入提供者登入
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(new TwitterAuthenticationOptions()
            //{
            //    ConsumerKey = "7ZiqwGrdf1igyYUAZJCNmXbmw",
            //    ConsumerSecret ="GIeMjbl9qPSpJD8qvGfnpI1H8uuSYJzV9J2bcsSpSwPAFAbVso"
            //});
        
            var options = new FacebookAuthenticationOptions
            {
                AppId = "630929474480553",
                AppSecret = "910c2a3bee4d4fb5608b8437caba6773",
            };
            options.Scope.Add("public_profile");
            options.Scope.Add("email");

            //add this for facebook to actually return the email and name
            options.Fields.Add("email");
            options.Fields.Add("name");
            app.UseFacebookAuthentication(options);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "199109713069-ijhd2escq6qce243ots0ai5ku0ltodql.apps.googleusercontent.com",
                ClientSecret = "xDaVpjZB3Zgh95NzXIbU_eOH"
            });
        }
    }
}