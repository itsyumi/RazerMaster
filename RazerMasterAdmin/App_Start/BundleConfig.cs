using System.Web;
using System.Web.Optimization;

namespace RazerMasterAdmin
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
           

            bundles.Add(new ScriptBundle("~/templete/js").Include(
                       "~/assets/js/main.js",
                       "~/assets/js/init/fullcalendar-init.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
           

            bundles.Add(new StyleBundle("~/templete/css").Include(
                      "~/assets/css/cs-skin-elastic.css",
                      "~/assets/css/style.css"));
        }
    }
}
