using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCTestToken.Startup))]
namespace MVCTestToken
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
