using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BeerAppreciation.Web.Startup))]

namespace BeerAppreciation.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureSignalR(app);
        }
    }
}
