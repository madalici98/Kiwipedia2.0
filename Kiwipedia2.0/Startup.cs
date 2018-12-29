using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kiwipedia2._0.Startup))]
namespace Kiwipedia2._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
