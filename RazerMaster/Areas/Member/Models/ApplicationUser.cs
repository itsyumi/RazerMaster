using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace RazerMaster.Areas.Member.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string Address { get; set; }
        public string NickName { get; set; }
        public DateTime? ResigiterDate { get; set; }
        public string RealName { get; set; }
        //public DateTime? Birthday { get; set; }
        public bool MemberStatus { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {


            // 注意 authenticationType 必須符合 CookieAuthenticationOptions.AuthenticationType 中定義的項目
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // 在這裡新增自訂使用者宣告
            userIdentity.AddClaim(new Claim("CustomName", this.NickName));
            return userIdentity;
        }
    }
}