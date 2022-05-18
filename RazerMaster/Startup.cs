using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RazerMaster.Startup))]
namespace RazerMaster
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
