using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MultiActiveSorbDirectory.Startup))]
namespace MultiActiveSorbDirectory
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
