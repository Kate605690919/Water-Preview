using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WaterPreview.Other;
using WaterPreview.Other.Authorization;

namespace WaterPreview
{
    public partial class Startup
    {
         // 有关配置身份验证的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            ConfigAuth(app);
            //app.UseCors(CorsOptions.AllowAll);
            //app.UseOAuthAuthorizationServer(OAuthOptions);
            //app.UseOAuthBearerAuthentication(new IOAuthBearerAuthenticationProvider());
        }

        public void ConfigAuth(IAppBuilder app)
        {
              var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                //RefreshTokenProvider = new RefreshTokenProvider()
            };

            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}