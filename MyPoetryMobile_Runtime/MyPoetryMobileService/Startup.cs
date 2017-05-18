using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MyPoetryMobileService.Startup))]

namespace MyPoetryMobileService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}