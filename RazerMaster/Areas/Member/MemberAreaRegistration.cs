using System.Web.Mvc;

namespace RazerMaster.Areas.Member
{
    public class MemberAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Member";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.MapRoute(
                "Member_default",
                "Member/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            //context.MapRoute(
            //    "Member_default",
            //    "{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional },
            //    new { controller = "(Member|Manage)" }
            //);
        }
    }
}