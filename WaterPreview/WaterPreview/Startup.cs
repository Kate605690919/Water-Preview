using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WaterPreview.Startup))]
namespace WaterPreview
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
