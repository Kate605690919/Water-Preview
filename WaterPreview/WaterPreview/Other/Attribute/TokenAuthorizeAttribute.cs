using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace WaterPreview.Other.Attribute
{
    public class TokenAuthorizeAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true))
            {
                return;
            }

            if (System.Configuration.ConfigurationManager.AppSettings["check_token"] == "true")
            {
                var requestHeader = HttpContext.Current.Request.Headers;
                var token = UserContext.access_token;
                var webtoken = requestHeader.Get("access_token");
                if (webtoken == null || webtoken.ToString() != token)
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