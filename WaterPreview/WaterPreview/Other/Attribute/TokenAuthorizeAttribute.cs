using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using WaterPreview.Redis;

namespace WaterPreview.Other.Attribute
{
    public class TokenAuthorizeAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = HttpContext.Current.Request;
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                ||request.HttpMethod=="OPTIONS")
            {
                return;
            }

            if (System.Configuration.ConfigurationManager.AppSettings["check_token"] == "true")
            {
                string token="";
                var webtoken = request.Headers.Get("access_token");
                if(webtoken!=null)
                {
                    if(UserContext.access_token==null){
                        Func<Guid> tokenvalueFunc = () => { return UserContext.account.Usr_UId; };
                        Guid userUid = DBHelper.getAndFreshT<Guid>(tokenvalueFunc, "token-" + webtoken);
                        if(userUid==new Guid()){
                             filterContext.Result = new RedirectResult("/Home/Login");
                        }
                        UserContext.account.Usr_UId = userUid;
                    }
                    else
                    {
                        token = UserContext.access_token;
                    }
                    if(webtoken.ToString() != token){
                        filterContext.Result = new RedirectResult("/Home/Login");
                    }

                }
                else
                {
                    filterContext.Result = new RedirectResult("/Home/Login");
                }
            }

            base.OnActionExecuting(filterContext);
        }

        //public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        //{

        //    HttpContext.Current.Request.QueryString["Token"];
        //    base.OnAuthorization(actionContext);
        //}
    }
}