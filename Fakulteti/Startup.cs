using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fakulteti.Startup))]
namespace Fakulteti
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
