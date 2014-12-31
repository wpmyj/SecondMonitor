using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SecondMonitor.Startup))]
namespace SecondMonitor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
