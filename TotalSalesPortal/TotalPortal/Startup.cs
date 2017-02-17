using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TotalPortal.Startup))]
namespace TotalPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
