using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterPreview.Service;
using WaterPreview.Service.Interface;
using WaterPreview.Service.Service;

namespace WaterPreview.Other.Attribute
{
    public class LoginAttribute : ActionFilterAttribute
    {

        static IAccountService accountservice = new AccountService();

        public bool IsCheck { get; set; }
        //执行Action之前操作
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (IsCheck)
            {
                if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true))
                {
                    return;
                }
                HttpCookieCollection CookieCollect = System.Web.HttpContext.Current.Request.Cookies;
                if (CookieCollect.Count == 0 || CookieCollect["username"] == null)
                {
                    filterContext.Result = new RedirectResult("/Home/Login");
                }
                else if (CookieCollect.Count != 0 && CookieCollect["username"] != null)
                {

                    string s = CookieCollect["username"].Value;

                    //IAccountService accountservice = new AccountService();
                    User_t user = accountservice.GetAccountByUid(Guid.Parse(s));
                    if (user.Usr_UId == new Guid())
                    {

                        filterContext.Result = new RedirectResult("/Home/Login");
                    }
                    UserContext.account = user;
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Home/Login");

                }

            }
        }
    }
}