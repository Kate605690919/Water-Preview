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
        //[Ninject.Inject]
        private IAccountService accountService;
        private IRoleService roleService;
        private IUserInnerRoleService userInnerRoleService;

        public HomeController()
        {
            //this.AddDisposableObject(accService);
            accountService = new AccountService();
            roleService = new RoleService();
            userInnerRoleService = new UserInnerRoleService();
        }

    //    public HomeController(){

    //}
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.Exception = false;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Login(string userName, string password)
        {
            JsonResult result= new JsonResult();

            User_t user = accountService.GetAccountByName(userName);
            if (user.Usr_UId == new Guid() || user.Usr_Password != MD5_Util.MD5Encrypt(password))
            {
                ViewBag.Exception = true;
                result.Data = false;
                return result;
            }
            password = MD5_Util.MD5Encrypt(password);

            Response.Headers.Add("username", user.Usr_Name);
            Response.Headers.Add("useruid", user.Usr_UId.ToString());
            //Response.Cookies[ConfigurationManager.AppSettings["CookieName"]].Value = user.Usr_UId.ToString();
            //Response.Cookies[ConfigurationManager.AppSettings["CookieName"]].Domain = ConfigurationManager.AppSettings["DomainName"];
            //Response.Cookies[ConfigurationManager.AppSettings["CookieName"]].Expires = DateTime.Now.AddDays(1);

            //Response.Cookies["username"].Value = user.Usr_Name;
            //Response.Cookies["username"].Domain = ConfigurationManager.AppSettings["DomainName"];
            //Response.Cookies["username"].Expires = DateTime.Now.AddDays(1);
            HttpClientCrant client = new HttpClientCrant();
            client.Call_WebAPI_By_Resource_Owner_Password_Credentials_Grant(userName,password);

            UserContext.account = user;
            //return RedirectToAction("index");
            //return View("index");
            var userInnerRoles = userInnerRoleService.GetByUid(user.Usr_UId);
            List<InnerRole_t> roleLists = new List<InnerRole_t>();
            foreach (var item in userInnerRoles)
            {
                var role = roleService.GetRoles(item.UIr_IrUId);
                if(role != null)
                {
                    roleLists.Add(role);
                }  
            }
            var names = new List<String>();
            foreach (var item in roleLists)
            {
                var name = item.Ir_Name;
                names.Add(name);
            }
            //var roles = new List<String>();
            RoleHelper.Role personalRole = new RoleHelper.Role();
            foreach (var item in names)
            {
                switch (item)
                {
                    case "总查看员":
                        RoleHelper.GetAllPermission(personalRole);
                        break;
                    case "流量计查看员":
                        RoleHelper.GetFlowMeterViewPermission(personalRole);
                        break;
                    case "流量计管理员":
                        RoleHelper.GetClientManagePermission(personalRole);
                        break;
                    case "压力计查看员":
                        RoleHelper.GetPressureMeterViewPermission(personalRole);
                        break;
                    case "压力计管理员":
                        RoleHelper.GetPressureMeterManagePermission(personalRole);
                        break;
                    case "水质计查看员":
                        RoleHelper.GetQualityMeterViewPermission(personalRole);
                        break;
                    case "水质计管理员":
                        RoleHelper.GetQualityMeterManagePermission(personalRole);
                        break;
                    case "区域查看员":
                        RoleHelper.GetAreaViewPermission(personalRole);
                        break;
                    case "区域管理员":
                        RoleHelper.GetAreaManagePermission(personalRole);
                        break;
                    case "客户查看员":
                        RoleHelper.GetClientViewPermission(personalRole);
                        break;
                    case "客户管理员":
                        RoleHelper.GetClientManagePermission(personalRole);
                        break;
                    case "职员查看员":
                        result.Data = RoleHelper.GetStaffViewPermission(personalRole);
                        break;
                    case "职员管理员":
                        RoleHelper.GetStaffManagePermission(personalRole);
                        break;
                    case "职位查看员":
                        RoleHelper.GetRolesViewPermission(personalRole);
                        break;
                    case "职位管理员":
                        RoleHelper.GetRolesManagePermission(personalRole);
                        break;
                }
            }
            result.Data = personalRole;
            return result;
        } 
      
        [Login(IsCheck=true)]
        [AllowAnonymous]
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