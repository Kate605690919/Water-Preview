using System;
using System.Collections.Generic;
using System.Configuration;
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

            if (IsCheck)
            {
                if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true))
                {
                    return;
                }

                //string uiddd = HttpContext.Current.Session["CookieName"].ToString();

                //string uid = TempDataDictionary["Cookie"].ToString();

                Cookie cookie = new Cookie();
                string uiddd = Cookie.GetCookie(ConfigurationManager.AppSettings["CookieName"]);
                switch (uiddd==null)
                {
                    case true:
                        filterContext.Result = new RedirectResult("/Home/Login");
                        break;
                    default:
                        User_t user = accountservice.GetAccountByUid(Guid.Parse(uiddd.ToString()));
                        if (user.Usr_UId == new Guid())
                        {
                            filterContext.Result = new RedirectResult("/Home/Login");
                        }
                        UserContext.account = user;
                        break;
                }
            }

            base.OnActionExecuting(filterContext);
            
        }
    }
}