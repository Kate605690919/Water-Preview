using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaterPreview.Base;
using WaterPreview.Other;
using WaterPreview.Other.Attribute;
using WaterPreview.Other.Client;
using WaterPreview.Redis;
using WaterPreview.Service;
using WaterPreview.Service.Interface;
using WaterPreview.Service.Service;

namespace WaterPreview.Controllers
{
    public class HomeController : Controller
    {
        private IAccountService accountService;

        public HomeController()
        {
            accountService = new AccountService();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.Exception = false;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string userName, string password)
        {
            User_t user = accountService.GetAccountByName(userName);
            if (user.Usr_UId == new Guid() || user.Usr_Password != MD5_Util.MD5Encrypt(password))
            {
                ViewBag.Exception = true;
                return View();
            }
            password = MD5_Util.MD5Encrypt(password);

            Response.Cookies[ConfigurationManager.AppSettings["CookieName"]].Value = user.Usr_UId.ToString();
            Response.Cookies[ConfigurationManager.AppSettings["CookieName"]].Domain = ConfigurationManager.AppSettings["DomainName"];
            Response.Cookies[ConfigurationManager.AppSettings["CookieName"]].Expires = DateTime.Now.AddDays(1);

            //HttpClientCrant client = new HttpClientCrant();
            //client.Call_WebAPI_By_Resource_Owner_Password_Credentials_Grant(userName,password);

            UserContext.account = user;
            return RedirectToAction("index");
            //return View("index");
        }       

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult LogOut()
        {
            User_t account = UserContext.account;
            account = account != null && account.Usr_UId != new Guid() ? account : new User_t();
            HttpCookie cookie = Request.Cookies[ConfigurationManager.AppSettings["CookieName"]];
            if (cookie != null)
            {
                cookie.Domain = ConfigurationManager.AppSettings["DomainName"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login", "Home");
        }
    }
}