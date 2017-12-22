using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WaterPreview.Service;
using WaterPreview.Service.Interface;
using WaterPreview.Service.RedisContract;
using WaterPreview.Service.Service;

namespace WaterPreview.Other
{
    public class UserContext
    {

        static IAccountService account_service = new AccountService();
        static IAreaUserService areauser_service = new AreaUserService();
        static IAreaDeviceService areadevice_service = new AreaDeviceService();

        private static User_t acc;

        public static User_t account;

        //public static User_t account
        //{
        //    get { return acc; }
        //    set
        //    {
        //        //if (account == null || account.Usr_UId == new Guid())
        //        //{
        //        //    Guid uid = Guid.Parse(System.Web.HttpContext.Current.Session["wp_username"].ToString());
        //        //    acc = account_service.GetAccountByUid(uid);
        //        //}
        //    }
        //}

        public static Guid AreaSourceUid = Guid.Parse("6F6B8DB5-1202-4644-B1B2-A52284D73E07");
        

        public static User_t GetCurrentAccount()
        {
            //Cookie cookie = new Cookie();
            //string uiddd = System.Web.HttpContext.Current.Session["CookieName"].ToString();
            //account = uiddd == null ? new User_t() : account_service.GetAccountByUid(Guid.Parse(uiddd));

            string useruid = Cookie.GetCookie(ConfigurationManager.AppSettings["CookieName"]);
            account = useruid == null ? new User_t() : account_service.GetAccountByUid(Guid.Parse(useruid));
            UserContext.account = account;
            return account;
        }

        public static Guid GetAreaByUserUid(Guid useruid)
        {
            AreaUser_t areauser = areauser_service.GetAllAreaUser().FirstOrDefault(p => p.AU_UserUId==useruid);
            if (areauser == null || areauser.AU_UId == new Guid())
            {
                return AreaSourceUid;
            }
            return areauser.AU_AreaUId;
        }

        
    }
}